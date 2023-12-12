using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NavigationArrowManager : MonoBehaviour
{
    [SerializeField] GameObject navigationArrow;
    [SerializeField] Transform playerTransform;

    private void Start()
    {
        StartCoroutine(DetectMapObject());
    }

    IEnumerator DetectMapObject()
    {
        yield return new WaitForSeconds(2);
        GameObject[] expeditionMapObjects = GameObject.FindGameObjectsWithTag("ExpeditionMapObject");
        if (expeditionMapObjects != null)
        {
            foreach (GameObject mapObject in expeditionMapObjects)
            {
                RenderNavigationArrow(mapObject.transform);
            }
        }
    }

    private void RenderNavigationArrow(Transform mapObjectTransform)
    {
        Vector3 playerPos = playerTransform.position;
        GameObject arrow = Instantiate(navigationArrow, new Vector3(playerPos.x, playerPos.y, playerPos.z), playerTransform.rotation);
        // arrow.transform.LookAt(mapObjectTransform);
        arrow.gameObject.transform.parent = this.gameObject.transform;
        arrow.GetComponent<NavigationArrow>().target = mapObjectTransform;
    }
}
