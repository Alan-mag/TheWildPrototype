// Copyright 2019 Niantic, Inc. All Rights Reserved.
using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace Niantic.Platform.Util.OrbitCameraInternal
{
    public interface IInputSource
    {
        List<InputEvent> Events { get; }

        void SetEventSystem(EventSystem eventSystem);

        void CollectInput();
    }
}
