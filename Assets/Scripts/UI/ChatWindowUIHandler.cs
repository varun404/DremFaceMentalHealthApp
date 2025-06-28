using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class ChatWindowUIHandler : MonoBehaviour
{

    public static ChatWindowUIHandler chatWindowUIHandlerInstance;    

    void Awake()
    {
        if (chatWindowUIHandlerInstance == null)
            chatWindowUIHandlerInstance = this;
        else
            Destroy(this);
    }

    [SerializeField]
    ScrollRect chatWindowScrollRect;

    [SerializeField]
    RectTransform content;

    [SerializeField]
    VerticalLayoutGroup contentVerticalLayoutGroup;

    [SerializeField]
    Button microPhoneButton;

    [SerializeField]
    Button submitButton;


    [SerializeField]
    GameObject aiMessagePrefab;

    [SerializeField]
    GameObject userMessagePrefab;


    [SerializeField]
    TMPro.TMP_InputField userResponseIF;

    //private void OnEnable()
    //{
    //    ChatManager.chatManagerInstance.currentConversation.OnNewMessageAdded -= ((Message newMessage) =>
    //    {
    //        StartCoroutine(AccomodateNewMessage(newMessage));
    //    });
    //}

    //private void OnDisable()
    //{
    //    ChatManager.chatManagerInstance.currentConversation.OnNewMessageAdded -= ((Message newMessage) =>
    //    { });
    //}

    public void AddMessageToUI(Message newMessage)
    {        
        StartCoroutine(AccomodateNewMessage(newMessage));        
    }


    GameObject newSpawnedMessage;
    IEnumerator AccomodateNewMessage(Message newMessage)
    {        
        Debug.Log("Adding new Message to UI...");
        if(newMessage is AIMessage)
        {            
            newSpawnedMessage = Instantiate(aiMessagePrefab, content);                                    
        }
        else if(newMessage is UserMessage)
        {
            newSpawnedMessage = Instantiate(userMessagePrefab, content);        
        }

        // Data Hand off from object to UI
        newSpawnedMessage.GetComponent<MessageBoxHandler>().SetText(newMessage.GetMessageText());



        Debug.Log("Adding New Message");

        AdjustTopPadding(100);

        yield return null;

        ScrollToBottom();

        yield return null;

        Canvas.ForceUpdateCanvases();

        yield break;
    }


    void AdjustTopPadding(int paddingOffset)
    {
        RectOffset newPadding = new RectOffset(
                contentVerticalLayoutGroup.padding.left,
                contentVerticalLayoutGroup.padding.right,
                contentVerticalLayoutGroup.padding.top,
                contentVerticalLayoutGroup.padding.bottom);

        newPadding.top -= paddingOffset;

        contentVerticalLayoutGroup.padding = newPadding;
    }


    public void ScrollToBottom()
    {
        chatWindowScrollRect.verticalNormalizedPosition = 0f;
    }

    public string GetUserResponse()
    {
        return userResponseIF.text.Length > 0 ? userResponseIF.text : "";
    }

    public void ClearUserResponse()
    {
        userResponseIF.text = "";
    }

    public void SetUserResponse(string responseText)
    {
        userResponseIF.text += responseText;
    }
    
}
