using System;
using ChatClient.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ChatClient.Models;
public partial class SettingsModel : ObservableObject
{
    public UserInfoModel UserInfoModel { get; init; }
    private readonly IClientUtils clientUtils;
    public SettingsModel(UserInfoModel userInfoModel,IClientUtils clientUtils)
    {
        this.clientUtils = clientUtils;
        UserInfoModel = userInfoModel;
        DarkTheme = Application.Current.UserAppTheme == AppTheme.Dark;
    }

    [ObservableProperty]
    bool darkTheme;

    partial void OnDarkThemeChanged(bool oldValue, bool newValue)
    {
        clientUtils.ChangeTheme(newValue);
    }

    [RelayCommand]
    async Task Logout()
    {
        await clientUtils.ChangeToLogin();
    }

}


