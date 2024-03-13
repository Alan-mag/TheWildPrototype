using Niantic.Lightship.Maps.Coordinates;
using Niantic.Lightship.Maps.Unity.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HistoricalImageMapManager : MonoBehaviour
{
    [SerializeField] LightshipMap lightshipMap;
    [SerializeField] GameObject historicalImageMapObject;

    private HistoricalImageLocationData[] historicalImageArray = new HistoricalImageLocationData[]
    {
        new HistoricalImageLocationData(47.64879123884934, -122.34967348768225, "Fremont Bridge Top 1917"), // fremont bridge top - 1917
        new HistoricalImageLocationData(47.64827380926415, -122.35245472766556, "Ship 1917"), // fremont bridge other side - 1917
        new HistoricalImageLocationData(47.64962229579044, -122.3495089049282, "Interurban 2020"), // interurban during pandemic - 2020
        new HistoricalImageLocationData(47.651024827257935, -122.34746929780523, "Fremont Troll 1990"), // fremont troll during construction - 1990
        new HistoricalImageLocationData(47.65479417996826, -122.34853254900098, "School children 1900"), // schoole children - 1900
    };

    void Start()
    {
        CreateHistoricalImageLocationOnMap();
    }

    private void CreateHistoricalImageLocationOnMap()
    {
        foreach (var item in historicalImageArray)
        {
            LatLng latLng1 = new LatLng(item.Latitude, item.Longitude);
            Instantiate(historicalImageMapObject, lightshipMap.LatLngToScene(in latLng1), Quaternion.identity);
        }
    }
}
