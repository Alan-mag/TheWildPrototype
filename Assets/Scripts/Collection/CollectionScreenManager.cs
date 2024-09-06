using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectionScreenManager : MonoBehaviour
{
    public enum CollectionPuzzleType
    {
        Signal,
        Sphere,
        AudioLog
    }

    [SerializeField] private GameObject collectionUiListItem;
    [SerializeField] private FirebaseManager firebaseManager;
    [SerializeField] private Canvas screenCanvas;

    [SerializeField]
    private CollectionPuzzleType collectionPuzzleType = CollectionPuzzleType.Signal;

    [Header("Chosen puzzle Scriptable Objects")]
    [SerializeField] ChosenSignalExperienceSO chosenSignalExperienceSO;

    private void Start()
    {
        // List<string> playerList = firebaseManager.GetPlayerList();
        // firebaseManager.GetPlayerList(DisplayPlayerList);

        // todo: create switch case given what type is chosen
        // call different firebase method depending
        switch(collectionPuzzleType)
        {
            case CollectionPuzzleType.Signal:
                // Todo: generic firebase method for grabbing player puzzle
                // pass a string value for specific puzzle type
                // return and call DisplayListToScreen as callback
                firebaseManager.GetPlayerSignals(HandleSignalsReturn);
                break;
                
            case CollectionPuzzleType.Sphere:
                break;
                
            case CollectionPuzzleType.AudioLog:
                break;
        }
    }

    /*private void DisplayListToScreen(List<string> itemNames)
    {
        Debug.Log("DisplayPlayerList " + itemNames.ToArray().ToString());
        Vector2 canvasPos = screenCanvas.GetComponent<RectTransform>().anchoredPosition;

        for (int i = 0; i < itemNames.Count; i++)
        {
            GameObject collectionItemObject = Instantiate(collectionUiListItem);
            collectionItemObject.transform.SetParent(screenCanvas.transform);
            collectionItemObject.GetComponent<RectTransform>().anchoredPosition = canvasPos;
            collectionItemObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(
                collectionItemObject.GetComponent<RectTransform>().anchoredPosition.x,
                i * 160 + 1600 // todo: actually figure out this spacing
            );
            collectionItemObject.GetComponentInChildren<TMP_Text>().text = itemNames.ToArray()[i];
        }

    }*/

    private void HandleSignalsReturn(List<SignalData> signalDataList)
    {
        // todo:
        // first, test and see if ui displays object for each item
        // each object should have signal data assoc w/it
        // when selected, it should then select that as chosenSignalSO
        // then change scene
        Vector2 canvasPos = screenCanvas.GetComponent<RectTransform>().anchoredPosition;

        for (int i = 0; i < signalDataList.Count; i++)
        {
            GameObject collectionItemObject = Instantiate(collectionUiListItem);
            collectionItemObject.transform.SetParent(screenCanvas.transform);
            collectionItemObject.GetComponent<RectTransform>().anchoredPosition = canvasPos;
            collectionItemObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(
                (collectionItemObject.GetComponent<RectTransform>().anchoredPosition.x + i * 250) - 400,
                collectionItemObject.GetComponent<RectTransform>().anchoredPosition.y + 700 // todo: update after first row
            );
            // collectionItemObject.GetComponentInChildren<TMP_Text>().text = i.ToString();
        }
    }
}
