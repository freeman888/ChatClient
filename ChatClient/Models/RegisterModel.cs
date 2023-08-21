using System.Diagnostics;
using System.Text.Json;
using ChatClient.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ChatClient.Models;

public partial class RegisterModel : ObservableObject
{
    private readonly IClientUtils utils;
    private readonly NetworkUtils netWorkUtils;
    public RegisterModel(IClientUtils _utils, NetworkUtils _networkUtils)
    {
        utils = _utils;
        netWorkUtils = _networkUtils;
    }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
    string userName;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsPasswordSame))]
    [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
    string password;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsPasswordSame))]
    [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
    string passwordTwice;

    [ObservableProperty]
    string motto;

    public bool IsPasswordSame => Password == PasswordTwice;
    public bool CanRegister => IsPasswordSame && !string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password);

    [RelayCommand]
    void Back()
    {
        utils.ChangeToLogin();
    }

    [RelayCommand(CanExecute = nameof(CanRegister))]
    async Task Register()
    {
        Debug.WriteLine($"{UserName} want to register");
        var obj = new { Name = UserName ,Motto = Motto, Password = Password};
        var json = JsonSerializer.Serialize(obj,obj.GetType());
        var res = await netWorkUtils.SendRegister(json);
        if (res == System.Net.HttpStatusCode.Created)
        {
            await utils.Alert("注册成功，请登录!");
            var dic = new Dictionary<string, object> { { "username", UserName }, { "password", Password } };
            await utils.ChangeToLogin(dic);
        }
        else if(res == System.Net.HttpStatusCode.Conflict)
        {
            await utils.Alert("用户名已被注册，请重试!");
        }
        else
        {
            await utils.Alert("网络出现错误，请检查！");
        }
    }

    
}


