using System.IO;
using ChatClient.Messages;
using CommunityToolkit.Mvvm.Messaging;

namespace ChatClient.Utils;

public class ClientUtils : IClientUtils
{
    public async Task Alert(string message, string title, string cancel)
    {

        if (MainThread.IsMainThread)
            await Shell.Current.CurrentPage.DisplayAlert(title: title, message: message, cancel: cancel);
        else
            await MainThread.InvokeOnMainThreadAsync(async () => await Shell.Current.CurrentPage.DisplayAlert(title: title, message: message, cancel: cancel));
    }

    public async Task ChangeToRegister()
    {
        if (MainThread.IsMainThread)
            await Shell.Current.GoToAsync("RegisterPage");
        else
            await MainThread.InvokeOnMainThreadAsync(async () => await Shell.Current.GoToAsync("RegisterPage"));
    }
    public async Task ChangeToLogin(IDictionary<string, object> param)
    {
        if (param is null)
            if (MainThread.IsMainThread)
                await Shell.Current.GoToAsync("//LoginPage");
            else
                await MainThread.InvokeOnMainThreadAsync(async () => await Shell.Current.GoToAsync("//LoginPage"));
        else
            if (MainThread.IsMainThread)
            await Shell.Current.GoToAsync("//LoginPage", param);
        else
            await MainThread.InvokeOnMainThreadAsync(async () => await Shell.Current.GoToAsync("//LoginPage", param));


    }
    public async Task ChangeToChat(IDictionary<string, object> param)
    {
        if (MainThread.IsMainThread)
            await Shell.Current.GoToAsync("ChatPage", param);
        else
            await MainThread.InvokeOnMainThreadAsync(async () => await Shell.Current.GoToAsync("ChatPage", param));

    }

    public async Task Login(IDictionary<string, object> param)
    {
        if (MainThread.IsMainThread)
        {
            await Shell.Current.GoToAsync("//UserListPage", param);
        }
        else
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await Shell.Current.GoToAsync("//UserListPage", param);
            });
        }
        await Task.Delay(500);
        WeakReferenceMessenger.Default.Send(new LoginMessage());
    }

    public void ChangeToPasswordInput()
    {
        WeakReferenceMessenger.Default.Send<ChangeToPasswordInputMessage>();
    }
    public void ChangeTheme(bool dark)
    {

        Application.Current.UserAppTheme = dark ? AppTheme.Dark : AppTheme.Light;

    }

    public void ShowKeyboardAgain()
    {

        WeakReferenceMessenger.Default.Send<ShowKeyboardAgainMessage>();
    }

    public async Task<(bool,Stream)> GetImage()
    {
        var res = await WeakReferenceMessenger.Default.Send<PickImageMessage>();
        return res;
    }
}


