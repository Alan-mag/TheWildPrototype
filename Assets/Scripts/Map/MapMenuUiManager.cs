using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MapMenuUiManager : MonoBehaviour
{
    [SerializeField] TMP_Text usernameDisplay;

    void Start()
    {
        if (PlayerPrefs.GetString("username") != null)
            usernameDisplay.text = PlayerPrefs.GetString("username");

    }
}
