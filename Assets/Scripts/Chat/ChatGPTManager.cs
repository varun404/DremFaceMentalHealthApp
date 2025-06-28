using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;

# region Message Body
[Serializable]
public class ChatMessage
{
    public string role;
    public string content = "";
}

[Serializable]
public class ChatRequest
{
    public string model = "gpt-4";
    public float temperature = 0.5f;
    public ChatMessage[] messages;
}
#endregion


public class ChatGPTManager : MonoBehaviour
{

    static Queue<ChatRequest> chatRequests = new Queue<ChatRequest>();

    // Might move to server side in production.    
    static string OpenAIKey = Secrets.OpenAIKey;

     
    public static string mentalHealthAssistant { get; private set; } = "a supportive, non-judgemental companion. " +
        "You listen first, respond with empathy, validation and gentle encouragement. " +
        "You are **not** a licensed therapist and you never claim to diagnose.If a user expresses self-harm intent, encourage them to reach out to a professional or crisis line (e.g. 988 in the US). " +
        "Keep answers under 30 words.";
    public static string mentalHealthActivityAssistant { get; private set; } = "a supportive, non-judgemental companion. " +
        "You listen first, respond with empathy, validation and gentle encouragement. " +
        "You are **not** a licensed therapist and you never claim to diagnose.If a user expresses self-harm intent, encourage them to reach out to a professional or crisis line (e.g. 988 in the US). " +
        "Keep answers under 30 words.";

    public static string universalNavigationAssistant { get; private set; } = "You are a command interpreter. " +
        "Convert user voice input into one of the following app commands: 'open_chat', 'go_home', 'start_activity', 'send_message:<message>', or 'none'. " +
        "Return ONLY the command.";

    private void Update()
    {
        while (chatRequests.Count > 0)
        {
            StartCoroutine(AskChatGPT(chatRequests.Dequeue()));
        }
    }


    public static void QueueMentalthHealtRequest(string messageText)
    {
                            
        // Build payload
        var payload = new ChatRequest
        {
            model = "gpt-4",
            temperature = 0.7f,
            messages = new ChatMessage[] {
                new ChatMessage {
                    role = "system",
                    content = mentalHealthAssistant
                },
                new ChatMessage {
                    role = "user",
                    content = messageText

                }
            }
        };

        chatRequests.Enqueue(payload);        
    }


    public static void QueueMentalHealthActivityRequest(string messageText)
    {

        // Build payload
        var payload = new ChatRequest
        {
            model = "gpt-4",
            temperature = 0.7f,
            messages = new ChatMessage[] {
                new ChatMessage {
                    role = "system",
                    content = mentalHealthActivityAssistant
                },
                new ChatMessage {
                    role = "user",
                    content = messageText

                }
            }
        };

        chatRequests.Enqueue(payload);
    }


    public static void QueueUniversalNavigationRequest(string messageText)
    {

        // Build payload
        var payload = new ChatRequest
        {
            model = "gpt-4",
            temperature = 0.7f,
            messages = new ChatMessage[] {
                new ChatMessage {
                    role = "system",
                    content = universalNavigationAssistant
                },
                new ChatMessage {
                    role = "user",
                    content = messageText

                }
            }
        };

        chatRequests.Enqueue(payload);
    }

    IEnumerator AskChatGPT(ChatRequest payload)
        {
            const string url = "https://api.openai.com/v1/chat/completions";        

            // Serialize and wrap as UTF-8 JSON
            byte[] bodyRaw = Encoding.UTF8.GetBytes(JsonUtility.ToJson(payload));
        
            // Create UWR
            using var uwr = new UnityWebRequest(url, "POST")
            {
                uploadHandler = new UploadHandlerRaw(bodyRaw),
                downloadHandler = new DownloadHandlerBuffer(),
                timeout = 15
            };
        
            // Set auth and content type
            uwr.SetRequestHeader("Content-Type", "application/json");

            // Set Auth
            uwr.SetRequestHeader("Authorization", $"Bearer {OpenAIKey}");

            // Make request
            yield return uwr.SendWebRequest();

            // Failed
            if (uwr.result != UnityWebRequest.Result.Success)
                Debug.LogError($"LLM error: {uwr.error}");
            // Success
            else
            {

                // Parse AI Response
                string result = uwr.downloadHandler.text;
                JObject jsonRoot = JObject.Parse(result);

                string reply = (string)jsonRoot["choices"]?[0]?["message"]?["content"];

                Debug.Log(reply);
            
                AIMessage aiResponse = new AIMessage(reply);

                // Get audio response
                //TTSManager.ttsManagerInstance.GetAudioResponse(reply);
                
                ProcessAIResponse(aiResponse);
                

                // Analytics
                AnalyticsManager.RecordMessageSent(1);
                aiResponse.CalculateWordCount();
                AnalyticsManager.RecordWordsCount(aiResponse.GetWordCount());
            }

        yield break;
    }

    void ProcessAIResponse(Message aiResponse)
    {
        switch (AppManager.appManagerInstance.currentAppState)
        {
            case AppManager.AppState.Activity:
                ActivityUIHandler.activityUIManagerInstance.SetAIAssessmentResponse(aiResponse.GetMessageText());
                break;

            case AppManager.AppState.Chat:
                ChatManager.chatManagerInstance.AddNewMessage(aiResponse);
                break;

            case AppManager.AppState.Profile:
            case AppManager.AppState.None:
            case AppManager.AppState.Home:
            default:
                break;
        }
    }    
}


