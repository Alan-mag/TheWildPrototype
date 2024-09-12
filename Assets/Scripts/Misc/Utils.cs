using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using JetBrains.Annotations;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Utils
{
    public static string UniqueKeyGenerator(int characters)
    {
        string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        int len = characters;

        System.Random rnd = new System.Random();
        StringBuilder b = new StringBuilder(len);
        for (int i = 0; i < len; i++)
        {
            b.Append(chars[rnd.Next(chars.Length)]);
        }
        return b.ToString();
    }
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
    [SerializeField]
    public Nullable<float> x;
    [SerializeField]
    public Nullable<float> y;
    [SerializeField]
    public Nullable<float> z;

    public PuzzleSphereTarget()
    {
        this.x = null;
        this.y = null;
        this.z = null;
    }
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
public class JSONArray<T>
{
    public T[] array;
}

[Serializable]
public class PuzzleSphereInformation
{
    [SerializeField]
    public string creatorName { get; set; }
    [SerializeField]
    public List<PuzzleSphereTarget> puzzleSphereTarget { get; set; }

    public PuzzleSphereInformation()
    {
        this.creatorName = null;
        this.puzzleSphereTarget = new List<PuzzleSphereTarget>();
    }
    public PuzzleSphereInformation(string creatorName, List<PuzzleSphereTarget> puzzleSphereTarget)
    {
        this.creatorName = creatorName;
        this.puzzleSphereTarget = puzzleSphereTarget;
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
    public string fmodEventReference;
    public int group;

    public AudioLogData(double lat, double lng, string msg, string fmodE, int grp = 0)
    {
        this.latitude = lat;
        this.longitude = lng;
        this.message = msg;
        this.fmodEventReference = fmodE;
        this.group = grp;
    }

    public AudioLogData(string lat, string lng, string msg, string fmodE, string grp = "0")
    {
        this.latitude = Convert.ToDouble(lat);
        this.longitude = Convert.ToDouble(lng);
        this.message = msg;
        this.fmodEventReference = fmodE;
        this.group = Convert.ToInt32(grp);
    }

    public string ToJson()
    {
        return JsonUtility.ToJson(this); // todo: doesn't seem to be working?
    }

}

[Serializable]
public class PlayerAudioLogData
{
    public double latitude;
    public double longitude;
    public string filename;
    public string username = null;

    public PlayerAudioLogData(string lat, string lng, string filename, string username = null)
    {
        this.latitude = Convert.ToDouble(lat);
        this.longitude = Convert.ToDouble(lng);
        this.filename = filename;
        this.username = username;
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
    public string creatorName;

    public SignalData()
    {
        this.sequence = new List<int>();
        this.creatorName = null;
    }

    public SignalData(List<int> sequence, string creatorName = null)
    {
        this.sequence = sequence;
        this.creatorName = creatorName;
    }

    public string ToJson()
    {
        return JsonConvert.SerializeObject(this);
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

[Serializable]
public class HistoricalImageLocationData
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

    private string imageSourceTitle;
    public string ImageSourceTitle
    {
        get { return imageSourceTitle;  }
        set { imageSourceTitle = value; }
    }

    private string imageTitle;
    public string ImageTitle
    {
        get { return imageTitle; }
        set { imageTitle = value; }
    }

    public HistoricalImageLocationData(double latitude, double longitude, string imageTitle, string imageSourceTitle)
    {
        this.latitude = latitude;
        this.longitude = longitude;
        this.imageSourceTitle = imageSourceTitle;
        this.imageTitle = imageTitle;
    }
}