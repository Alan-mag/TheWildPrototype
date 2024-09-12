using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerAudioLogMapObject : MonoBehaviour
{
    public string latitude;
    public string longitude;
    public string username;
    public string filename;

    [SerializeField] TMP_Text usernameText;

    public void HandleSetup()
    {
        usernameText.text = username;
    }
}