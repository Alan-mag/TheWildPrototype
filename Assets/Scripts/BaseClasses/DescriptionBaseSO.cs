using UnityEngine;


// 'SerializableScriptableObject was used here
public class DescriptionBaseSO : SerializableScriptableObject
{
    [TextArea] public string description;
}