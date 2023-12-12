// Copyright 2019 Niantic, Inc. All Rights Reserved.
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Niantic.Platform.Util.OrbitCameraInternal
{
    public class UnityTouchInput : IInputSource
    {
        private readonly List<InputEvent> activeEvents = new List<InputEvent>();

        public List<InputEvent> Events
        {
            get { return activeEvents; }
        }

        private EventSystem uiEventSystem;
        private readonly Dictionary<int, Touch> activeTouches = new Dictionary<int, Touch>();

        public void SetEventSystem(EventSystem eventSystem)
        {
            uiEventSystem = eventSystem;
        }

        private InputPhase ToInputPhase(TouchPhase phase)
        {
            switch (phase)
            {
                case TouchPhase.Began:
                    return InputPhase.Began;
                case TouchPhase.Canceled:
                    return InputPhase.Canceled;
                case TouchPhase.Ended:
                    return InputPhase.Ended;
                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    return InputPhase.Held;
                default:
                    throw new NotImplementedException("Unexpected UnityEngine.TouchPhase " + phase);
            }
        }

        public void CollectInput()
        {
            activeEvents.Clear();
            var recipWidth = 1.0f / Screen.width;
            var recipHeight = 1.0f / Screen.height;
            for (int i = 0; i < UnityEngine.Input.touchCount; i++)
            {
                var touch = UnityEngine.Input.GetTouch(i);
                // find prev touch of this finger id
                // if it exists, create delta position
                var deltaPos = new Vector3();
                Touch prevTouch;
                if (activeTouches.TryGetValue(touch.fingerId, out prevTouch))
                {
                    deltaPos = touch.position - prevTouch.position;
                }
                activeEvents.Add(
                    new InputEvent(
                        this,
                        ToInputPhase(touch.phase),
                        Time.unscaledTime,
                        Time.unscaledDeltaTime,
                        new TransformData(
                            (uint)touch.fingerId,
                            new Vector3(
                                touch.position.x * recipWidth,
                                touch.position.y * recipHeight
                            ),
                            new Vector3(deltaPos.x * recipWidth, deltaPos.y * recipHeight),
                            uiEventSystem.IsPointerOverGameObject(touch.fingerId)
                        )
                    )
                );
                if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved)
                {
                    activeTouches[touch.fingerId] = touch;
                }
                else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    if (activeTouches.ContainsKey(touch.fingerId))
                    {
                        activeTouches.Remove(touch.fingerId);
                    }
                }
            }

            if (UnityEngine.Input.touchCount > 0)
            {
                // Debug.Log(string.Format("GetInput t:{0} c:{1} f:{2}",Time.unscaledTime, UnityEngine.Input.touchCount, Time.frameCount));
                for (int i = 0; i < 4; ++i)
                {
                    if (activeTouches.ContainsKey(i))
                    {
                        Touch t = activeTouches[i];
                        // Debug.Log(string.Format("#{0} {1} {2} {3}", i, t.position.x, t.position.y, t.phase));
                    }
                }
            }
        }
    }
}
