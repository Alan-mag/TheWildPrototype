using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Map : MonoBehaviour
{
    [SerializeField] TMP_Text usernameDisplay;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetString("user_id") != null)
            usernameDisplay.text = PlayerPrefs.GetString("user_id");

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
