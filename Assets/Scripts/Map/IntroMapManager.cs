using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroMapManager : MonoBehaviour
{
    [SerializeField] private GameObject introMapSpawner;

    void Start()
    {
        Vector3 spawnLocation = new Vector3(20.0f, 10.0f, 40.0f);
        GameObject tutorialSpawnObj = Instantiate(introMapSpawner, spawnLocation, Quaternion.identity);
    }
}
