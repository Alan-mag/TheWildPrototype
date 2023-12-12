using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDetectedEffectHandler : MonoBehaviour
{
    [SerializeField]
    Material appearEffectMaterial;

    [SerializeField]
    Material objectMaterial;

    [SerializeField]
    private CompanionStateView companionStateView;

    private int _counter = 0;

    private void Start()
    {

    }

    private void Awake()
    {
        if (companionStateView != null)
        {
            companionStateView.PulseEnvironmentTriggered += HandleObjectDetected;
        }
    }

    private void OnDestroy()
    {
        if (companionStateView != null)
        {
            companionStateView.PulseEnvironmentTriggered -= HandleObjectDetected;
        }
    }

    private void HandleObjectDetected()
    {
        if (_counter >= 1) return;

        _counter++;
        ChangeMaterial(appearEffectMaterial);
        StartCoroutine(SetObjectMaterialNormal());
    }

    IEnumerator SetObjectMaterialNormal()
    {
        yield return new WaitForSeconds(4f);
        ChangeMaterial(objectMaterial);
    }

    private void ChangeMaterial(Material materialToChangeTo)
    {
        gameObject.GetComponent<Renderer>().material = materialToChangeTo;
    }
}
