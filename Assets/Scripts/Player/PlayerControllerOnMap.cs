using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerOnMap : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("OnCollisionEnter called");
        GameObject otherGameObject = collision.gameObject;
        if (otherGameObject.name.Contains("TestScene"))
        {
            otherGameObject.GetComponent<SceneChangeHandler>().RotateInteractiveObject();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        GameObject otherGameObject = collision.gameObject;
        if (otherGameObject.name.Contains("TestScene"))
        {
            otherGameObject.GetComponent<SceneChangeHandler>().DisableRotationInteractiveObject();
        }
    }
}
