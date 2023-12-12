// Copyright 2019 Niantic, Inc. All Rights Reserved.
using System.Collections.Generic;

namespace Niantic.Platform.Util.OrbitCameraInternal
{
    public enum InputLayerPriority
    {
        Early = 0,
        Screen = 1000,
        World = 2000,
        Late = 3000
    }

    /// <summary>
    /// InputLayers correspond to input "scenes". For instance, there should be one layer
    /// that handles all the input you would want to process while on the world map.
    /// </summary>
    public interface IInputLayer
    {
        /// <summary>
        /// Layers are processed in Priority order
        /// </summary>
        InputLayerPriority Priority { get; }

        /// <summary>
        /// Called every frame with all the input events that this layer is eligible to handle
        /// </summary>
        void ProcessEvents(List<InputEvent> events);

        /// <summary>
        /// Called when the input layer is removed
        /// </summary>
        void Removed();
    }
}
