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

    [SerializeField] public static double explorerExperienceVerySmall = 0.25f;
    [SerializeField] public static double explorerExperienceSmall = 1f;
    [SerializeField] public static double explorerExperienceMedium = 5f;
    [SerializeField] public static double explorerExperienceLarge = 10f;

    [Header("Progression")]
    [SerializeField] private double explorerFirstTier = 5f;
    [SerializeField] private double explorerSecondTier = 25f;
    [SerializeField] private double explorerThirdTier = 100f;

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

    [SerializeField] public static double adventurerExperienceVerySmall = 0.25f;
    [SerializeField] public static double adventurerExperienceSmall = 1f;
    [SerializeField] public static double adventurerExperienceMedium = 5f;
    [SerializeField] public static double adventurerExperienceLarge = 10f;

    [Header("Progression")]
    [SerializeField] private static double adventurerFirstTier = 5f;
    [SerializeField] private static double adventurerSecondTier = 25f;
    [SerializeField] private static double adventurerThirdTier = 100f;

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

    [SerializeField] private double creatorExperienceVerySmall = 0.25f;
    [SerializeField] private double creatorExperienceSmall = 1f;
    [SerializeField] private double creatorExperienceMedium = 5f;
    [SerializeField] private double creatorExperienceLarge = 10f;

    [Header("Progression")]
    [SerializeField] private static double creatorFirstTier = 5f;
    [SerializeField] private static double creatorSecondTier = 25f;
    [SerializeField] private static double creatorThirdTier = 100f;
    [SerializeField] private static double creatorFourthTier = 150f;
}
