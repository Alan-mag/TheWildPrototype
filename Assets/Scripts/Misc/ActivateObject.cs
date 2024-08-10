using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActivateObject : MonoBehaviour
{
    [SerializeField] float timeToVisible;
    [SerializeField] GameObject objectToSetActive;

    void Start()
    {
        StartCoroutine(EnableObject(timeToVisible));
        // StartCoroutine(FadeButton(completeButton, true, 5f));
    }

    private IEnumerator EnableObject(float timeToVisible)
    {
        yield return new WaitForSeconds(timeToVisible);
        objectToSetActive.SetActive(true);
    }
}
