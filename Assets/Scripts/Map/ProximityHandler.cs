using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityHandler : MonoBehaviour
{
    [SerializeField] float radiusOfDetectionSphere = 100;
    [SerializeField] int proximityObjectLimitForSpawn = 2;
    [SerializeField] int totalCurrentProximityObjects;

    // Map game objects
    [SerializeField] List<GameObject> dynamicMapObjects;
    [SerializeField] GameObject pool3dPuzzleObject;
    [SerializeField] GameObject poolSignalObject;

    void Awake()
    {
        gameObject.GetComponent<SphereCollider>().radius = radiusOfDetectionSphere;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetTotalObjectsInSphere());
        if (dynamicMapObjects.Count == 0)
        {
            Debug.Log("Need object in dynamic map objects list to spawn.");
        }
    }

    // write a method to detect how many objects are inside sphere
    IEnumerator GetTotalObjectsInSphere()
    {
        while (true) // continue while script is active
        {
            totalCurrentProximityObjects = 0; // reset total
            Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.GetComponent<SphereCollider>().bounds.center, radiusOfDetectionSphere); // todo: fix
            foreach (var hitCollider in hitColliders)
            {
                // Debug.Log("hitCollider tag: " + hitCollider.tag);
                if (hitCollider.tag == "MapSpawnObject")
                {
                    totalCurrentProximityObjects++;
                }
            }
            yield return new WaitForSeconds(5);
            SpawnMapExperience();
        }
    }

    // write method to spawn a prefab at a location inside sphere
    void SpawnMapExperience()
    {

        Vector3 playerPos = transform.parent.position;
        Vector3 playerDirection = transform.parent.forward;
        Quaternion playerRotation = transform.parent.rotation;
        float spawnDistance = Random.Range(15, 60);

        if (totalCurrentProximityObjects < proximityObjectLimitForSpawn) // make this variable?? Also make position variable?
        {
            Vector3 spawnPos = playerPos + playerDirection * spawnDistance;

            Instantiate(
                SelectMapObjectToSpawn(),
                spawnPos, 
                Quaternion.identity
            );
        }
    }

    GameObject SelectMapObjectToSpawn()
    {
        int pos = Random.Range(0, dynamicMapObjects.Count);
        return dynamicMapObjects[pos];
    }

    // todo: add functionality to pull down player-created puzzles from db
}
