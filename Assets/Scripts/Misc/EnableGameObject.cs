using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetGameObjectActive : MonoBehaviour
{
    [SerializeField] float timeToVisible;
    [SerializeField] GameObject objectToSetActive;

    void Start()
    {
        StartCoroutine(SetObjectActive(timeToVisible));
        // StartCoroutine(FadeButton(completeButton, true, 5f));
    }

    private IEnumerator SetObjectActive(float timeToVisible)
    {
        yield return new WaitForSeconds(timeToVisible);
        objectToSetActive.SetActive(true);
    }

    /*IEnumerator FadeButton(Button button, bool fadeIn, float duration)
    {
        float counter = 0f;

        float a, b;
        if (fadeIn)
        {
            a = 0f;
            b = 1f;
        }
        else 
        {
            a = 1f;
            b = 0f;
        }

        Image buttonImage = button.GetComponent<Image>();
        TextMeshPro buttonText = button.GetComponentInChildren<TextMeshPro>();

        Debug.Log(buttonText);

        if (!button.enabled)
        {
            button.enabled = true; 
        }
        if (!buttonImage.enabled)
        {
            buttonImage.enabled = true;
        }
        if (!buttonText.enabled)
        {
            buttonText.enabled = true;
        }

        Color buttonColor = buttonImage.color;
        Color textColor = buttonText.color;

        ColorBlock colorBlock = button.colors;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(a, b, counter / duration);


            if (button.transition == Selectable.Transition.None || button.transition == Selectable.Transition.ColorTint)
            {
                buttonImage.color = new Color(buttonColor.r, buttonColor.g, buttonColor.b, alpha);//Fade Traget Image
                buttonText.color = new Color(textColor.r, textColor.g, textColor.b, alpha);//Fade Text
            }
            else if (button.transition == Selectable.Transition.SpriteSwap)
            {
                ////Fade All Transition Images
                colorBlock.normalColor = new Color(colorBlock.normalColor.r, colorBlock.normalColor.g, colorBlock.normalColor.b, alpha);
                colorBlock.pressedColor = new Color(colorBlock.pressedColor.r, colorBlock.pressedColor.g, colorBlock.pressedColor.b, alpha);
                colorBlock.highlightedColor = new Color(colorBlock.highlightedColor.r, colorBlock.highlightedColor.g, colorBlock.highlightedColor.b, alpha);
                colorBlock.disabledColor = new Color(colorBlock.disabledColor.r, colorBlock.disabledColor.g, colorBlock.disabledColor.b, alpha);

                button.colors = colorBlock; //Assign the colors back to the Button
                buttonImage.color = new Color(buttonColor.r, buttonColor.g, buttonColor.b, alpha);//Fade Traget Image
                buttonText.color = new Color(textColor.r, textColor.g, textColor.b, alpha);//Fade Text
            }
            else
            {
                Debug.LogError("Button Transition Type not Supported");
            }

            yield return null;
        }
    }*/
}
