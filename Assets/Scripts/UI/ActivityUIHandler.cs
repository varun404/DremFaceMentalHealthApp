using UnityEngine;

public class ActivityUIHandler : MonoBehaviour
{
    public static ActivityUIHandler activityUIManagerInstance;

    private void Awake()
    {
        if (activityUIManagerInstance == null)
            activityUIManagerInstance = this;
        else
            Destroy(this);
    }

    [SerializeField]
    TMPro.TMP_InputField userResponseIF;

    [SerializeField]
    TMPro.TMP_Text aiAssessmentResponseText;
   

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ClearUserResponse();
        ClearAIResponse();
    }

    public void SetUserResponse(string responseText)
    {
        userResponseIF.text += responseText;
    }

    public string GetUserResponse()
    {
        return userResponseIF.text.Length > 0 ? userResponseIF.text : "";
    }

    public void ClearUserResponse()
    {
        userResponseIF.text = "";
    }

    public void SetAIAssessmentResponse(string responseText)
    {
        aiAssessmentResponseText.text = responseText;
    }

    public void ClearAIResponse()
    {
        aiAssessmentResponseText.text = "";
    }

}
