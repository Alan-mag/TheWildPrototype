// Copyright 2019 Niantic, Inc. All Rights Reserved.
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Niantic.Platform.Util.OrbitCameraInternal
{
    public interface IInputService : IInputEventService
    {
        /// <summary>
        /// All the sources that were found under this input service
        /// </summary>
        IEnumerable<IInputSource> Sources { get; }

        /// <summary>
        /// Add a new layer to receive input events
        /// </summary>
        void AddLayer(IInputLayer layer);

        /// <summary>
        /// IReceipt.Complete also calls remove layer, no need to do this twice
        /// </summary>
        void RemoveLayer(IInputLayer layer);

        EventSystem MainEventSystem { get; }

        bool EventsEnabled { get; }

        bool HasBackButtonAction();

        void FireBackButtonAction();
    }
}
