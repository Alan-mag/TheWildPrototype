using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalInputHandler : MonoBehaviour
{
    [SerializeField]
    Camera mainCamera;

    private void Update()
    {
        var pos = new Vector3();

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            pos = Input.mousePosition;
        }
#else
if (Input.touches.Length > 0)
        {
            pos = Input.touches[0].position;
        }
#endif
        if (pos != new Vector3(0, 0, 0))
            OnTouchScreen(pos);
    }

    void OnTouchScreen(Vector3 position)
    {
        Ray ray = mainCamera.ScreenPointToRay(position);

        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            var gameObjectHitWithRaycast = hitInfo.collider.gameObject;

            if (gameObjectHitWithRaycast.name.Contains("Thread"))
            {
                var threadScript = gameObjectHitWithRaycast.GetComponent<ThreadObject>();
                threadScript.CollectThread();
            }
        }
    }
}
