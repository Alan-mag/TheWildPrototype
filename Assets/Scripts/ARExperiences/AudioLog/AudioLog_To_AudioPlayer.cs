using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLog_To_AudioPlayer : MonoBehaviour
{
    [SerializeField] GameObject sceneChangeObject;

    public void ToAudioPlayer()
    {
        sceneChangeObject.GetComponent<SceneChangeHandler>().ChangeScene();
    }
}
