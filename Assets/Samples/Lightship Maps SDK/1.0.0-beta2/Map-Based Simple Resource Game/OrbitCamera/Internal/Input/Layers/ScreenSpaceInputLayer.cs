// Copyright 2019 Niantic, Inc. All Rights Reserved.
using System.Collections.Generic;
using System.Linq;

namespace Niantic.Platform.Util.OrbitCameraInternal
{
    public interface IScreenSpaceInputLayer : IInputLayer
    {
        void Register(IScreenInputGesture gesture);
    }

    public class ScreenSpaceInputLayer : IScreenSpaceInputLayer
    {
        private readonly List<IScreenInputGesture> gestures = new List<IScreenInputGesture>();

        public InputLayerPriority Priority { get; private set; }

        public ScreenSpaceInputLayer(InputLayerPriority priority)
        {
            Priority = priority;
        }

        public ScreenSpaceInputLayer() : this(InputLayerPriority.Screen) { }

        public void ProcessEvents(List<InputEvent> events)
        {
            foreach (var gesture in gestures)
            {
                foreach (var inputEvent in events)
                {
                    if (inputEvent.ConsumedBy == null)
                    {
                        gesture.ProcessEvent(inputEvent);
                    }
                }
                gesture.PostProcessInput();
            }
        }

        public void Register(IScreenInputGesture gesture)
        {
            gestures.Add(gesture);
        }

        public void Removed()
        {
            gestures.Clear();
        }
    }
}
