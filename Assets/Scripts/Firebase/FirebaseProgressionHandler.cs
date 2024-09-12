using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
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

    [Header("Player stats")]
    [SerializeField] float adventureExperience;
    [SerializeField] float creatorExperience;
    [SerializeField] float explorerExperience;

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

    // Start is called before the first frame update
    void Start()
    {
        if (userId == null)
        {
            throw new UnassignedReferenceException("User Id in FirebaseManager is null on Start.");
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

        adventureExperience = playerStats.Adventurer;
        creatorExperience = playerStats.Creator;
        explorerExperience = playerStats.Explorer;

        // handle this in way better // TODO: FIX
        UpdateSliderValue();
    }

    private void UpdateSliderValue()
    {
        // first icon showing
        if (adventureExperience > 0.0f) {
            UpdateIconAlphaToOne(adventureToolOneIcon.GetComponent<Image>());
        }
        if (creatorExperience > 0.0f) {
            UpdateIconAlphaToOne(creatorToolOneIcon.GetComponent<Image>());
        }
        if (explorerExperience > 0.0f) {
            UpdateIconAlphaToOne(explorerToolOneIcon.GetComponent<Image>());
        }


        if (adventureExperience >= maxSliderValue)
        {
            adventureSlideOne.GetComponent<Slider>().value = maxSliderValue;
            UpdateIconAlphaToOne(adventureToolTwoIcon.GetComponent<Image>());
        }
        else if (adventureExperience > 0 && adventureExperience < maxSliderValue)
        {
            adventureSlideOne.GetComponent<Slider>().value = adventureExperience % maxSliderValue;
        }

        if (creatorExperience >= maxSliderValue)
        {
            creatorSlideOne.GetComponent<Slider>().value = maxSliderValue;
            UpdateIconAlphaToOne(creatorToolTwoIcon.GetComponent<Image>());
        }
        else if (creatorExperience > 0 && creatorExperience < maxSliderValue)
        {
            creatorSlideOne.GetComponent<Slider>().value = creatorExperience % maxSliderValue;
        }

        if (explorerExperience >= maxSliderValue)
        {
            explorerSlideOne.GetComponent<Slider>().value = maxSliderValue;
            UpdateIconAlphaToOne(explorerToolTwoIcon.GetComponent<Image>());
        }
        else if (explorerExperience > 0 && explorerExperience < maxSliderValue)
        {
            explorerSlideOne.GetComponent<Slider>().value = explorerExperience % maxSliderValue;
        }

        // for second sliders
        if (adventureExperience > maxSliderValue && adventureExperience < maxSliderValue * 2)
        {
            adventureSlideTwo.GetComponent<Slider>().value = (adventureExperience - maxSliderValue) % maxSliderValue;
            UpdateIconAlphaToOne(adventureToolTwoIcon.GetComponent<Image>());
        }
        else if (adventureExperience >= maxSliderValue * 2)
        {
            adventureSlideTwo.GetComponent<Slider>().value = maxSliderValue;
            UpdateIconAlphaToOne(adventureToolThreeIcon.GetComponent<Image>());
        }

        if (creatorExperience > maxSliderValue && creatorExperience < maxSliderValue * 2)
        {
            creatorSlideTwo.GetComponent<Slider>().value = (creatorExperience - maxSliderValue) % maxSliderValue;
            UpdateIconAlphaToOne(creatorToolTwoIcon.GetComponent<Image>());
        }
        else if (creatorExperience >= maxSliderValue * 2)
        {
            creatorSlideTwo.GetComponent<Slider>().value = maxSliderValue;
            UpdateIconAlphaToOne(creatorToolThreeIcon.GetComponent<Image>());
        }

        if (explorerExperience > maxSliderValue && explorerExperience < maxSliderValue * 2)
        {
            explorerSlideTwo.GetComponent<Slider>().value = (explorerExperience - maxSliderValue) % maxSliderValue;
            UpdateIconAlphaToOne(explorerToolTwoIcon.GetComponent<Image>());
        }
        else if (explorerExperience >= maxSliderValue * 2)
        {
            explorerSlideTwo.GetComponent<Slider>().value = maxSliderValue;
            UpdateIconAlphaToOne(explorerToolThreeIcon.GetComponent<Image>());
        }

        // third level don't exist?
        if (adventureExperience > maxSliderValue * 2 && adventureExperience < maxSliderValue * 3)
        {
            adventureSlideThree.GetComponent<Slider>().value = (adventureExperience - maxSliderValue * 2) % maxSliderValue;
            UpdateIconAlphaToOne(adventureToolThreeIcon.GetComponent<Image>());
        }
        else if (adventureExperience >= maxSliderValue * 3)
        {
            adventureSlideThree.GetComponent<Slider>().value = maxSliderValue;
            UpdateIconAlphaToOne(adventureToolThreeIcon.GetComponent<Image>());
        }

        // level 4 btn
        if (adventureExperience >= maxSliderValue * 3)
        {
            UpdateIconAlphaToOne(adventureToolFourIcon.GetComponent<Image>());
        }
    }

    private void UpdateIconAlphaToOne(Image iconImage)
    {
        var tempColor = iconImage.color;
        tempColor.a = 1f;
        iconImage.color = tempColor;
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
}
