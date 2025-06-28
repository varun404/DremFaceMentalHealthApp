using System.Collections;
using UnityEngine;

public class ChatManager : MonoBehaviour
{
    public static ChatManager chatManagerInstance;

    private void Awake()
    {
        if (chatManagerInstance == null)
            chatManagerInstance = this;
        else
            Destroy(this);
                
    }

    public Conversation currentConversation { get; private set; } = new Conversation();


    //private void OnEnable()
    //{
    //    AppManager.appManagerInstance.OnAppStateChanged += ((AppManager.AppState appState) =>
    //    {
    //        if (appState == AppManager.AppState.Chat)
    //            StartCoroutine(InitializeChat());            
    //    });
    //}

    //private void OnDisable()
    //{
    //    AppManager.appManagerInstance.OnAppStateChanged -= ((AppManager.AppState appState) =>
    //    { });
    //}

    public void StartChat()
    {
        StartCoroutine(InitializeChat());
    }


    IEnumerator InitializeChat()
    {
        // Initialize STT
        SpeechToText.Initialize("en-US");

        while (!SpeechToText.IsServiceAvailable())
            yield return null;
        

        // Check if we have a history and if not then add an AI message
        if (currentConversation.GetConversationLength() == 0)
        {            
            ChatGPTManager.QueueMentalthHealtRequest("Hello!");
        }
        // Else scroll down
        else
            ChatWindowUIHandler.chatWindowUIHandlerInstance.ScrollToBottom();
        
        Debug.Log("Initialized Chat");

        yield break;
    }
 


    public void AddNewMessage(Message newMessage)
    {
        currentConversation.AddToConversation(newMessage);
        ChatWindowUIHandler.chatWindowUIHandlerInstance.AddMessageToUI(newMessage);
    }

    public void SubmitRequest(string messageText)
    {
        UserMessage userMessage = new UserMessage(messageText);
        AddNewMessage(userMessage);

        ChatGPTManager.QueueMentalthHealtRequest(messageText);


        // Aanlytics
        AnalyticsManager.RecordMessageSent(1);
        userMessage.CalculateWordCount();
        AnalyticsManager.RecordWordsCount(userMessage.GetWordCount());
    }
}
