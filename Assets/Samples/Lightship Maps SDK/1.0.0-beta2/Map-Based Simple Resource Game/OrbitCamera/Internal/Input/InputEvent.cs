// Copyright 2019 Niantic, Inc. All Rights Reserved.
using System;
using System.Text;
using UnityEngine;

namespace Niantic.Platform.Util.OrbitCameraInternal
{
    /// <summary>
    /// Input event lifecycle descriptor
    /// </summary>
    public enum InputPhase
    {
        Began,
        Held,
        Ended,
        Hovered,
        Canceled
    }

    [Serializable]
    public sealed class InputEvent
    {
        /// <summary>
        /// Unique, sequential ID for this event
        /// </summary>
        public readonly uint Uid;

        /// <summary>
        /// What part of the touch's lifetime is this event occuring in
        /// </summary>
        public readonly InputPhase Phase;

        /// <summary>
        /// The unscaled seconds since startup that this input was created
        /// </summary>
        public readonly float Time;

        /// <summary>
        /// The unscaled seconds delta, if any, associated with this input
        /// </summary>
        public readonly float DeltaTime;

        /// <summary>
        /// Contains positional data such as mouse/touch Position and DeltaPosition,
        /// if applicable to this event
        /// </summary>
        public readonly TransformData? Transform;

        /// <summary>
        /// Contains keycodes for mouse, gamepad and keyboard events
        /// </summary>
        public readonly KeyCode? Key;

        /// <summary>
        /// The source that emitted this event
        /// </summary>
        public readonly IInputSource Source;

        /// <summary>
        /// Scrolling amount if this is an event that contains scrolling
        /// </summary>
        public readonly Vector2? ScrollDelta;

        /// <summary>
        /// Unless this input is consumed, this field will be null
        /// </summary>
        public IGesture ConsumedBy { get; private set; }

        private static uint nextId = 0;

        private InputEvent(
            IInputSource source,
            InputPhase phase,
            float time,
            float deltaTime,
            Vector2? scrollDelta,
            TransformData? transform,
            KeyCode? key
        )
        {
            unchecked
            {
                Uid = nextId++;
            }

            Source = source;
            Phase = phase;
            Transform = transform;
            Key = key;
            Time = time;
            DeltaTime = deltaTime;
            ScrollDelta = scrollDelta;
            ConsumedBy = null;
        }

        public InputEvent(
            IInputSource source,
            InputPhase phase,
            float time,
            float deltaTime,
            TransformData transform
        ) : this(source, phase, time, deltaTime, null, transform, null) { }

        public InputEvent(
            IInputSource source,
            InputPhase phase,
            float time,
            float deltaTime,
            Vector2 scrollDelta,
            TransformData transform
        ) : this(source, phase, time, deltaTime, scrollDelta, transform, null) { }

        public InputEvent(
            IInputSource source,
            InputPhase phase,
            float time,
            float deltaTime,
            KeyCode key
        ) : this(source, phase, time, deltaTime, null, null, key) { }

        public InputEvent(
            IInputSource source,
            InputPhase phase,
            float time,
            float deltaTime,
            TransformData transform,
            KeyCode key
        ) : this(source, phase, time, deltaTime, null, transform, key) { }

        public InputEvent(
            IInputSource source,
            InputPhase phase,
            float time,
            float deltaTime,
            Vector2 scrollDelta,
            TransformData transform,
            KeyCode key
        ) : this(source, phase, time, deltaTime, scrollDelta, transform, (KeyCode?)key) { }

        public void Consume(IGesture consumer)
        {
            if (ConsumedBy != null)
            {
                throw new InvalidOperationException("InputEvent already consumed");
            }

            ConsumedBy = consumer;
        }

        public override string ToString()
        {
            var s = new StringBuilder();

            s.AppendFormat(
                "[InputEvent({0}): Src: {1}, Phase: {2}, T: {3}, dT: {4}",
                Uid,
                Source.GetType().Name,
                Phase,
                Time,
                DeltaTime
            );

            if (Key.HasValue)
            {
                s.Append(", Key:");
                s.Append(Key.Value);
            }

            if (Transform.HasValue)
            {
                s.Append(", Transform: ");
                s.Append(Transform.Value);
            }

            if (ScrollDelta.HasValue)
            {
                s.Append(", Scroll: ");
                s.Append(ScrollDelta.Value.ToString("f2"));
            }

            s.Append("]");

            return s.ToString();
        }
    }

    [Serializable]
    public struct TransformData
    {
        /// <summary>
        /// Id for this positional info. For example, each finger of a touch
        /// event or each mouse button would have different, consistent Ids.
        /// </summary>
        public readonly uint Id;

        /// <summary>
        /// The position, if any, that this input occured at. Often in screen space,
        /// but since the space is assigned by the IInputSource, it may not be.
        /// </summary>
        public readonly Vector3 Position;

        /// <summary>
        /// Delta of the position above
        /// </summary>
        public readonly Vector3 DeltaPosition;

        /// <summary>
        /// Is this transformation occuring over a UI element?
        /// </summary>
        public readonly bool OverUI;

        public TransformData(uint id, Vector3 position, Vector3 deltaPosition, bool overUI)
        {
            Id = id;
            Position = position;
            DeltaPosition = deltaPosition;
            OverUI = overUI;
        }

        public override string ToString()
        {
            return string.Format(
                "[TransformData({0}): Pos: {1}, dPos: {2}, OnUI: {3}]",
                Id,
                Position.ToString("f3"),
                DeltaPosition.ToString("f3"),
                OverUI
            );
        }
    }
}
