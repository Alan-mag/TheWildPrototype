using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Progression/GameProgressionSO")]
public class GameProgressionSO : ScriptableObject
{
    [Header("Explorer")]
    [Space(10)]
    [SerializeField] private double _playerExplorerExperience;
    public double PlayerExplorerExperience
    {
        get { return _playerExplorerExperience; }
        set { _playerExplorerExperience = value; }
    }
    [Space(10)]

    [SerializeField] public double explorerExperienceVerySmall = 0.25f;
    [SerializeField] public double explorerExperienceSmall = 1f;
    [SerializeField] public double explorerExperienceMedium = 5f;
    [SerializeField] public double explorerExperienceLarge = 10f;

    [Header("Progression")]
    [SerializeField] public double explorerFirstTier = 5f;
    [SerializeField] public double explorerSecondTier = 25f;
    [SerializeField] public double explorerThirdTier = 100f;

    [Space(30)]

    [Header("Adventurer")]
    [Space(10)]
    [SerializeField] private double _playerAdventurerExperience;
    public double PlayerAdventurerExperience
    {
        get { return _playerAdventurerExperience; }
        set { _playerAdventurerExperience = value; }
    }
    [Space(10)]

    [SerializeField] public double adventurerExperienceVerySmall = 0.25f;
    [SerializeField] public double adventurerExperienceSmall = 1f;
    [SerializeField] public double adventurerExperienceMedium = 5f;
    [SerializeField] public double adventurerExperienceLarge = 10f;

    [Header("Progression")]
    [SerializeField] public double adventurerFirstTier = 5f;
    [SerializeField] public double adventurerSecondTier = 25f;
    [SerializeField] public double adventurerThirdTier = 100f;
    [SerializeField] public double adventurerFourthTier = 150f;

    [Space(30)]

    [Header("Creator")]
    [Space(10)]
    [SerializeField] private double _playerCreatorExperience;
    public double PlayerCreatorExperience
    {
        get { return _playerCreatorExperience;  }
        set { _playerCreatorExperience = value; }
    }
    [Space(10)]

    [SerializeField] public double creatorExperienceVerySmall = 0.25f;
    [SerializeField] public double creatorExperienceSmall = 1f;
    [SerializeField] public double creatorExperienceMedium = 5f;
    [SerializeField] public double creatorExperienceLarge = 10f;

    [Header("Progression")]
    [SerializeField] public double creatorFirstTier = 5f;
    [SerializeField] public double creatorSecondTier = 25f;
    [SerializeField] public double creatorThirdTier = 100f;
}
