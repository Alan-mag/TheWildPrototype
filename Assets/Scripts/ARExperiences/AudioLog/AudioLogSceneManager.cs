using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class AudioLogSceneManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _messageText = null;

    private void Awake()
    {
        _messageText.text = AudioLogInfo.Title;
    }
}
