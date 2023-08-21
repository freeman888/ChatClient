using System;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace ChatClient.Messages
{
	public class ChangeToPasswordInputMessage:ValueChangedMessage<int>
	{
		public ChangeToPasswordInputMessage():base(0)
		{
		}
	}
}

