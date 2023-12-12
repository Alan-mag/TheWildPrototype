using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    
}

/// <summary>
/// Data Models
/// </summary>
public class PlayerStatistics 
{
    public float Adventurer;
    public float Creator;
    public float Explorer;
}

[Serializable]
public class PuzzleSphereTarget
{
    public float x;
    public float y;
    public float z;

    public PuzzleSphereTarget(float xParam, float yParam, float zParam)
    {
        this.x = xParam;
        this.y = yParam;
        this.z = zParam;
    }

    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }

}

[Serializable]
public class AudioLogData
{
    public double latitude;
    public double longitude;
    public string message;
    public int group;

    public AudioLogData(double lat, double lng, string msg, int grp = 0)
    {
        this.latitude = lat;
        this.longitude = lng;
        this.message = msg;
        this.group = grp;
    }

    public AudioLogData(string lat, string lng, string msg, string grp = "0")
    {
        this.latitude = Convert.ToDouble(lat);
        this.longitude = Convert.ToDouble(lng);
        this.message = msg;
        this.group = Convert.ToInt32(grp); ;
    }

    public string ToJson()
    {
        return JsonUtility.ToJson(this); // todo: doesn't seem to be working?
    }

}

[Serializable]
public class SignalData
{
    public List<int> sequence;

    public SignalData(List<int> seq)
    {
        this.sequence = seq;
    }

    public string ToJson()
    {
        return JsonConvert.SerializeObject(this.sequence); // only serializing sequence
    }
}

[Serializable]
public class ExpeditionLocationData
{
    private double latitude;
    public double Latitude
    {
        get { return latitude; } 
        set { latitude = value; }
    }

    private double longitude;
    public double Longitude
    {
        get { return longitude; }
        set { longitude = value; }
    }

    public ExpeditionLocationData(double latitude, double longitude)
    {
        this.latitude = latitude;
        this.longitude = longitude;
    } 
}