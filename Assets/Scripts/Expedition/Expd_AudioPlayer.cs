using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Expd_AudioPlayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText = null;

    [SerializeField] private ExpeditionSO expeditionData;

    private void Start()
    {
        if (expeditionData != null)
        {
            messageText.text = expeditionData.AudioLogMessage;
        }
    }
}
