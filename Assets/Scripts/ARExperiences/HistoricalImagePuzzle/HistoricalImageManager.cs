using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HistoricalImageManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _infoPanelText = null;

    private void Awake()
    {
        _infoPanelText.text = HistoricalImageInfo.ImageTitle;
    }
}
