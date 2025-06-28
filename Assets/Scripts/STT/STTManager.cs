using System.Collections;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.Networking;

public class STTManager : MonoBehaviour, ISpeechToTextListener
{
    //[SerializeField]
    //TMPro.TMP_Text outputText;

    public static STTManager sTTManagerInstance;    

    private void Awake()
    {
        if (sTTManagerInstance == null)        
            sTTManagerInstance = this;       
        else
            Destroy(this);
    }


    public void StartSpeechToText()
    {
        SpeechToText.RequestPermissionAsync((permission) =>
        {
            if (permission == SpeechToText.Permission.Granted)
            {
                if (SpeechToText.Start(this, preferOfflineRecognition: false))
                {
                    Debug.Log("Started");
                    //outputText.text += "\n Started";
                }
                else
                {
                    Debug.Log("Couldn't start speech recognition session!");
                    //outputText.text += "\n Couldn't start speech recognition session!";
                }
            }
            else
            {
                Debug.LogError("Permission is denied!");
                //outputText.text += "\n Permission is denied!";
            }
        });
    }

    
    //void SaveAsWav()
    //{
    //    string filePath = Path.Combine(Application.persistentDataPath, "recording.wav");
    //    byte[] wavData = WavUtility.FromAudioClip(generatedClip); // Use a helper like WavUtility
    //    File.WriteAllBytes(filePath, wavData);

    //    //StartCoroutine(SendToWhisper(filePath));
    //}

    public void OnReadyForSpeech()
    {
        Debug.Log("Ready for Speech");
        //outputText.text += "\n Ready!";

    }

    public void OnBeginningOfSpeech()
    {
        Debug.Log("Recording....");
        //outputText.text += "\n Recording...";
    }

    public void OnVoiceLevelChanged(float normalizedVoiceLevel)
    {
        // Note that On Android, voice detection starts with a beep sound and it can trigger this callback. You may want to ignore this callback for ~0.5s on Android.
        
    }

    public void OnPartialResultReceived(string spokenText)
    {
        Debug.Log("OnPartialResultReceived: " + spokenText);
        //outputText.text += "\n" + spokenText;
    }

    public void OnResultReceived(string spokenText, int? errorCode)
    {
        Debug.Log("OnResultReceived: " + spokenText + (errorCode.HasValue ? (" --- Error: " + errorCode) : ""));
        //outputText.text += "\n" + spokenText + (errorCode.HasValue ? (" --- Error: " + errorCode) : "");
        
        // Recommended approach:
        // - If errorCode is 0, session was aborted via SpeechToText.Cancel. Handle the case appropriately.
        // - If errorCode is 9, notify the user that they must grant Microphone permission to the Google app and call SpeechToText.OpenGoogleAppSettings.
        // - If the speech session took shorter than 1 seconds (should be an error) or a null/empty spokenText is returned, prompt the user to try again (note that if
        //   errorCode is 6, then the user hasn't spoken and the session has timed out as expected).

        switch (errorCode)
        {
            case 0:
                break;

            case 6:
            case 9:
                StartSpeechToText();
                break;

            default:
                ForwardResponseToUI(spokenText);
                break;
        }
    }

    void ForwardResponseToUI(string response)
    {
        switch (AppManager.appManagerInstance.currentAppState)
        {
            case AppManager.AppState.Activity:
                ActivityUIHandler.activityUIManagerInstance.SetUserResponse(response);
                break;
            
            case AppManager.AppState.Chat:
                ChatWindowUIHandler.chatWindowUIHandlerInstance.SetUserResponse(response);
                break;
            
            case AppManager.AppState.Profile:            
            case AppManager.AppState.None:                
            case AppManager.AppState.Home:
            default:
                break;
        }
    }



}


