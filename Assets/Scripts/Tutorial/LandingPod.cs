using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingPod : MonoBehaviour
{
    [SerializeField] Animator landingPodAnimator;

    private void OnCollisionEnter(Collision collision)
    {
        // landingPodAnimator.SetTrigger("Open");
    }

    public void OpenPod()
    {
        landingPodAnimator.SetTrigger("Open");
    }
}
