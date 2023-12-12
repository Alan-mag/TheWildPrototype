using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathSelectionHandler : MonoBehaviour
{
    public int pathValue;
    [SerializeField] ExpeditionSO expeditionData;

    public void SelectPath()
    {
        expeditionData.SetPathTaken(pathValue);
    }
}
