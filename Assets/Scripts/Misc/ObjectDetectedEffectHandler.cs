using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectDetectedEffectHandler : MonoBehaviour
{
    [SerializeField]
    Material appearEffectMaterial;

    [SerializeField]
    Material[] objectMaterialArray;

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
        ApplyDetectedEffect(appearEffectMaterial);
        StartCoroutine(SetObjectMaterialNormal());
    }

    IEnumerator SetObjectMaterialNormal()
    {
        yield return new WaitForSeconds(4f);
        ApplyNewGameObjectMaterial(objectMaterialArray);
    }

    private void ApplyDetectedEffect(Material appearEffectMaterial)
    {
        /*Renderer renderer = gameObject.GetComponent<Renderer>();
        renderer.material = appearEffectMaterial;*/

        Renderer renderer = gameObject.GetComponent<Renderer>();
        int numOfObjectMaterials = objectMaterialArray.Length;
        Material[] effectArray = Enumerable.Repeat(appearEffectMaterial, numOfObjectMaterials).ToArray();
        renderer.materials = effectArray;
    }

    private void ApplyNewGameObjectMaterial(Material[] materialToChangeTo)
    {
        Renderer renderer = gameObject.GetComponent<Renderer>();
        renderer.materials = objectMaterialArray;
    }
}
