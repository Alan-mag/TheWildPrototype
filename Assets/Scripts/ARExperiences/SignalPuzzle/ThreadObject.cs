using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreadObject : MonoBehaviour
{
    [SerializeField] int threadNumber;

    [Header("Signal Scene Manager Required in Scene")]
    private SignalSceneManager signalSceneManagerObj;

    private void Awake()
    {
        if (signalSceneManagerObj == null)
            signalSceneManagerObj = FindObjectOfType<SignalSceneManager>();
    }

    private void Start()
    {
        if (GameObject.Find("Companion") != null)
        {
            this.gameObject.transform.LookAt(GameObject.Find("Companion").transform);
            gameObject.transform.rotation = Quaternion.identity; // object will face companion, but rotate up and down won't happen
        }
    }

    public void CollectThread()
    {
        var mngrComponent = signalSceneManagerObj.GetComponent<SignalSceneManager>();
        mngrComponent.CollectThreadObject(threadNumber);
        Destroy(this.gameObject);
    }
}
