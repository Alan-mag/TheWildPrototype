// Copyright 2019 Niantic, Inc. All Rights Reserved.
namespace Niantic.Platform.Util.OrbitCameraInternal
{
    public interface IZoomCurveEvaluator
    {
        float GetAngleFromDistance(float distance);
        float GetElevationFromDistance(float distance);
        float GetDistanceFromZoomFraction(float zoomFraction);

        void SetCameraParameters(NianticCameraParameters cameraParameters);
    }
}
