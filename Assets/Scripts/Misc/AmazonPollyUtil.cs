using Amazon.Polly.Model;
using Amazon.Polly;
using Amazon.Runtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using Amazon;
using System.Threading.Tasks;

public class AmazonPollyUtil : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    public async void PlayMessageWithPolly(string message)
    {
        var cred = new BasicAWSCredentials("AKIAQ344IYWB5HZ5WZFB", "oByvU5MsKk+uzcI1GXd1y7k3gyG6pAyiVVZh4rtM"); // TODO: update --> not to be used after test
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
    }

    private void WriteSpeechToFile(Stream stream)
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
    }
}
