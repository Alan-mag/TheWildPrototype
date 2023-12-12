using Niantic.Lightship.Maps.Coordinates;
using Niantic.Lightship.Maps.Unity.Core;
using Niantic.Platform.Debugging;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExpeditionManager : MonoBehaviour
{
    [SerializeField] ExpeditionSO expeditionData = default;

    // make expedition location object, create array of that -- for now has lat lng, maybe name later or something? idk
    private ExpeditionLocationData[] locationArray = new ExpeditionLocationData[] 
    {
        new ExpeditionLocationData(47.64911685880216, -122.34881377439586), // intro - plaza
        new ExpeditionLocationData(47.64832220549204, -122.34775536723672), // tree
        new ExpeditionLocationData(47.648420416023555, -122.3505378046487), // bridge
        new ExpeditionLocationData(47.65044935402641, -122.35477596465509), // dinos
        new ExpeditionLocationData(47.651284456009485, -122.35439674340027), // rocket
        new ExpeditionLocationData(47.6494718243353, -122.34948059350657), // interurban people
        new ExpeditionLocationData(47.64911685880216, -122.34881377439586) // back at plaza
        // new ExpeditionLocationData(47.64885666377061, -122.34796990881757) // final hq old - in building was inconsistent
    };

    [Header("Map Scene Objects")]
    [SerializeField] GameObject locationPreviewObject;

    [SerializeField] GameObject levelOneMapObject;
    [SerializeField] GameObject levelTwoMapObject;
    [SerializeField] GameObject levelThreeMapObject;
    [SerializeField] GameObject levelFourMapObject;
    [SerializeField] GameObject levelFiveMapObject;

    [SerializeField] LightshipMap lightshipMap;

    [SerializeField] private IntEventChannelSO _onCompletedStage = default;

    // actions
    // public static event Action OnCompleteExpedition;


    // look at quests in uop
    // todo will probably add:
    // event channels
    // some type of expedition manager or scriptable object

    // for now, expeditions will launch new map scene
    // we will then load in each experience at correct locations,
    // there will be some type of manager to handle linear parts
    // and open ended 'choose your own path' parts

    // events channels for handling data and events across scenes

    // stage one: 47.64911685880216, -122.34881377439586 [intro]
    // stage two: 47.64832220549204, -122.34775536723672 [tree]
    // stage 2.5: 47.647537223315595, -122.34783112331242 [statue?] // not now?
    // stage three: 47.648420416023555, -122.3505378046487 [bridge]

    // open stage 4: 47.65044935402641, -122.35477596465509 [dinos]
    // open stage 4: 47.651284456009485, -122.35439674340027 rocket
    // open stage 4: 47.6494718243353, -122.34948059350657 [interurban people]

    // final: hq rounghly? 47.64885666377061, -122.34796990881757

    private void OnEnable()
    {
        if (_onCompletedStage != null) {
            _onCompletedStage.OnEventRaised += HandleStageCompleted;
        }

        // OnCompleteExpedition += CompleteExpedition();
    }

    /*private Action CompleteExpedition()
    {
        SceneManager.LoadScene("MapTest");
        return OnCompleteExpedition;
    }*/

    private void OnDisable()
    {
        if (_onCompletedStage != null)
        {
            _onCompletedStage.OnEventRaised -= HandleStageCompleted;
        }

        // OnCompleteExpedition -= CompleteExpedition();
    }

    private void Start()
    {
        // todo for rendering path
        // RenderPathForExpedition();
        LoadExperienceLocations();

        /*if (expeditionData.CurrentLevel == expeditionData.TotalLevels)
        {
            OnCompleteExpedition.Invoke(); // dumb action delegate test
        }*/
    }

    private void LoadExperienceLocations()
    {
        switch(expeditionData.CurrentLevel)
        {
            case 0:
                LatLng latLng1 = new LatLng(locationArray[0].Latitude, locationArray[0].Longitude);
                Instantiate(levelOneMapObject, lightshipMap.LatLngToScene(in latLng1), Quaternion.identity);
                break;
            case 1:
                LatLng latLng2 = new LatLng(locationArray[1].Latitude, locationArray[1].Longitude);
                Instantiate(levelTwoMapObject, lightshipMap.LatLngToScene(in latLng2), Quaternion.identity);
                break;
            case 2:
                LatLng latLng3 = new LatLng(locationArray[2].Latitude, locationArray[2].Longitude);
                Instantiate(levelThreeMapObject, lightshipMap.LatLngToScene(in latLng3), Quaternion.identity);
                break;
            case 3:
                //
                LatLng latLng4_1 = new LatLng(locationArray[3].Latitude, locationArray[3].Longitude);
                GameObject path1 = Instantiate(levelFourMapObject, lightshipMap.LatLngToScene(in latLng4_1), Quaternion.identity);
                path1.GetComponent<PathSelectionHandler>().pathValue = 1;
                //
                LatLng latLng4_2 = new LatLng(locationArray[4].Latitude, locationArray[4].Longitude);
                GameObject path2 = Instantiate(levelFourMapObject, lightshipMap.LatLngToScene(in latLng4_2), Quaternion.identity);
                path2.GetComponent<PathSelectionHandler>().pathValue = 2;
                //
                LatLng latLng4_3 = new LatLng(locationArray[5].Latitude, locationArray[5].Longitude);
                GameObject path3 = Instantiate(levelFourMapObject, lightshipMap.LatLngToScene(in latLng4_3), Quaternion.identity);
                path3.GetComponent<PathSelectionHandler>().pathValue = 3;
                break;
            case 4:
                LatLng latLng = new LatLng(locationArray[6].Latitude, locationArray[6].Longitude);
                Instantiate(levelFiveMapObject, lightshipMap.LatLngToScene(in latLng), Quaternion.identity);
                break;
            case 5:
                break;
            default: 
                break;
        }
    }

    // todo: for rendering path later on
    // need to figure out 'open ended' parts
    // not necessary for prototype
    private void RenderPathForExpedition()
    {
        for (int i = 0; i < locationArray.Length; i++)
        {
            LatLng latLng = new LatLng(locationArray[i].Latitude, locationArray[i].Longitude);
            Instantiate(locationPreviewObject, lightshipMap.LatLngToScene(in latLng), Quaternion.identity);
        }
    }

    // todo: handle better with event system
    // will work with persistent scenes
    public void HandleStageCompleted(int value)
    {

    }
}
