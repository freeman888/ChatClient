using CommunityToolkit.Mvvm.Messaging.Messages;

namespace ChatClient.Messages;
public class LoginMessage:ValueChangedMessage<int>
{
    public LoginMessage():base(0)
    {

    }
}


