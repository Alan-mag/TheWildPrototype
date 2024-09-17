using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatorToolsUi : MonoBehaviour
{
    [SerializeField] GameObject signalCreatorButton;
    [SerializeField] GameObject sphereCreatorButton;
    [SerializeField] GameObject audioLogCreatorButton;

    [Header("Progression")]
    [SerializeField] GameProgressionSO gameProgressionSO;

    private void Start()
    {
        // default: signalCreator should always to available after tutorial

        if (gameProgressionSO.PlayerCreatorExperience >= gameProgressionSO.creatorSecondTier)
        {
            sphereCreatorButton.SetActive(true);
        }
        if (gameProgressionSO.PlayerCreatorExperience >= gameProgressionSO.creatorThirdTier)
        {
            audioLogCreatorButton.SetActive(true);
        }
    }
}
