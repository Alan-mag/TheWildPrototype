using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.UI;


// TODO:
// pull player experience
// populate sliders with values form db
// do this for each [adv, explor, creat]
// fill center of each button that needs it
// add btn to go back to map
public class FirebaseProgressionHandler : MonoBehaviour
{
    [SerializeField] string userId;
    [SerializeField] DatabaseReference userRootDbReference;
    [SerializeField] DatabaseReference userStatsDbReference;

    // Game Progression SO
    [SerializeField] private GameProgressionSO gameProgressionSO;

    // slider ui
    [SerializeField] float maxSliderValue = 10f;

    [SerializeField] GameObject adventureSlideOne;
    [SerializeField] GameObject adventureSlideTwo;
    [SerializeField] GameObject adventureSlideThree;

    [SerializeField] GameObject creatorSlideOne;
    [SerializeField] GameObject creatorSlideTwo;
    
    [SerializeField] GameObject explorerSlideOne;
    [SerializeField] GameObject explorerSlideTwo;

    // icons
    [SerializeField] GameObject adventureToolOneIcon;
    [SerializeField] GameObject adventureToolTwoIcon;
    [SerializeField] GameObject adventureToolThreeIcon;
    [SerializeField] GameObject adventureToolFourIcon;

    [SerializeField] GameObject creatorToolOneIcon;
    [SerializeField] GameObject creatorToolTwoIcon;
    [SerializeField] GameObject creatorToolThreeIcon;

    [SerializeField] GameObject explorerToolOneIcon;
    [SerializeField] GameObject explorerToolTwoIcon;
    [SerializeField] GameObject explorerToolThreeIcon;

    private void Awake()
    {
        // todo: this should all pull down at beginning for player stats
        if (PlayerPrefs.GetString("user_id") != null)
        {
            userId = PlayerPrefs.GetString("user_id");
            userRootDbReference = FirebaseDatabase.DefaultInstance.GetReference($"players/{userId}/");
            userStatsDbReference = FirebaseDatabase.DefaultInstance.GetReference($"players/{userId}/stats/");
        }
    }

    void Start()
    {
        if (userId == null)
        {
            throw new UnassignedReferenceException("User Id in FirebaseProgressionHandler is null on Start.");
        }
        GetPlayerProgressionStats();
    }

    private void GetPlayerProgressionStats()
    {
        GetPlayerStats(PopulateProgressionUi);
    }

    private void PopulateProgressionUi(string statSnapshot)
    {
        PlayerStatistics playerStats = JsonUtility.FromJson<PlayerStatistics>(statSnapshot);
        Debug.Log($"Player stats:\nAdventure Stats: ${playerStats.Adventurer}\nCreator Stats: ${playerStats.Creator}\nExplorer Stats: ${playerStats.Explorer}");
        gameProgressionSO.PlayerExplorerExperience = playerStats.Explorer;
        gameProgressionSO.PlayerAdventurerExperience = playerStats.Adventurer;
        gameProgressionSO.PlayerCreatorExperience = playerStats.Creator;
        UpdateSliderMinMaxValues();
        UpdateSlidersExplorer();
        UpdateSlidersCreator();
        UpdateSlidersAdventurer();
    }

    // update slider min max value
    private void UpdateSliderMinMaxValues()
    {
        // Adventurer slider values
        // min assumed to be 0
        adventureSlideOne.GetComponent<Slider>().maxValue = (float)gameProgressionSO.adventurerSecondTier;

        adventureSlideTwo.GetComponent<Slider>().minValue = (float)gameProgressionSO.adventurerSecondTier;
        adventureSlideTwo.GetComponent<Slider>().maxValue = (float)gameProgressionSO.adventurerThirdTier;

        adventureSlideThree.GetComponent<Slider>().minValue = (float)gameProgressionSO.adventurerThirdTier;
        adventureSlideThree.GetComponent<Slider>().maxValue = (float)gameProgressionSO.adventurerFourthTier;

        // Explorer slider values
        // min assumed to be 0
        explorerSlideOne.GetComponent<Slider>().maxValue = (float)gameProgressionSO.explorerSecondTier;

        explorerSlideTwo.GetComponent<Slider>().minValue = (float)gameProgressionSO.explorerSecondTier;
        explorerSlideTwo.GetComponent<Slider>().maxValue = (float)gameProgressionSO.explorerThirdTier;

        // Creator slider values
        // min assumed to be 0
        creatorSlideOne.GetComponent<Slider>().maxValue = (float)gameProgressionSO.creatorSecondTier;

        creatorSlideTwo.GetComponent<Slider>().minValue = (float)gameProgressionSO.creatorSecondTier;
        creatorSlideTwo.GetComponent<Slider>().maxValue = (float)gameProgressionSO.creatorThirdTier;
    }

