using System.Diagnostics;
using ChatClient.Models;

namespace ChatClient.MobilePages;

public partial class RegisterPage : ContentPage
{
	readonly RegisterModel registerModel;
	public RegisterPage(RegisterModel registerModel)
	{
		this.registerModel = registerModel;
		this.BindingContext = registerModel;
		InitializeComponent();
	}

	

	

	void ContentPage_Appearing(System.Object sender, System.EventArgs e)
	{
		registerModel.UserName = string.Empty;
		registerModel.Password = string.Empty;
		registerModel.PasswordTwice = string.Empty;
		registerModel.Motto = string.Empty;
	}
}
