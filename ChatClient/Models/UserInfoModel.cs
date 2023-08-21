using CommunityToolkit.Mvvm.ComponentModel;

namespace ChatClient.Models;
public partial class UserInfoModel:ObservableObject
{
    [ObservableProperty]
    string userName;

    [ObservableProperty]
    string password;

    [ObservableProperty]
    string motto;
}
public record UserInfo(string Name,string Motto,string Password);


