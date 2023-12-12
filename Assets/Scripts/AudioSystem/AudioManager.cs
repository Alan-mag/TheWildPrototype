using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// todo:
// flipping playSignalLoop off and back on should start audio sequence again
public class AudioManager : MonoBehaviour
{
    [SerializeField] GameObject signalSequenceGameObject;
    [SerializeField] List<AudioClip> audioClips;
    [SerializeField] AudioClip[] audioClipArray;

    [SerializeField] AudioClip[] audioClipReferences;

    AudioSource audioSource;

    // public bool playSignalLoop = true;
    public int audioLoopRepeat = 1;

    private void Awake()
    {
        if (!audioSource)
            audioSource = this.GetComponent<AudioSource>();

        if (signalSequenceGameObject == null)
            signalSequenceGameObject = GameObject.Find("SignalSceneManager");
    }

    private void OnEnable()
    {
        SignalSceneManager.onSequenceGenerated += PopulateAudioClips;
    }

    // TODO: refactor should be called on event from signalscenemanager, this hsould be private -- need persistent mngr too
    public void PopulateAudioClips()
    {
        if (signalSequenceGameObject != null)
        {
            SignalSceneManager signalManagerScript = signalSequenceGameObject.GetComponent<SignalSceneManager>();
            int[] threadInts = signalManagerScript.GetSignalSequenceAsArray();
            for (int i = 0; i < threadInts.Length; i++)
            {
                AudioClip correctClip = signalManagerScript.audioClips[threadInts[i]];
                audioClips.Add(correctClip);
                Array.Resize(ref audioClipArray, audioClipArray.Length + 1); 
                audioClipArray[i] = correctClip; // populate array at same time - kinda messy
            }
        }

        StartCoroutine(PlayAudioSequentially());
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator PlayAudioSequentially()
    {
        while (audioLoopRepeat > 0)
        {
            yield return new WaitForSeconds(1);

            for (int i = 0; i < audioClipArray.Length; i++)
            {
                audioSource.clip = audioClipArray[i];
                audioSource.Play();
                while (audioSource.isPlaying)
                {
                    yield return null;
                }
            }
            audioLoopRepeat--;
        }
    }

    public void PlaySound(int soundVal)
    {
        switch (soundVal)
        {
            case 0:
                audioSource.clip = audioClipReferences[0];
                audioSource.Play();
                break;
            case 1:
                audioSource.clip = audioClipReferences[1];
                audioSource.Play();
                break;
            case 2:
                audioSource.clip = audioClipReferences[2];
                audioSource.Play();
                break;
            default:
                break;

        }
    }
}
