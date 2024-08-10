using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Map/Signal Map Experience Manager")]
public class SignalMapExperiencesManagerSO : ScriptableObject
{
    [SerializeField] public List<SignalData> signalCollection;

    /*public SignalData SignalCollection {
        get { return signalCollection.; }
        set { SignalCollection = value; }
    }*/
}
