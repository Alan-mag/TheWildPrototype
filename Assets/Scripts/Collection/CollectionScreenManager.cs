using System;
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
    [SerializeField] ChosenSphereExperienceSO chosenSphereExperienceSO;
    [SerializeField] ChosenAudioLogExperienceSO chosenAudioLogExperienceSO;

    private void Start()
    {
        switch(collectionPuzzleType)
        {
            case CollectionPuzzleType.Signal:
                firebaseManager.GetPlayerSignals(HandleSignalsReturn);
                break;
                
            case CollectionPuzzleType.Sphere:
                firebaseManager.GetPlayerSpheres(HandleSpherePuzzlesReturn);
                break;
                
            case CollectionPuzzleType.AudioLog:
                firebaseManager.GetPlayerAudioLogs(HandlePlayerAudioLogsReturn);
                break;
        }
    }

    private void HandleSignalsReturn(List<SignalData> signalDataList)
    {
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
            collectionItemObject.GetComponentInChildren<SignalCollectionItem>().itemSignalData = signalDataList[i];
        }
    }

    private void HandleSpherePuzzlesReturn(List<PuzzleSphereInformation> sphereDataList)
    {
        Vector2 canvasPos = screenCanvas.GetComponent<RectTransform>().anchoredPosition;

        for (int i = 0; i < sphereDataList.Count; i++)
        {
            GameObject collectionItemObject = Instantiate(collectionUiListItem);
            collectionItemObject.transform.SetParent(screenCanvas.transform);
            collectionItemObject.GetComponent<RectTransform>().anchoredPosition = canvasPos;
            collectionItemObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(
                (collectionItemObject.GetComponent<RectTransform>().anchoredPosition.x + i * 250) - 400,
                collectionItemObject.GetComponent<RectTransform>().anchoredPosition.y + 700 // todo: update after first row
            );
            collectionItemObject.GetComponentInChildren<SphereCollectionItem>().itemPuzzleData = sphereDataList[i];
        }
    }

    private void HandlePlayerAudioLogsReturn(List<PlayerAudioLogData> audioLogDataList)
    {
        Vector2 canvasPos = screenCanvas.GetComponent<RectTransform>().anchoredPosition;

        for (int i = 0; i < audioLogDataList.Count; i++)
        {
            GameObject collectionItemObject = Instantiate(collectionUiListItem);
            collectionItemObject.transform.SetParent(screenCanvas.transform);
            collectionItemObject.GetComponent<RectTransform>().anchoredPosition = canvasPos;
            collectionItemObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(
                (collectionItemObject.GetComponent<RectTransform>().anchoredPosition.x + i * 250) - 400,
                collectionItemObject.GetComponent<RectTransform>().anchoredPosition.y + 700 // todo: update after first row
            );
            collectionItemObject.GetComponentInChildren<AudioLogCollectionItem>().itemAudioLogData = audioLogDataList[i];
        }
    }
}
