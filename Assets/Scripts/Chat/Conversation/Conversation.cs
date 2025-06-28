using System;
using System.Collections.Generic;

public class Conversation
{
    static Stack<Message> conversationMessages = conversationMessages = new Stack<Message>();
    
    //public event Action<Message> OnNewMessageAdded;


    public void AddToConversation(Message newMessage)
    {                
        if (newMessage != null)
        {
            conversationMessages.Push(newMessage);
            //OnNewMessageAdded?.Invoke(newMessage);
            //ChatWindowUIManager.chatWindowUIManagerInstance.AddMessageToUI(newMessage);
        }
    }

    public Message GetLastMessage()
    {
        return conversationMessages.Count > 0 ? conversationMessages.Peek() : null;
    }

    public int GetConversationLength()
    {
        return conversationMessages.Count;
    }
}
