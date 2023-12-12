using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.UI;

public class ImageFade : MonoBehaviour
{
    [SerializeField] RawImage img;
    [SerializeField] float delay;

    public bool isLoading = true;

    private void Start()
    {
        StartFade();
    }

    public void StartFade()
    {
        StartCoroutine(FadeImage(true, delay));
    }

    IEnumerator FadeImage(bool fadeAway, float delay)
    {
        while (isLoading)
        {
            yield return new WaitForSeconds(delay);
            if (fadeAway)
            {
                for (float i = 1; i > 0; i -= Time.deltaTime)
                {
                    // img.color = new Color(30, 184, 233, i);
                    var tempColor = img.color;
                    tempColor.a = i;
                    img.color = tempColor;
                    yield return null;
                }
            }
            else
            {
                for (float i = 0; i <= 1; i += Time.deltaTime)
                {
                    // img.color = new Color(30, 184, 233, i);
                    var tempColor = img.color;
                    tempColor.a = i;
                    img.color = tempColor;
                    yield return null;
                }
            }
        }
    }
}
