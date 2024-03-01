using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Characters/Companion Data")]
public class CompanionDataSO : ScriptableObject
{
    [Header("Appearance information")]
    [SerializeField]
    private Material orbMaterial;
    [SerializeField]
    private Material largeWingMaterial;
    [SerializeField]
    private Material smallWingMaterial;
    public Material OrbMaterial
    {
        get { return orbMaterial; }
        set { orbMaterial = value; }
    }
    public Material LargeWingMaterial
    {
        get { return largeWingMaterial; }
        set { largeWingMaterial = value; }
    }

    public Material SmallWingMaterial
    {
        get { return smallWingMaterial; }
        set { smallWingMaterial = value; }
    }
}
