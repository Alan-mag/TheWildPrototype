using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Signal/Chosen Signal Experience")]
public class ChosenSignalExperienceSO : ScriptableObject
{
    [SerializeField] public SignalData chosenSignal;
}
