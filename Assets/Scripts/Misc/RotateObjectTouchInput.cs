using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObjectTouchInput : MonoBehaviour
{
    /********Rotation Variables*********/
    [SerializeField] float rotationRate = 0.1f;
    [SerializeField] GameObject targetItem;

    private Vector3 pos = new Vector3();
    private bool wasRotating;
    private RaycastHit hit;
    private int layerMask = (1 << 8) | (1 << 2);

    void Start()
    {
        layerMask = ~layerMask;
    }

    void Update()
    {
        Ray ray = new Ray();
        Touch theTouch = new Touch();

        if (Input.touches.Length > 0)
        {
            pos = Input.touches[0].position;
            theTouch = Input.touches[0];
        }
        if (pos != new Vector3(0, 0, 0))
            ray = Camera.main.ScreenPointToRay(pos);

        if (Physics.Raycast(ray, out hit, 50, layerMask))
        {
            if (Input.touches.Length == 1)
            {
                if (theTouch.fingerId > 0)
                {
                    if (theTouch.phase == TouchPhase.Began)
                    {
                        Debug.Log("input touch began");
                        wasRotating = false;
                    }

                    if (theTouch.phase == TouchPhase.Moved)
                    {
                        Debug.Log("input touch moved");
                        targetItem.transform.Rotate(theTouch.deltaPosition.y * rotationRate, -theTouch.deltaPosition.x * rotationRate, 0, Space.World);
                        wasRotating = true;
                    }
                }

            }
        }
    }
}
