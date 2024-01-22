using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// SO Event channel 
// fire event when experience completed
// 

public class MapObjectManager : MonoBehaviour
{
    [SerializeField]
    private double id;

    public double Id
    {
        get { return id; }
        set { id = value; }
    }
}
