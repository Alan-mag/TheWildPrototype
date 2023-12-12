using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Initializer : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(LoadPersisentScene());
    }

    private IEnumerator LoadPersisentScene()
    {
        AsyncOperation load = SceneManager.LoadSceneAsync("PersistentScene", LoadSceneMode.Additive);
        yield return null;
    }
}
