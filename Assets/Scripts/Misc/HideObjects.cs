using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideObjects : MonoBehaviour
{
    [SerializeField]
    GameObject[] objectsToReveal;

    [SerializeField]
    private int secondsToWait = 3;

    private void Start()
    {
        StartCoroutine(RevealObjects());
    }

    private IEnumerator RevealObjects()
    {
        yield return new WaitForSeconds(secondsToWait);
        foreach (var obj in objectsToReveal)
        {
            obj.SetActive(true);
        }
    }
}