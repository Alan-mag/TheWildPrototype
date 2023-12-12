// Copyright 2019 Niantic, Inc. All Rights Reserved.
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Niantic.Platform.Util.OrbitCameraInternal
{
    public class UnityMouseInput : IInputSource
    {
        private const uint MAX_MOUSE_BUTTONS = 6;
        private readonly List<InputEvent> _activeEvents = new List<InputEvent>();

        private Vector3? lastPosition;

        public List<InputEvent> Events
        {
            get { return _activeEvents; }
        }

        public InputPhase CurrentMousePhase(int buttonId)
        {
            if (UnityEngine.Input.GetMouseButtonDown(buttonId))
            {
                return InputPhase.Began;
            }
            else if (UnityEngine.Input.GetMouseButton(buttonId))
            {
                return InputPhase.Held;
            }
            else if (UnityEngine.Input.GetMouseButtonUp(buttonId))
            {
                return InputPhase.Ended;
            }

            return InputPhase.Hovered;
        }

        private EventSystem uiEventSystem;

        public void SetEventSystem(EventSystem eventSystem)
        {
            uiEventSystem = eventSystem;
        }

        public void CollectInput()
        {
            var mouseDelta = lastPosition.HasValue
                ? (UnityEngine.Input.mousePosition - lastPosition.Value)
                : Vector3.zero;

            var recipWidth = 1.0f / Screen.width;
            var recipHeight = 1.0f / Screen.height;

            var mousePositionNormalized = new Vector3(
                UnityEngine.Input.mousePosition.x * recipWidth,
                UnityEngine.Input.mousePosition.y * recipHeight
            );
            var mouseDeltaNormalized = new Vector3(
                mouseDelta.x * recipWidth,
                mouseDelta.y * recipHeight
            );

            var mouseOverUI = uiEventSystem.IsPointerOverGameObject();

            _activeEvents.Clear();
            for (int i = 0; i < MAX_MOUSE_BUTTONS; ++i)
            {
                var phase = CurrentMousePhase((int)i);

                // skip sending hovered events for all the buttons
                if (i == 0 || phase != InputPhase.Hovered)
                {
                    _activeEvents.Add(
                        new InputEvent(
                            this,
                            phase,
                            Time.unscaledTime,
                            Time.unscaledDeltaTime,
                            UnityEngine.Input.mouseScrollDelta,
                            new TransformData(
                                (uint)i,
                                mousePositionNormalized,
                                mouseDeltaNormalized,
                                mouseOverUI
                            )
                        )
                    );
                }
            }

            lastPosition = UnityEngine.Input.mousePosition;
        }
    }
}
