using CommunityToolkit.Mvvm.Messaging.Messages;
namespace ChatClient.Messages;

public class SetUserListMessage : ValueChangedMessage<int>
{
    public SetUserListMessage() : base(0)
    {
    }
}


