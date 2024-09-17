using System.Collections;
using System.Collections.Generic;
using TMPro;
using Amazon;
using Amazon.Polly;
using Amazon.Runtime;
using UnityEngine;
using Amazon.Polly.Model;
using System.IO;
using UnityEngine.Networking;
using System.Threading.Tasks;

public class AudioLog : MonoBehaviour
{
    [SerializeField] GameObject logObject;
    [SerializeField] AudioSource audioSource;
    [SerializeField] GameObject AudioLogUI;
    [SerializeField] private GameObject tutorialManager;

    [SerializeField] private string accessKey;
    [SerializeField] private string secretKey;

    SceneChangeHandler sceneChangeHandler;

    [SerializeField]
    private TextMeshProUGUI _messageText = null;

    [Header("Progression")]
    [SerializeField] GameProgressionSO gameProgressionSO;

    private void Awake()
    {
        if (sceneChangeHandler == null)
            sceneChangeHandler = gameObject.GetComponent<SceneChangeHandler>();
    }

    private void Start()
    {

    }

    public void CollectAudioLog()
    {
        AudioLogUI.SetActive(true);
    }

    public void AddPlayerExpFromAudioLog()
    {
        // todo:
        // get AudioLogInfo fmod ref
        // handlefmodAudioEVent(sourceRef)

        FirebaseManager firebaseManager = GameObject.FindObjectOfType<FirebaseManager>();
        firebaseManager.UpdatePlayerExperience(EXPERIENCE_TYPE.Explorer, (float)gameProgressionSO.explorerExperienceMedium);
        firebaseManager.UpdatePlayerExperience(EXPERIENCE_TYPE.Adventurer, (float)gameProgressionSO.adventurerExperienceMedium);
    }

    public void StopAudioFromAudioLog()
    {
        audioSource.Stop();
    }

    public void FinishBtnSelect()
    {
        sceneChangeHandler.ChangeScene();
    }

    // AMAZON POLLY TEST
    /*private async void PlayMessageWithPolly(string message)
    {
        var cred = new BasicAWSCredentials(accessKey, secretKey); // TODO: update --> not to be used after test
        var client = new AmazonPollyClient(cred, RegionEndpoint.EUCentral1);

        var request = new SynthesizeSpeechRequest()
        {
            Text = message,
            Engine = Engine.Neural,
            VoiceId = VoiceId.Aria,
            OutputFormat = OutputFormat.Mp3
        };

        var response = await client.SynthesizeSpeechAsync(request);
        WriteSpeechToFile(response.AudioStream);

        using (var www = UnityWebRequestMultimedia.GetAudioClip($"file://{Application.persistentDataPath}/speech_audio.mp3", AudioType.MPEG))
        {
            var op = www.SendWebRequest();
            while (!op.isDone) await Task.Yield();

            var clip = DownloadHandlerAudioClip.GetContent(www);
            audioSource.clip = clip;
            audioSource.Play();
        }
    }*/

    /*private void WriteSpeechToFile(Stream stream)
    {
        using (var fileStream = new FileStream($"{Application.persistentDataPath}/speech_audio.mp3", FileMode.Create))
        {
            byte[] buffer = new byte[8 * 1024];
            int bytesRead;

            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                fileStream.Write(buffer, 0, bytesRead);
            }
        }
    }*/
}
