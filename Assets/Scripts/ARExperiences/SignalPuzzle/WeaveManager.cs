using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaveManager : MonoBehaviour
{
    [SerializeField] GameObject signalSequenceGameObject;
    [SerializeField] List<GameObject> threads;

    private void OnEnable()
    {
        SignalSceneManager.onSequenceGenerated += PopulateThreads;
    }

    // TODO: refactor should be called on event from signalscenemanager, this hsould be private -- need persistent mngr too
    public void PopulateThreads()
    {
        if (signalSequenceGameObject != null)
        {
            SignalSceneManager signalManagerScript = signalSequenceGameObject.GetComponent<SignalSceneManager>();
            int[] threadInts = signalManagerScript.GetSignalSequenceAsArray();
            for (int i = 0; i < threadInts.Length; i++)
            {
                GameObject correctThread = signalManagerScript.threadObjects[threadInts[i]];
                threads.Add(correctThread);
            }
        }

        InstantiateThreadObjects();
    }

    private void Start()
    {

    }

    private void InstantiateThreadObjects()
    {
        var threadArray = threads.ToArray();
        for (int i = 0; i < threadArray.Length; i++)
            Instantiate(threadArray[i], new Vector3((i * 0.5f) - 0.5f, 4.0f, -2.0f), Quaternion.identity);
            // Instantiate(threadArray[i], new Vector3(UnityEngine.Random.Range(-2.0f, 2.0f), -0.35f, UnityEngine.Random.Range(1.0f, 4.0f)), Quaternion.identity);
    }
}
