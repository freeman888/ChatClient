using CommunityToolkit.Mvvm.Messaging.Messages;

namespace ChatClient.Messages;
public class ContentScrollMessage : ValueChangedMessage<int>
{
    public ContentScrollMessage(int v) : base(v)
    {

    }
}


