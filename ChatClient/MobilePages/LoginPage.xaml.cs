using System.Diagnostics;
using ChatClient.Messages;
using ChatClient.Models;
using CommunityToolkit.Maui.Core.Platform;
using CommunityToolkit.Mvvm.Messaging;

namespace ChatClient.MobilePages;

public partial class LoginPage : ContentPage
{
    readonly LoginModel loginModel;
	public LoginPage(LoginModel loginModel)
	{
		this.loginModel = loginModel;
		this.BindingContext = loginModel;
		InitializeComponent();
		WeakReferenceMessenger.Default.Register<ChangeToPasswordInputMessage>(this,async (r, m) =>
		{
			await passwordEntry.ShowKeyboardAsync(CancellationToken.None);
        });
	}

    
}
