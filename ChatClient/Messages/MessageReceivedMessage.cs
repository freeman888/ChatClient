using ChatClient.Models;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace ChatClient.Messages;
public class MessageReceivedMessage : ValueChangedMessage<int>
{
    public Message Message { get; set; }
    public MessageReceivedMessage(Message message) : base(0)
    {
        Message = message;
    }
}


