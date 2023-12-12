// Copyright 2019 Niantic, Inc. All Rights Reserved.
namespace Niantic.Platform.Util.OrbitCameraInternal
{
    /// <summary>
    /// Base interface for gestures.
    /// </summary>
    public interface IGesture
    {
        /// <summary>
        /// Called after all input for this frame has been passed to this gesture
        /// </summary>
        void PostProcessInput();
    }
}
