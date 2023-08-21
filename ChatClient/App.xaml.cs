using ChatClient.MobilePages;
namespace ChatClient;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
		MainPage = new AppShell();
		Routing.RegisterRoute("RegisterPage", typeof(RegisterPage));
		Routing.RegisterRoute("ChatPage", typeof(ChatPage));
	}
}

