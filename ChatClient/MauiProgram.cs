using ChatClient.MobilePages;
using ChatClient.Models;
using ChatClient.Utils;
using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
namespace ChatClient;

public static class MauiProgram
{
    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<LoginModel>();
        services.AddTransient<RegisterModel>();
        services.AddTransient<UserListModel>();
		services.AddTransient<SettingsModel>();
		services.AddTransient<ChatModel>();
		services.AddSingleton<UserInfoModel>();

		services.AddTransient<LoginPage>();
		services.AddTransient<RegisterPage>();
		services.AddTransient<SettingsPage>();
		services.AddTransient<UserListPage>();
		services.AddTransient<ChatPage>();

        services.AddSingleton<IClientUtils, ClientUtils>();
        services.AddSingleton<NetworkUtils>();
        services.AddSingleton<SignalRUtils>();

		
    }

    public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiCommunityToolkit()
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
		ConfigureServices(builder.Services);

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}

