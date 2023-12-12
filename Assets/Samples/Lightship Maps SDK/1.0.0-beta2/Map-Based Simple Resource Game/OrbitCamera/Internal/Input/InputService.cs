// Copyright 2019 Niantic, Inc. All Rights Reserved.
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Niantic.Platform.Util.OrbitCameraInternal
{
    public class InputService : IInputService
    {
        public event Action BackButtonPressed;

        private readonly EventSystem _unityEventSystem;

        private readonly List<IInputSource> _sources = new List<IInputSource>();

        private readonly List<IInputLayer> _layers = new List<IInputLayer>();

        private readonly List<IInputLayer> _layersToAdd = new List<IInputLayer>();
        private readonly List<IInputLayer> _layersToRemove = new List<IInputLayer>();

        private uint _disableCount;

        private readonly List<InputEvent> _tempEventList = new List<InputEvent>();

        public InputService(EventSystem unityEventSystem)
        {
            _unityEventSystem = unityEventSystem;
        }

        public EventSystem MainEventSystem
        {
            get { return _unityEventSystem; }
        }

        private class BackButtonPressedInputLayer : IInputLayer, IGesture
        {
            private readonly InputService _inputService;

            public InputLayerPriority Priority
            {
                get { return InputLayerPriority.Screen; }
            }

            public BackButtonPressedInputLayer(InputService inputService)
            {
                _inputService = inputService;
            }

            public void Removed() { }

            public void PostProcessInput() { }

            public void ProcessEvents(List<InputEvent> events)
            {
                if (!_inputService.EventsEnabled)
                {
                    return;
                }

                if (_inputService.BackButtonPressed != null)
                {
                    foreach (var inputEvent in events)
                    {
                        if (
                            inputEvent.Key.HasValue
                            && inputEvent.Key.Value == KeyCode.Escape
                            && // android passes back button press as escape
                            inputEvent.Phase == InputPhase.Ended
                        )
                        {
                            inputEvent.Consume(this);
                            _inputService.BackButtonPressed();
                        }
                    }
                }
            }
        }

        public bool EventsEnabled
        {
            get { return _disableCount == 0; }
        }

        public void EnableInputEvents(bool enable)
        {
            if (!enable)
            {
                _disableCount++;
            }
            else if (_disableCount != 0)
            {
                _disableCount--;
            }

            _unityEventSystem.enabled = EventsEnabled;
        }

        private void SetupLegacyBackButtonHandler()
        {
            AddLayer(new BackButtonPressedInputLayer(this));
        }

        public void Initialize(IEnumerable<IScreenInputGesture> gestures)
        {
            _sources.Clear();

            _sources.Add(new UnityTouchInput());
            _sources.Add(new UnityMouseInput());

            foreach (IInputSource source in _sources)
            {
                source.SetEventSystem(_unityEventSystem);
            }

            ScreenSpaceInputLayer screenSpaceInputLayer = new ScreenSpaceInputLayer(
                InputLayerPriority.Screen
            );

            foreach (IScreenInputGesture g in gestures)
            {
                screenSpaceInputLayer.Register(g);
            }

            AddLayer(screenSpaceInputLayer);
        }

        public void Update()
        {
            foreach (var source in _sources)
            {
                source.CollectInput();
            }

            ProcessInput();
        }

        public void AddLayer(IInputLayer layer)
        {
            _layersToAdd.Add(layer);
        }

        private void AddPendingLayers()
        {
            if (_layersToAdd.Count == 0)
            {
                return;
            }

            foreach (var layerToAdd in _layersToAdd)
            {
                _layers.Add(layerToAdd);
            }

            _layers.Sort(LayerComparer);
            _layersToAdd.Clear();
        }

        int LayerComparer(IInputLayer l1, IInputLayer l2)
        {
            return l1.Priority.CompareTo(l2.Priority);
        }

        public void RemoveLayer(IInputLayer layer)
        {
            _layersToRemove.Add(layer);
        }

        private void RemovePendingLayers()
        {
            if (_layersToRemove.Count == 0)
            {
                return;
            }

            foreach (var layerToRemove in _layersToRemove)
            {
                var removed = _layers.Remove(layerToRemove);
                if (!removed)
                {
                    throw new InvalidOperationException(
                        "Cannot remove input layer that isn't in the InputService"
                    );
                }

                layerToRemove.Removed();
            }

            _layersToRemove.Clear();
        }

        public IEnumerable<IInputSource> Sources
        {
            get { return _sources; }
        }

        private bool EventIsAvailable(InputEvent e)
        {
            return e.ConsumedBy == null;
        }

        private void ProcessInput()
        {
            AddPendingLayers();
            RemovePendingLayers();

            foreach (var layer in _layers)
            {
                if (layer == null)
                {
                    // Debug.LogWarning("Skipping null input layer");
                    continue;
                }

                foreach (var source in _sources)
                {
                    _tempEventList.Clear();
                    foreach (var ev in source.Events)
                    {
                        if (ev != null && EventIsAvailable(ev))
                        {
                            _tempEventList.Add(ev);
                        }
                    }

                    if (_tempEventList.Count > 0)
                    {
                        layer.ProcessEvents(_tempEventList);

                        // Only accept events from a single layer per frame (e.g. let touch
                        // override mouse).
                        return;
                    }
                }
            }
        }

        public bool HasBackButtonAction()
        {
            return BackButtonPressed != null;
        }

        public void FireBackButtonAction()
        {
            if (HasBackButtonAction())
            {
                BackButtonPressed();
            }
        }
    }
}
