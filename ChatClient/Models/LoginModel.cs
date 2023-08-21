using System.Diagnostics;
using ChatClient.Messages;
using ChatClient.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace ChatClient.Models;

[QueryProperty(nameof(UserName),"username")]
[QueryProperty(nameof(Password),"password")]
public partial class LoginModel:ObservableObject
{
    private readonly IClientUtils utils;
    private readonly NetworkUtils netWorkUtils;
    public LoginModel(IClientUtils _utils,NetworkUtils _networkUtils)
    {
        utils = _utils;
        netWorkUtils = _networkUtils;
    }
    [ObservableProperty]
    public string userName;

    [ObservableProperty]
    public string password;

    [RelayCommand]
    async Task Login()
    {
        Debug.WriteLine($"{UserName} start login");
        await utils.Login(new Dictionary<string,object> { { "username", UserName }, { "password",Password} });
    }


    [RelayCommand]
    void ChangeToRegister()
    {
        Debug.WriteLine("user want to register");
        utils.ChangeToRegister();
    }

    [RelayCommand]
    void ChangeToPasswordInput()
    {
        utils.ChangeToPasswordInput();
    }
}


