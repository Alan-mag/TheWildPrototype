using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalCreationManager : MonoBehaviour
{
    [SerializeField] Transform focusObject;
    [SerializeField] GameObject signalOne;

    [SerializeField] List<GameObject> spawnedSignals;
    [SerializeField] List<int> signalSequenceValues;

    [Header("Audio")]
    [SerializeField] AudioClip[] audioClipReferences;

    AudioSource audioSource;

    public int audioLoopRepeat = 1;

    private void Awake()
    {
        if (!audioSource)
            audioSource = this.GetComponent<AudioSource>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SpawnSignal(int signalVal) // todo not using int yet
    {
        Vector3 posToSpawn = new Vector3(2.25f, 0.0f, 10f);

        if (spawnedSignals.Count >= 1)
        {
            foreach(var obj in spawnedSignals)
            {
                StartCoroutine(MoveFunction(obj));
            }
        }

        GameObject spawnedObj = Instantiate(signalOne, posToSpawn, Quaternion.identity);
        spawnedObj.transform.LookAt(focusObject.position);
        spawnedSignals.Add(spawnedObj);
        signalSequenceValues.Add(signalVal);

    }

    IEnumerator MoveFunction(GameObject signalObj)
    {
        var newPosition = signalObj.transform.position;
        newPosition.x = newPosition.x  - 1.5f;
        Debug.Log(newPosition);
        float timeSinceStarted = 0f;
        while (true)
        {
            timeSinceStarted += Time.deltaTime;
            signalObj.transform.position = Vector3.Lerp(signalObj.transform.position, newPosition, timeSinceStarted);
            signalObj.transform.LookAt(focusObject.position);
            if (signalObj.transform.position == newPosition)
            {
                yield break;
            }

            yield return null;
        }
    }

    public void Play()
    {
        StartCoroutine(PlayAudioSequentially());
    }

    public void Clear()
    {
        signalSequenceValues.Clear();
        foreach(GameObject spawnedSig in spawnedSignals)
        {
            Destroy(spawnedSig);
        }
        spawnedSignals.Clear();
    }

    public void SaveSignalSequenceToDatabase()
    {
        SignalData data = new SignalData(signalSequenceValues);

        FirebaseManager firebaseManager = GameObject.FindObjectOfType<FirebaseManager>();
        firebaseManager.AddSignalSequenceToDatabase(data.ToJson());
    }

    IEnumerator PlayAudioSequentially()
    {
        while (audioLoopRepeat > 0)
        {
            yield return new WaitForSeconds(1);

            for (int i = 0; i < signalSequenceValues.Count; i++)
            {
                Debug.Log(audioClipReferences[signalSequenceValues.ToArray()[i]]);
                audioSource.clip = audioClipReferences[signalSequenceValues.ToArray()[i]];
                audioSource.Play();
                while (audioSource.isPlaying)
                {
                    yield return null;
                }
            }
            audioLoopRepeat--;
        }
        audioLoopRepeat = 1; // reset for next play
    }
}
