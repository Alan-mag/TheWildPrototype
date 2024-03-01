using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder;

public class CompanionDataManager : MonoBehaviour
{
    [SerializeField] private CompanionDataSO companionDataSO;
    [SerializeField] private Renderer orbRenderer;
    [SerializeField] private Renderer largeWingRenderer;
    [SerializeField] private Renderer smallWingRenderer;

    private void Start()
    {
        Material[] matsOrb = orbRenderer.materials;
        matsOrb[0] = companionDataSO.OrbMaterial;
        orbRenderer.materials = matsOrb;

        Material[] matsLargeWing = largeWingRenderer.materials;
        matsLargeWing[0] = companionDataSO.LargeWingMaterial;
        largeWingRenderer.materials = matsLargeWing;

        Material[] matsSmallWing = smallWingRenderer.materials;
        matsSmallWing[0] = companionDataSO.SmallWingMaterial;
        smallWingRenderer.materials = matsSmallWing;
    }
}