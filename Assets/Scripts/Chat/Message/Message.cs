using System;

public class Message
{
    string _messageText = "";
    int _wordCount;
    DateTime _messageTime; 

    protected Message(string messageText)
    {
        _messageText = _messageText.Trim();
        _messageText = messageText;
        _messageTime = DateTime.UtcNow;

        CalculateWordCount();
    }

     
    public void CalculateWordCount()
    {        
        _wordCount = _messageText.Split(' ').Length;
    }
    

    public DateTime GetMessageTime()
    {
        return _messageTime;
    }

    public string GetMessageText()
    {
        return _messageText;
    }

    public int GetWordCount()
    {
        return _wordCount;
    }
    
}
