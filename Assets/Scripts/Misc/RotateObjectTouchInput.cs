using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObjectTouchInput : MonoBehaviour
{
    /********Rotation Variables*********/
    [SerializeField] float rotationRate = 0.1f;
    [SerializeField] GameObject objectToRotate;

    private int layerMask = (1 << 8) | (1 << 2);

    void Start()
    {
        layerMask = ~layerMask;
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    Debug.Log("touch phase began");
                    break;
                case TouchPhase.Moved:
                    Debug.Log("touch phase moved");
                    objectToRotate.transform.Rotate(
                        touch.deltaPosition.y * rotationRate,
                        -touch.deltaPosition.x * rotationRate,
                        0,
                        Space.World
                    );
                    break;
                case TouchPhase.Ended:
                    Debug.Log("Touch end");
                    break;
            }
        }
    }
}
