using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Sphere/Chosen Sphere Experience")]
public class ChosenSphereExperienceSO : ScriptableObject
{
    [SerializeField] public PuzzleSphereInformation chosenSpherePuzzle;
}
