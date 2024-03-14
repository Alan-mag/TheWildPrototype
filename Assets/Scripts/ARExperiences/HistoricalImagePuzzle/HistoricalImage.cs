using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HistoricalImage : MonoBehaviour
{
    [SerializeField] GameObject imageSource;
    [SerializeField] TextMeshProUGUI historicalImageTitleText = null;
    [SerializeField] Sprite[] historicalImageSprites;

    private void Start()
    {
        if (HistoricalImageInfo.ImageTitle != null)
        {
            historicalImageTitleText.text = HistoricalImageInfo.ImageTitle;
            foreach (Sprite sprite in historicalImageSprites)
            {
                if (sprite.name == HistoricalImageInfo.ImageTitle)
                {
                    imageSource.GetComponent<SpriteRenderer>().sprite = sprite;
                    imageSource.GetComponent<SpriteRenderer>().size = new Vector2(7.5f, 5f);
                }
            }
        }
    }
}
