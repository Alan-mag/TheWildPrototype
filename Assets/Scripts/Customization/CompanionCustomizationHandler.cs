using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionCustomizationHandler : MonoBehaviour
{
    [SerializeField] CompanionDataSO companionData;
    [SerializeField] Material[] orbMaterials;
    [SerializeField] Material[] largeWingMaterials;
    [SerializeField] Material[] smallWingMaterials;

    [SerializeField] private Renderer orbRenderer;
    [SerializeField] private Renderer largeWingRenderer;
    [SerializeField] private Renderer smallWingRenderer;

    private bool changeOrbMat;
    private bool changeLargeWingMat;
    private bool changeSmallWingMat;

    private int orbMatIndex = 0;       // all of these needs to be pulled from companionSO
    private int largeWingMatIndex = 0;
    private int smallWingMatIndex = 0;

    public void UpdateOrbMaterial(bool forward)
    {
        if (orbMatIndex >= orbMaterials.Length && forward)
        {
            orbMatIndex = 0;
        }

        if (orbMatIndex < 0 && !forward)
        {
            orbMatIndex = orbMaterials.Length - 1;
        }

        if (forward)
        {
            orbMatIndex++;
        }
        else
        {
            orbMatIndex--;
        }
        
        Material curMat = orbMaterials[orbMatIndex];
        companionData.OrbMaterial = curMat;
        ChangeMaterialOnRenderer(curMat, orbRenderer);
    }

    public void UpdateLargeWingMaterial(bool forward)
    {
        if (largeWingMatIndex >= largeWingMaterials.Length && forward)
        {
            largeWingMatIndex = 0;
        }

        if (largeWingMatIndex < 0 && !forward)
        {
            largeWingMatIndex = largeWingMaterials.Length - 1;
        }

        if (forward)
        {
            largeWingMatIndex++;
        }
        else
        {
            largeWingMatIndex--;
        }
        
        Material curMat = largeWingMaterials[largeWingMatIndex];
        companionData.LargeWingMaterial = curMat;
        ChangeMaterialOnRenderer(curMat, largeWingRenderer);
    }

    public void UpdateSmallWingMaterial(bool forward)
    {

        if (smallWingMatIndex >= smallWingMaterials.Length && forward)
        {
            smallWingMatIndex = 0;
        }

        if (smallWingMatIndex < 0 && !forward)
        {
            smallWingMatIndex = smallWingMaterials.Length - 1;
        }

        if (forward)
        {
            largeWingMatIndex++;
        }
        else
        {
            largeWingMatIndex--;
        }

        Material curMat = smallWingMaterials[smallWingMatIndex];
        companionData.SmallWingMaterial = curMat;
        ChangeMaterialOnRenderer(curMat, smallWingRenderer);
    }

    private void ChangeMaterialOnRenderer(Material mat, Renderer ren)
    {
        Material[] allRendererMats = ren.materials;
        allRendererMats[0] = mat;
        ren.materials = allRendererMats;
    }

}
