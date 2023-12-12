using Niantic.Lightship.AR.Semantics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;

public class ARInputHandler : MonoBehaviour
{
    [SerializeField]
    private Camera _camera;
    [SerializeField]
    private ARInteractionEventChannelSO _arInteractionEventChannel;
    [SerializeField]
    private ARInteractionAudioLogEventChannelSO _arInteractionAudioLogEventChannel;
    [SerializeField]
    private ARSemanticSegmentationManager segmentationManager;
    [SerializeField]
    private AROcclusionManager occlusionManager;

    private PlayerInput _lightshipInput;
    private InputAction _primaryTouch;

    private void Awake()
    {
        _lightshipInput = GetComponent<PlayerInput>();
        _primaryTouch = _lightshipInput.actions["Point"];
    }

    private void Update()
    {
        HandleTouch();
    }

    private void HandleTouch()
    {
        if (!_primaryTouch.WasPerformedThisFrame())
        {
            return;
        }
        else
        {
            Ray ray = _camera.ScreenPointToRay(_primaryTouch.ReadValue<Vector2>());
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && 
                !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                var gameObjectHitWithRaycast = hit.collider.gameObject;
                HandleARObjectEventChannelInvocation(gameObjectHitWithRaycast, hit);
            }
        }

    }

    private void HandleARObjectEventChannelInvocation(GameObject gObject, RaycastHit hit)
    {
        if (gObject.name.Contains("Image"))
        {
            _arInteractionEventChannel.RaiseEvent(gObject.name, hit.point);
        }

        if (gObject.name.Contains("Audio"))
        {
            _arInteractionAudioLogEventChannel.RaiseEvent(gObject.GetComponent<ARInteractiveObject>().id, gObject.name, hit.point);
        }
        
    }

    public void HandleSwitchToScanMode()
    {
        occlusionManager.enabled = false;
        StartCoroutine(SwitchToScan(3.0f));
    }

    IEnumerator SwitchToScan(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        segmentationManager.enabled = true;
        segmentationManager.GetComponentInParent<SemanticQuerying>().StartScan();
    }

    public void HandleSwitchToOcclusionMode()
    {
        segmentationManager.enabled = false;
        StartCoroutine(SwitchToOcclusion(3.0f));
    }

    IEnumerator SwitchToOcclusion(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        segmentationManager.enabled = true;
    }
}
