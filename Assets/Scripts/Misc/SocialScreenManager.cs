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
        Debug.Log("DisplayPlayerList " + playerNames.Count.ToString());
        for (int i = 0; i < playerNames.Count; i++) 
        {
            GameObject friendItemObject = Instantiate(friendListItem);
            friendItemObject.transform.SetParent(screenCanvas.transform);
            // todo: figure out position
            // RectTransform uitransform = friendItemObject.GetComponent<RectTransform>();
            friendItemObject.transform.position = new Vector3(
                friendItemObject.transform.position.x,
                i + 80,
                friendItemObject.transform.position.z
            );
            friendItemObject.GetComponentInChildren<TMP_Text>().text = playerNames.ToArray()[i];
        }
    }
}
