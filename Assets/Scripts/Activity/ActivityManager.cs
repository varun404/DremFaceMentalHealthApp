using UnityEngine;

public class ActivityManager : MonoBehaviour
{
    public static ActivityManager activityManagerInstance;

    private void Awake()
    {
        if (activityManagerInstance == null)
            activityManagerInstance = this;
        else
            Destroy(this);
    }


    public void SubmitRequest(string messageText)
    {
        UserMessage userMessage = new UserMessage(messageText);
        
        ChatGPTManager.QueueMentalHealthActivityRequest(messageText);


        // Aanlytics
        AnalyticsManager.RecordMessageSent(1);
        userMessage.CalculateWordCount();
        AnalyticsManager.RecordWordsCount(userMessage.GetWordCount());
    }


    
}
