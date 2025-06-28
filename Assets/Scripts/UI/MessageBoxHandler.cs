using UnityEngine;

public class MessageBoxHandler : MonoBehaviour
{
    [SerializeField]
    TMPro.TMP_Text messageTextBody;


    public void SetText(string targetText)
    {
        messageTextBody.text = targetText;
    }
}
