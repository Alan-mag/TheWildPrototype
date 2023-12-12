using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Niantic.Lightship.Utilities;
using System;
using UnityEditor;

// Todo: this just handles input, might be fair to change script name at some point?
public class PuzzleSphere : MonoBehaviour
{
    [SerializeField] PuzzleSceneManager puzzleSceneManager;

    [SerializeField] GameObject targetItem;
    [SerializeField] Camera GUICamera;
    [SerializeField] GameObject ambient;


    /********Rotation Variables*********/
    [SerializeField] float rotationRate = 0.1f;
    [SerializeField] bool wasRotating;

    /************Scrolling inertia variables************/
    [SerializeField] Vector2 scrollPosition = Vector2.zero;
    [SerializeField] float scrollVelocity = 0f;
    [SerializeField] float timeTouchPhaseEnded;
    [SerializeField] float inertiaDuration = 0.5f;

    [SerializeField] float itemInertiaDuration = 1.0f;
    [SerializeField] float itemTimeTouchPhaseEnded;
    [SerializeField] float rotateVelocityX = 0;
    [SerializeField] float rotateVelocityY = 0;

    [Header("Puzzle Components")]
    [SerializeField] GameObject creationObject;
    [SerializeField] GameObject solutionObject;

    [Header("Puzzle Rotation Solution")]
    [SerializeField] public float xRotation;
    [SerializeField] public float yRotation;
    [SerializeField] public float zRotation;

    private Vector3 pos = new Vector3();


    RaycastHit hit;

    private int layerMask = (1 << 8) | (1 << 2);


    void Start()
    {
        layerMask = ~layerMask;
    }

    // todo this needs to be updated with new input for 3.0
    void Update()
    {
        Ray ray = new Ray();
        Touch theTouch = new Touch();

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
        theTouch = Input.touches[0];
#endif
        if (pos != new Vector3(0, 0, 0))

            ray = Camera.main.ScreenPointToRay(pos);
            // var GUIRayq = GUICamera.ScreenPointToRay(theTouch.position);

            if (Physics.Raycast(ray, out hit, 50, layerMask))
            {

                if (Input.touches.Length == 1)
                {
                if (theTouch.fingerId > 0)
                {
                    if (theTouch.phase == TouchPhase.Began)
                    {
                        wasRotating = false;
                    }

                    if (theTouch.phase == TouchPhase.Moved)
                    {

                        targetItem.transform.Rotate(theTouch.deltaPosition.y * rotationRate, -theTouch.deltaPosition.x * rotationRate, 0, Space.World);
                        wasRotating = true;
                    }
                }
            }
        }
    }
}
