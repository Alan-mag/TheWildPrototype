using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SocialScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject friendListItem;
    [SerializeField] private FirebaseManager firebaseManager;
    [SerializeField] private Canvas screenCanvas;

    private void Start()
    {
        // List<string> playerList = firebaseManager.GetPlayerList();
        firebaseManager.GetPlayerList(DisplayPlayerList);
    }

    private void DisplayPlayerList(List<string> playerNames)
    {
        Debug.Log("DisplayPlayerList " + playerNames.ToArray().ToString());
        Vector2 canvasPos = screenCanvas.GetComponent<RectTransform>().anchoredPosition;

        for (int i = 0; i < playerNames.Count; i++) 
        {
            GameObject friendItemObject = Instantiate(friendListItem);
            friendItemObject.transform.SetParent(screenCanvas.transform);
            friendItemObject.GetComponent<RectTransform>().anchoredPosition = canvasPos;

            // todo: figure out position
            // RectTransform uitransform = friendItemObject.GetComponent<RectTransform>();
            friendItemObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(
                friendItemObject.GetComponent<RectTransform>().anchoredPosition.x,
                i * 160 + 1600 // todo: actually figure out this spacing
            );
            friendItemObject.GetComponentInChildren<TMP_Text>().text = playerNames.ToArray()[i];
        }

    }
}
