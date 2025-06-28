using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    public static UIManager uIManagerInstance;

    void Awake()
    {
        if (uIManagerInstance == null)
            uIManagerInstance = this;
        else
            Destroy(this);
    }



    [SerializeField]
    GameObject homeMenuUI, activitiesMenuUI, chatMenuUI, analyticsMenuUI;


# region Navigation
    public void GoToHomePage()
    {
        AppManager.appManagerInstance.SetAppState(AppManager.AppState.Home);
    }

    public void GoToActivityPage()
    {
        AppManager.appManagerInstance.SetAppState(AppManager.AppState.Activity);
    }

    public void GoToChatPage()
    {
        AppManager.appManagerInstance.SetAppState(AppManager.AppState.Chat);
    }

    public void GoToProfilePage()
    {
        AppManager.appManagerInstance.SetAppState(AppManager.AppState.Profile);
    }

    #endregion



    #region Button Clicks
    public void OnSubmit()
    {
        switch (AppManager.appManagerInstance.currentAppState)
        {
            case AppManager.AppState.Activity:
                string userResponse_A = ActivityUIHandler.activityUIManagerInstance.GetUserResponse();
                ActivityManager.activityManagerInstance.SubmitRequest(userResponse_A);
                ActivityUIHandler.activityUIManagerInstance.ClearUserResponse();
                break;

            case AppManager.AppState.Chat:
                string userResponse_C = ChatWindowUIHandler.chatWindowUIHandlerInstance.GetUserResponse();
                ChatManager.chatManagerInstance.SubmitRequest(userResponse_C);
                ChatWindowUIHandler.chatWindowUIHandlerInstance.ClearUserResponse();                
                break;

            case AppManager.AppState.Profile:
            case AppManager.AppState.None:
            case AppManager.AppState.Home:
            default:
                break;
        }
    }

    public void OnVoiceInput()
    {
        STTManager.sTTManagerInstance.StartSpeechToText();
    }
    #endregion


    //private void OnEnable()
    //{
    //    AppManager.appManagerInstance.OnAppStateChanged += SetUIState;
    //}

    //private void OnDisable()
    //{
    //    AppManager.appManagerInstance.OnAppStateChanged += SetUIState;
    //}



    public void SetUIState(AppManager.AppState appState)
    {
        switch (appState)
        {
            case AppManager.AppState.Home:
                ResetActivityUI();
                ResetChatUI();
                ResetProfileUI();

                homeMenuUI.SetActive(true);                
                break;

            case AppManager.AppState.Activity:
                ResetHomeUI();
                ResetChatUI();
                ResetProfileUI();

                activitiesMenuUI.SetActive(true);
                break;

            case AppManager.AppState.Chat:
                ResetHomeUI();
                ResetActivityUI();
                ResetProfileUI();

                chatMenuUI.SetActive(true);
                break;

            case AppManager.AppState.Profile:
                ResetHomeUI();
                ResetActivityUI();
                ResetChatUI();

                analyticsMenuUI.SetActive(true);
                break;

            default:
                break;
        }

        Debug.Log("UI Updated");
    }


    void ResetHomeUI()
    {
        if (homeMenuUI.activeSelf)
            homeMenuUI.SetActive(false);
    }


    void ResetActivityUI()
    {
        if (activitiesMenuUI.activeSelf)
            activitiesMenuUI.SetActive(false);
    }


    void ResetChatUI()
    {
        if (chatMenuUI.activeSelf)
            chatMenuUI.SetActive(false);
    }

    void ResetProfileUI()
    {
        if (analyticsMenuUI.activeSelf)
            analyticsMenuUI.SetActive(false);
    }

}
