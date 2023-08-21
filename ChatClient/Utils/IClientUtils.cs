using System;
namespace ChatClient.Utils;

public interface IClientUtils
{
    Task Alert(string message, string title="提示", string cancel="确定");
    Task ChangeToRegister();
    Task ChangeToLogin(IDictionary<string, object> param = null);
    void ChangeToPasswordInput();
    Task Login(IDictionary<string, object> param);
    Task ChangeToChat(IDictionary<string, object> param);
    void ChangeTheme(bool darkTheme);
    void ShowKeyboardAgain();
    Task<(bool, Stream)> GetImage();
}

