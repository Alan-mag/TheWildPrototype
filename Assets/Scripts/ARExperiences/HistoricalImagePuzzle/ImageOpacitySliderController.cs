using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ImageOpacitySliderController : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] SpriteRenderer historicalImage;

    private void Start()
    {
        slider.onValueChanged.AddListener(delegate { SliderValueChange(slider.value); }); // might be old legacy functionality
    }

    private void SliderValueChange(float value)
    {
        historicalImage.color = new Color(1f, 1f, 1f, value);
    }
}
