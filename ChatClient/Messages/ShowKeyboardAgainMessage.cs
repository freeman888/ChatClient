using System;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace ChatClient.Messages
{
	public class ShowKeyboardAgainMessage:ValueChangedMessage<int>
	{
		public ShowKeyboardAgainMessage():base(0)
		{
		}
	}
}

