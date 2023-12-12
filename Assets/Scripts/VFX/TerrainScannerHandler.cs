using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainScannerHandler : MonoBehaviour
{
    public GameObject terrainScannerPrefab;
    public float duration = 10;
    public float size = 500;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            SpawnTerrainScanner();
        }
    }

    private void SpawnTerrainScanner()
    {
        GameObject terrainScanner = Instantiate(terrainScannerPrefab, gameObject.transform.position, Quaternion.identity) as GameObject;
        ParticleSystem terrainScannerPS = terrainScanner.transform.GetChild(0).GetComponent<ParticleSystem>();

        if (terrainScannerPS != null )
        {
            var main = terrainScannerPS.main;
            main.startLifetime = duration;
            main.startSize = size;
        }
        else
        {
            Debug.Log("The first child doesn't have a particle system.");
        }
        Destroy(terrainScannerPrefab, duration + 1);
    }
}
