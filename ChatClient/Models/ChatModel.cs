using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using ChatClient.Messages;
using ChatClient.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.Controls;

namespace ChatClient.Models;

public record Message(string SendUser, string ReceiveUser, string Content, DateTime SendTime, int MessageID, byte[] Img);
[QueryProperty(nameof(TargetUserName),"targetusername")]
public partial class ChatModel : ObservableObject
{
    public ChatModel(UserInfoModel userInfoModel,SignalRUtils signalRUtils,IClientUtils clientUtils)
    {
        
        this.clientUtils = clientUtils;
        this.userInfoModel = userInfoModel;
        this.signalRUtils = signalRUtils;
        WeakReferenceMessenger.Default.Register<MessageReceivedMessage>(this, (r, m) =>
        {
            Messages.Add(m.Message);
            WeakReferenceMessenger.Default.Send(new ContentScrollMessage(Messages.Count()));
        });
    }

    private readonly UserInfoModel userInfoModel;
    private readonly SignalRUtils signalRUtils;
    private readonly IClientUtils clientUtils;

    public ObservableCollection<Message> Messages { get; set; } = new();

    [ObservableProperty]
    string targetUserName;

    async partial void OnTargetUserNameChanged(string oldValue, string newValue)
    {
        var res = await signalRUtils.TryGetMessages(userInfoModel.UserName, newValue);
        Messages.Clear();
        foreach(var i in res)
        {
            Messages.Add(i);
        }
        WeakReferenceMessenger.Default.Send(new ContentScrollMessage(Messages.Count()));
    }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SendMessageCommand))]
    [NotifyCanExecuteChangedFor(nameof(SendMessageFromKeyboardCommand))]
    string message;
    bool CanSendMessage => !string.IsNullOrWhiteSpace(Message);


    
    [RelayCommand(CanExecute = nameof(CanSendMessage))]
    async Task SendMessage()
    {
        var res = await signalRUtils.TrySendMessage(userInfoModel.UserName, TargetUserName, Message);
        if (res)
        {
            Message = "";
        }
    }

    [RelayCommand(CanExecute = nameof(CanSendMessage))]
    async Task SendMessageFromKeyboard()
    {
        await SendMessage();
        clientUtils.ShowKeyboardAgain(); 
    }

    [RelayCommand]
    async Task SendImage()
    {
        var res = await clientUtils.GetImage();
        if (res.Item1)
        {
            _ = await signalRUtils.TrySendImage(userInfoModel.UserName, TargetUserName, res.Item2);
        }
    }

}



