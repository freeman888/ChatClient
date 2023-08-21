using System.Collections.ObjectModel;
using System.Diagnostics;
using ChatClient.Messages;
using ChatClient.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Windows.Input;
using System.Text.Json;

namespace ChatClient.Models;

[QueryProperty(nameof(UserName), "username")]
[QueryProperty(nameof(Password), "password")]
public partial class UserListModel:ObservableObject
{
    private readonly SignalRUtils signalRUtils;
    private readonly IClientUtils clientUtils;
    public readonly UserInfoModel userInfoModel;

    [ObservableProperty]
    string userName;

    [ObservableProperty]
    string password;

    public ObservableCollection<UserModel> UserList { get; } = new ObservableCollection<UserModel>();

    public UserListModel(SignalRUtils _signalRUtils,IClientUtils clientUtils,UserInfoModel userInfoModel)
    {
        signalRUtils = _signalRUtils;
        this.clientUtils = clientUtils;
        this.userInfoModel = userInfoModel;
        WeakReferenceMessenger.Default.Register<LoginMessage>(this, async (r, m) =>
        {
            Debug.WriteLine($"msg received ,r:{r},m:{m.Value}");
            UserListModel userListModel = (UserListModel)r;
            await signalRUtils.TryConnect();
            var res = await signalRUtils.TryLogin(userListModel.UserName,userListModel.Password);

            if (res)
            {
                string userjson = await signalRUtils.TryGetUserInfo(userListModel.UserName);
                UserInfo userInfo = (UserInfo)JsonSerializer.Deserialize(userjson, typeof(UserInfo));
                userInfoModel.UserName = userInfo.Name;
                userInfoModel.Password = userInfo.Password;
                userInfoModel.Motto = userInfo.Motto;
                //请求用户列表
                _ = RefreshUserList();
                
            }
            else
            {
                await clientUtils.Alert("登录失败，请检查用户名和密码");
                await clientUtils.ChangeToLogin();
            }
        });
    }

    [RelayCommand]
    async Task RefreshUserList()
    {
        UserList.Clear();
        var tup = await signalRUtils.TryGetUserList(UserName);
        if (tup is null)
        {
            //登录失效，等待处理
        }
        else
        {
                foreach (var i in tup)
                {
                    UserList.Add(new(i, ""));
                }
        }
    }
    [RelayCommand]
    async Task StartChat(string user)
    {

        var dic = new Dictionary<string, object>
        {
            {"targetusername", user}
        };
        await clientUtils.ChangeToChat(dic);
    }
}

public record UserModel(string UserName,string LastMessage)
{
}


