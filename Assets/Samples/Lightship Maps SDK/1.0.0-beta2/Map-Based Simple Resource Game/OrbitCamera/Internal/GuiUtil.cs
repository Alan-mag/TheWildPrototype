// Copyright 2019 Niantic, Inc. All Rights Reserved.
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace Niantic.Platform.Util.OrbitCameraInternal
{
    public static class GuiUtil
    {
        // This is similar to EventSystem.current.IsPointerOverGameObject()
        // But it works properly on mobile
        public static bool IsScreenPositionOverUi(Vector2 inputPos)
        {
            if (EventSystem.current != null)
            {
                var pointer = new PointerEventData(EventSystem.current);
                pointer.position = inputPos;
                var raycastResults = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointer, raycastResults);

                if (raycastResults.Count > 0)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
