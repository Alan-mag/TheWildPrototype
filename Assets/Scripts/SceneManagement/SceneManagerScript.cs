using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagerScript : MonoBehaviour
{
    [SerializeField] Button btn1;
    [SerializeField] Button btn2;
    [SerializeField] Button btn3;
    [SerializeField] Button btn4;

    // Start is called before the first frame update
    void Start()
    {
        btn1 = GameObject.Find("SignalBtn").GetComponent<Button>();
        btn1.onClick.AddListener(() => LoadScene("Signal_Scene_Test"));

        btn2 = GameObject.Find("Puzzle1Btn").GetComponent<Button>();
        btn2.onClick.AddListener(() => LoadScene("Puzzle_Creation_1"));

        btn3 = GameObject.Find("AudioLogBtn").GetComponent<Button>();
        btn3.onClick.AddListener(() => LoadScene("AudioLog_Test_1"));

        btn4 = GameObject.Find("ScannerBtn").GetComponent<Button>();
        btn4.onClick.AddListener(() => LoadScene("Scanner_Test_1"));
    }

    private void LoadScene(string sceneName)
    {
        SceneManager.UnloadScene(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }
}
