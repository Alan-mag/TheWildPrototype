// Copyright 2019 Niantic, Inc. All Rights Reserved.
namespace Niantic.Platform.Util.OrbitCameraInternal
{
    /// <summary>
    /// Input gesture handled in screen space
    /// </summary>
    public interface IScreenInputGesture : IGesture
    {
        void ProcessEvent(InputEvent inputEvent);
    }
}
