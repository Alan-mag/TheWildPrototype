using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Map : MonoBehaviour
{
    // Game Progression SO
    [Header("Character Progression")]
    [SerializeField] private GameProgressionSO gameProgressionSO;
    [SerializeField] GameObject scannerIcon;

    void Start()
    {
        if (gameProgressionSO.PlayerAdventurerExperience >= gameProgressionSO.adventurerSecondTier)
        {
            scannerIcon.SetActive(true);
        }

    }

}
