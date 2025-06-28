using System;
using System.Collections;
using UnityEngine;

public class AppManager : MonoBehaviour
{
    float sessionStartTime;

    public static AppManager appManagerInstance;
    private void Awake()
    {
        if (appManagerInstance == null)
            appManagerInstance = this;
        else
            Destroy(this);

        AnalyticsManager.Load();

        AnalyticsManager.RecordAppLaunches(1);

        sessionStartTime = Time.realtimeSinceStartup;
    }

    

    public enum AppState
    {
        None,
        Home,
        Activity,
        Chat,
        Profile
    }
    public AppState currentAppState
    {
        get;
        private set;
    }


    

    //public event Action<AppState> OnAppStateChanged;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetAppState(AppState.Home);        
    }

    public void SetAppState(AppState newAppState)
    {
        if (newAppState == currentAppState)
            return;

        currentAppState = newAppState;

        //OnAppStateChanged?.Invoke(currentAppState);

        UIManager.uIManagerInstance.SetUIState(currentAppState);

        if(currentAppState == AppState.Chat)
            ChatManager.chatManagerInstance.StartChat();
    }

    void OnApplicationPause(bool paused)
    {
        if (paused)
            FlushUsage();
        else
            sessionStartTime = Time.realtimeSinceStartup;
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            FlushUsage();
            AnalyticsManager.Save();
        }
        else
            sessionStartTime = Time.realtimeSinceStartup;
    }


    void OnApplicationQuit()
    {
        FlushUsage();
        AnalyticsManager.Save();
    }

    void FlushUsage()
    {
        float usageTimeInSeconds = Time.realtimeSinceStartup - sessionStartTime;
        
        if (usageTimeInSeconds > 0f)
            AnalyticsManager.RecordUsageTime(usageTimeInSeconds);

        AnalyticsManager.RecordSessionDate(DateTime.UtcNow);
                
        sessionStartTime = Time.realtimeSinceStartup;
    }
}
