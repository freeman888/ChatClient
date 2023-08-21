using ChatClient.Models;

namespace ChatClient.MobilePages;

public partial class SettingsPage : ContentPage
{
	SettingsModel settingsModel;
	public SettingsPage(SettingsModel settingsModel)
	{
		this.settingsModel = settingsModel;
		InitializeComponent();
        this.BindingContext = settingsModel;
    }

    void Switch_Toggled(System.Object sender, Microsoft.Maui.Controls.ToggledEventArgs e)
    {
    }
}
