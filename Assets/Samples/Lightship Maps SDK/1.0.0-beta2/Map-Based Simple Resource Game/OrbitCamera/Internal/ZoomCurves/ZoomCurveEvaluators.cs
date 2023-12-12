// Copyright 2019 Niantic, Inc. All Rights Reserved.
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Niantic.Platform.Util.OrbitCameraInternal
{
    public class NianticCameraParameters
    {
        public float MinZoomMeters { get; private set; }
        public float MaxZoomMeters { get; private set; }

        public float MinPitchDegrees { get; private set; }
        public float MaxPitchDegrees { get; private set; }

        public float VerticalOffsetMeters { get; private set; } // the height of the camera focus off the ground
        public float MinElevMeters { get; private set; } // includes the vertical offset
        public float MaxElevMeters { get; private set; } // includes the vertical offset

        public NianticCameraParameters(
            float minZoomMeters,
            float maxZoomMeters,
            float minPitchDegrees,
            float maxPitchDegrees,
            float verticalOffsetMeters
        )
        {
            MinZoomMeters = minZoomMeters;
            MaxZoomMeters = maxZoomMeters;
            MinPitchDegrees = minPitchDegrees;
            MaxPitchDegrees = maxPitchDegrees;
            VerticalOffsetMeters = verticalOffsetMeters;
            MinElevMeters =
                verticalOffsetMeters + Mathf.Tan(MinPitchDegrees * Mathf.Deg2Rad) * minZoomMeters;
            MaxElevMeters =
                verticalOffsetMeters + Mathf.Tan(MaxPitchDegrees * Mathf.Deg2Rad) * maxZoomMeters;
        }
    }

    public abstract class AbstractZoomCurveEvaluator : IZoomCurveEvaluator
    {
        protected NianticCameraParameters cameraParameters;

        public void SetCameraParameters(NianticCameraParameters cameraParameters)
        {
            this.cameraParameters = cameraParameters;
        }

        public float GetDistanceFromZoomFraction(float zoomFraction)
        {
            return CameraMathSupport.Map(
                zoomFraction,
                0.0f,
                1.0f,
                cameraParameters.MinZoomMeters,
                cameraParameters.MaxZoomMeters
            );
        }

        public abstract float GetAngleFromDistance(float distance);

        public abstract float GetElevationFromDistance(float distanceMeters);
    }

    public abstract class AbstractAngularZoomCurveEvaluator : AbstractZoomCurveEvaluator
    {
        public override float GetElevationFromDistance(float distanceMeters)
        {
            float angleDegrees = GetAngleFromDistance(distanceMeters);
            float angleRadians = angleDegrees * Mathf.Deg2Rad;
            return cameraParameters.VerticalOffsetMeters + Mathf.Tan(angleRadians) * distanceMeters;
        }
    }

    public class LinearAngleZoomCurveEvaluator : AbstractAngularZoomCurveEvaluator
    {
        public override float GetAngleFromDistance(float distanceM)
        {
            return CameraMathSupport.Map(
                distanceM,
                cameraParameters.MinZoomMeters,
                cameraParameters.MaxZoomMeters,
                cameraParameters.MinPitchDegrees,
                cameraParameters.MaxPitchDegrees
            );
        }
    }

    public class QuadraticAngleZoomCurveEvaluator : AbstractAngularZoomCurveEvaluator
    {
        public override float GetAngleFromDistance(float distanceM)
        {
            float distanceFrac =
                (distanceM - cameraParameters.MinZoomMeters)
                / (cameraParameters.MaxZoomMeters - cameraParameters.MinZoomMeters);

            float yFrac = distanceFrac * distanceFrac;
            float pitchDelta = cameraParameters.MaxPitchDegrees - cameraParameters.MinPitchDegrees;
            return cameraParameters.MinPitchDegrees + yFrac * pitchDelta;
        }
    }

    public class CubicAngleZoomCurveEvaluator : AbstractAngularZoomCurveEvaluator
    {
        private float y0,
            y1,
            y2,
            y3;

        public CubicAngleZoomCurveEvaluator(float y0, float y1, float y2, float y3)
        {
            this.y0 = y0;
            this.y1 = y1;
            this.y2 = y2;
            this.y3 = y3;
        }

        public override float GetAngleFromDistance(float distanceM)
        {
            float x =
                (distanceM - cameraParameters.MinZoomMeters)
                / (cameraParameters.MaxZoomMeters - cameraParameters.MinZoomMeters);

            // Cubic spline equation
            float yFrac = (float)(
                Math.Pow(1.0f - x, 3.0f) * y0
                + 3 * Math.Pow(1.0f - x, 2.0f) * x * y1
                + 3 * (1.0f - x) * x * x * y2
                + x * x * x * y3
            );
            float pitchDelta = cameraParameters.MaxPitchDegrees - cameraParameters.MinPitchDegrees;
            return cameraParameters.MinPitchDegrees + yFrac * pitchDelta;
        }
    }

    public class LinearElevationZoomCurveEvaluator : AbstractElevationZoomCurveEvaluator
    {
        private List<Vector2> controlPoints;

        public LinearElevationZoomCurveEvaluator(List<Vector2> controlPoints)
        {
            this.controlPoints = controlPoints;
        }

        private int FindSpan(float distanceMeters)
        {
            if (distanceMeters < controlPoints[0].x)
            {
                return -1;
            }

            for (int i = 0; i < controlPoints.Count - 1; ++i)
            {
                if (
                    (distanceMeters >= controlPoints[i].x)
                    && (distanceMeters <= controlPoints[i + 1].x)
                )
                {
                    return i;
                }
            }

            return controlPoints.Count;
        }

        public override float GetElevationFromDistance(float distanceMeters)
        {
            int spanIndex = FindSpan(distanceMeters);
            float minDist,
                maxDist,
                minElev,
                maxElev;

            // if distanceMeters is not within the domain supported by our control
            // points, linearly interpolate to get there.

            if (spanIndex < 0)
            {
                // just linearly interpolate to get to first control point
                minDist = cameraParameters.MinZoomMeters;
                maxDist = controlPoints[0].x;
                minElev = cameraParameters.MinElevMeters;
                maxElev = controlPoints[0].y;
            }
            else if (spanIndex > controlPoints.Count - 1)
            {
                // linearly interpolate after end

                int lastIndex = controlPoints.Count - 1;
                minDist = controlPoints[lastIndex].x;
                maxDist = cameraParameters.MaxZoomMeters;
                minElev = controlPoints[lastIndex].y;
                maxElev = cameraParameters.MaxElevMeters;
            }
            else
            {
                minDist = controlPoints[spanIndex].x;
                maxDist = controlPoints[spanIndex + 1].x;
                minElev = controlPoints[spanIndex].y;
                maxElev = controlPoints[spanIndex + 1].y;
            }

            return CameraMathSupport.Map(distanceMeters, minDist, maxDist, minElev, maxElev);
        }
    }

    public abstract class AbstractElevationZoomCurveEvaluator : AbstractZoomCurveEvaluator
    {
        public override float GetAngleFromDistance(float distanceMeters)
        {
            float elevMeters = GetElevationFromDistance(distanceMeters);
            float elevAboveFocusMeters = elevMeters - cameraParameters.VerticalOffsetMeters;
            float angleRadians = Mathf.Atan2(elevAboveFocusMeters, distanceMeters);
            float angleDegrees = Mathf.Rad2Deg * angleRadians;
            return angleDegrees;
        }
    }

    public class CubicHermiteSplineElevationZoomCurveEvaluator : AbstractElevationZoomCurveEvaluator
    {
        private List<Vector2> controlPoints;

        public CubicHermiteSplineElevationZoomCurveEvaluator(List<Vector2> controlPoints)
        {
            this.controlPoints = controlPoints;
        }

        private int findSpan(float distanceMeters)
        {
            if (distanceMeters < controlPoints[0].x)
            {
                return -1;
            }

            for (int i = 0; i < controlPoints.Count - 1; ++i)
            {
                if (
                    (distanceMeters >= controlPoints[i].x)
                    && (distanceMeters <= controlPoints[i + 1].x)
                )
                {
                    return i;
                }
            }

            return controlPoints.Count;
        }

        private float getTangent(int controlIndex)
        {
            // single-sided tangent, pointing from endpoint to adjacent point (for first)

            Vector2 delta;

            if (controlIndex == 0)
            {
                delta = controlPoints[1] - controlPoints[0];
                return delta.y / delta.x;
            }

            // single-sided tangent, pointing from endpoint to adjacent point (for last)
            if (controlIndex == controlPoints.Count - 1)
            {
                delta = controlPoints[controlIndex] - controlPoints[controlIndex - 1];
                return delta.y / delta.x;
            }

            // else, find slope between adjoining points
            Vector2 cpPlus = controlPoints[controlIndex + 1];
            Vector2 cpMinus = controlPoints[controlIndex - 1];

            delta = cpPlus - cpMinus;
            return delta.y / delta.x;
        }

        public override float GetElevationFromDistance(float distanceMeters)
        {
            // see https://en.wikipedia.org/wiki/Cubic_Hermite_spline

            int spanIndex = findSpan(distanceMeters);

            // if distanceMeters is not within the domain supported by our control
            // points, linearly interpolate to get there.

            if (spanIndex < 0)
            {
                // just linearly interpolate to get to first control point
                float minDist = cameraParameters.MinZoomMeters;
                float maxDist = controlPoints[0].x;
                float minElev = cameraParameters.MinElevMeters;
                float maxElev = controlPoints[0].y;

                return CameraMathSupport.Map(distanceMeters, minDist, maxDist, minElev, maxElev);
            }
            else if (spanIndex > controlPoints.Count - 1)
            {
                // linearly interpolate after end

                int lastIndex = controlPoints.Count - 1;
                float minDist = controlPoints[lastIndex].x;
                float maxDist = cameraParameters.MaxZoomMeters;
                float minElev = controlPoints[lastIndex].y;
                float maxElev = cameraParameters.MaxElevMeters;

                return CameraMathSupport.Map(distanceMeters, minDist, maxDist, minElev, maxElev);
            }

            float tangent0 = getTangent(spanIndex);
            float tangent1 = getTangent(spanIndex + 1);
            Vector2 control0 = controlPoints[spanIndex];
            Vector2 control1 = controlPoints[spanIndex + 1];

            float t = CameraMathSupport.Map(distanceMeters, control0.x, control1.x, 0.0f, 1.0f);

            float t2 = t * t;
            float t3 = t2 * t;

            float h00 = 2 * t3 - 3 * t2 + 1;
            float h10 = t3 - 2 * t2 + t;
            float h01 = -2 * t3 + 3 * t2;
            float h11 = t3 - t2;

            float p = h00 * control0.y + h10 * tangent0 + h01 * control1.y + h11 * tangent1;

            return p;
        }
    }
}
