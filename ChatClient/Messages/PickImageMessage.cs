using ChatClient.Models;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace ChatClient.Messages;
public class PickImageMessage : AsyncRequestMessage<(bool,Stream)>
{
}