    private void UpdateSlidersExplorer()
    {
        var explorerExp = gameProgressionSO.PlayerExplorerExperience;
        if (explorerExp >= gameProgressionSO.explorerFirstTier)
        {
            UpdateIconAlphaToOne(explorerToolOneIcon.GetComponentInChildren<Image>());
            explorerSlideOne.GetComponent<Slider>().value = (float)gameProgressionSO.PlayerExplorerExperience;
        }
        if (explorerExp >= gameProgressionSO.explorerSecondTier)
        {
            UpdateIconAlphaToOne(explorerToolTwoIcon.GetComponentInChildren<Image>());
            explorerSlideTwo.GetComponent<Slider>().value = (float)gameProgressionSO.PlayerExplorerExperience;
        }
        if (explorerExp >= gameProgressionSO.explorerThirdTier)
        {
            UpdateIconAlphaToOne(explorerToolThreeIcon.GetComponentInChildren<Image>());
        }
    }

    private void UpdateSlidersCreator()
    {
        var creatorExp = gameProgressionSO.PlayerCreatorExperience;
        if (creatorExp >= gameProgressionSO.creatorFirstTier)
        {
            UpdateIconAlphaToOne(creatorToolOneIcon.GetComponentInChildren<Image>());
            creatorSlideOne.GetComponent<Slider>().value = (float)gameProgressionSO.PlayerCreatorExperience;
        }
        if (creatorExp >= gameProgressionSO.creatorSecondTier)
        {
            UpdateIconAlphaToOne(creatorToolTwoIcon.GetComponentInChildren<Image>());
            creatorSlideTwo.GetComponent<Slider>().value = (float)gameProgressionSO.PlayerCreatorExperience;
        }
        if (creatorExp >= gameProgressionSO.creatorThirdTier)
        {
            UpdateIconAlphaToOne(creatorToolThreeIcon.GetComponentInChildren<Image>());
        }
    }

    private void UpdateSlidersAdventurer()
    {
        var adventurerExp = gameProgressionSO.PlayerAdventurerExperience;
        if (adventurerExp >= gameProgressionSO.creatorFirstTier)
        {
            UpdateIconAlphaToOne(adventureToolOneIcon.GetComponentInChildren<Image>());
            adventureSlideOne.GetComponent<Slider>().value = (float)gameProgressionSO.PlayerAdventurerExperience;
        }
        if (adventurerExp >= gameProgressionSO.creatorSecondTier)
        {
            UpdateIconAlphaToOne(adventureToolTwoIcon.GetComponentInChildren<Image>());
            adventureSlideTwo.GetComponent<Slider>().value = (float)gameProgressionSO.PlayerAdventurerExperience;
        }
        if (adventurerExp >= gameProgressionSO.adventurerThirdTier)
        {
            UpdateIconAlphaToOne(adventureToolThreeIcon.GetComponentInChildren<Image>());
            adventureSlideThree.GetComponent<Slider>().value = (float)gameProgressionSO.PlayerAdventurerExperience;
        }
        if (adventurerExp >= gameProgressionSO.adventurerThirdTier)
        {
            UpdateIconAlphaToOne(adventureToolFourIcon.GetComponentInChildren<Image>());
        }
    }


    private void GetPlayerStats(Action<string> callback)
    {
       FirebaseDatabase.DefaultInstance.GetReference($"players/{userId}/stats/")
            .GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("Error getting player stats from db");
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    Debug.Log(snapshot);
                    callback(snapshot.GetRawJsonValue());
                }
            });
    }

    private void UpdateIconAlphaToOne(Image iconImage)
    {
        var tempColor = iconImage.color;
        tempColor.a = 1f;
        iconImage.color = tempColor;
    }
}
