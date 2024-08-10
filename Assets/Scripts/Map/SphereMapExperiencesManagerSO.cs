using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Map/Sphere Map Experiences Manager")]
public class SphereMapExperiencesManagerSO : ScriptableObject
{
    [SerializeField] public List<PuzzleSphereInformation> sphereInformationCollection;
}
