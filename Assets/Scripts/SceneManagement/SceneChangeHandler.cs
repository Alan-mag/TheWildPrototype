using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeHandler : MonoBehaviour
{
    [SerializeField] string sceneName = "Signal_Scene_Test";

    public void ChangeScene()
    {
        SceneManager.LoadScene(sceneName);
        Debug.Log(gameObject.name);
        if (gameObject.name == "TestSceneAudio_map(Clone)")
        {
            AudioLogInfo.Message = gameObject.GetComponent<AudioLogMapHandler>().GetAudioLogMessage();
        }
        // todo:
        // check if going to audio log scene
        // if so, use static class to set current audio log text
        // to value from attached component to this object
        // something like:
        // <AudioLogMapScript>.setMessage() <-- then this sets in static class
        // fix this implementation with scriptable objects
    }

    public void RotateInteractiveObject()                                                                    
    {
        Debug.Log("RotateInteractiveObject called");
        GetComponentInChildren<Rotate>().enabled = true;
    }

    public void DisableRotationInteractiveObject()
    {
        Debug.Log("DisableRotationInteractiveObject called");
        GetComponentInChildren<Rotate>().enabled = false;
    }
}
