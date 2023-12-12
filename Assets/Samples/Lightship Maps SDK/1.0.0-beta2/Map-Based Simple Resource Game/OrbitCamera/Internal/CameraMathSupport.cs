// Copyright 2019 Niantic, Inc. All Rights Reserved.
using UnityEngine;

namespace Niantic.Platform.Util.OrbitCameraInternal
{
    public static class CameraMathSupport
    {
        // TODO(dlecompte): move this code out of camera-specific code into more
        // general-purpose math code.

        public static float Clamp(float value, float minValue, float maxValue)
        {
            if (value < minValue)
            {
                return minValue;
            }

            if (value > maxValue)
            {
                return maxValue;
            }

            return value;
        }

        public static float Map(
            float value,
            float minInValue,
            float maxInValue,
            float minOutValue,
            float maxOutValue
        )
        {
            float inFrac = (value - minInValue) / (maxInValue - minInValue);
            return (maxOutValue - minOutValue) * inFrac + minOutValue;
        }

        public static float WrapAngleDegrees(float degrees)
        {
            while (degrees > 180)
            {
                degrees -= 360;
            }

            while (degrees < -180)
            {
                degrees += 360;
            }

            return degrees;
        }

        public static bool WithinEpsilon(float value, float epsilon)
        {
            return (Mathf.Abs(value) < epsilon);
        }
    }
}
