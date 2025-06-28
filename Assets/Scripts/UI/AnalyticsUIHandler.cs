using System;
using UnityEngine;

public class AnalyticsUIHandler : MonoBehaviour
{
     [SerializeField] 
    TMPro.TMP_Text txtTotalMessages;
    
    [SerializeField] 
    TMPro.TMP_Text txtTotalWords;
    
    [SerializeField]
    TMPro.TMP_Text txtAppLaunches;
    
    [SerializeField]
    TMPro.TMP_Text txtUsageTime;
    
    [SerializeField]
    TMPro.TMP_Text txtLastSession;

    // Update is called once per frame
    void Update()
    {
        UpdateAnalyticsUI();
    }

    public void UpdateAnalyticsUI()
    {
        txtTotalMessages.text = $"Messages Sent: {AnalyticsManager.totalMessagesSent : 0}";
        txtTotalWords.text = $"Words Typed  : {AnalyticsManager.totalWordCount : 0}";
        txtAppLaunches.text = $"App Launches : {AnalyticsManager.totalAppLaunches : 0}";

        
        TimeSpan timeSpan = TimeSpan.FromSeconds(AnalyticsManager.totalUsageTime);
        txtUsageTime.text = $"Usage Time   : {timeSpan:hh\\:mm\\:ss}";

        // Show “–” if never closed before
        txtLastSession.text = AnalyticsManager.lastSessionDate == DateTime.MinValue
            ? "Last Session : –"
            : $"Last Session : {AnalyticsManager.lastSessionDate:yyyy-MM-dd HH:mm}";
    }
}
