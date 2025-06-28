using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;

#region TTS Request Body
[Serializable]
public class TTSInput
{
    public string text;
}

[Serializable]
public class TTSVoice
{
    public string languageCode = "en-US";
    public string name = "en-US-Wavenet-D";
}

[Serializable]
public class TTSAudioConfig
{
    public string audioEncoding = "MP3";
}

[Serializable]
public class TTSRequest
{
    public TTSInput input;
    public TTSVoice voice;
    public TTSAudioConfig audioConfig;
}
#endregion


public class TTSManager : MonoBehaviour
{
    // NOTE: Secrets.cs should be in .gitignore — replace with real key when testing
    static string GoogleAPIKey = Secrets.GoogleAPIKey;

    static string endpoint = "https://texttospeech.googleapis.com/v1/text:synthesize";

    AudioSource audioSource;

    public static TTSManager ttsManagerInstance;
    void Awake()
    {

        if (ttsManagerInstance == null)
            ttsManagerInstance = this;
        else
            Destroy(this);


        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void GetAudioResponse(string messageText)
    {
        StartCoroutine(ttsManagerInstance.SendTTSRequest(messageText));        
    }
    

    IEnumerator SendTTSRequest(string text)
    {
        var payload = new TTSRequest
        {
            input = new TTSInput { text = text },
            voice = new TTSVoice(),
            audioConfig = new TTSAudioConfig()
        };

        string jsonBody = JsonUtility.ToJson(payload);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);

        using var uwr = new UnityWebRequest($"{endpoint}?key={GoogleAPIKey}", "POST")
        {
            uploadHandler = new UploadHandlerRaw(bodyRaw),
            downloadHandler = new DownloadHandlerBuffer(),
            timeout = 15
        };

        uwr.SetRequestHeader("Content-Type", "application/json");

        yield return uwr.SendWebRequest();

        if (uwr.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"TTS error: {uwr.error}");
            yield break;
        }

        string response = uwr.downloadHandler.text;

        JObject jsonRoot = JObject.Parse(response);
        string base64Audio = (string)jsonRoot["audioContent"];

        if (string.IsNullOrEmpty(base64Audio))
        {
            Debug.LogWarning("TTS returned empty audio content.");
            yield break;
        }

        byte[] audioBytes = Convert.FromBase64String(base64Audio);
        PlayMP3(audioBytes);

        yield break;
    }

    void PlayMP3(byte[] audioData)
    {        
        var audioClip = WavUtility.ToAudioClip(audioData);        
        audioSource.clip = audioClip;
        audioSource.Play();                
    }
}
