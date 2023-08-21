using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.Json;
using ChatClient.Messages;
using ChatClient.Models;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.AspNetCore.SignalR.Client;

namespace ChatClient.Utils;
public class SignalRUtils
{
    private readonly HubConnection hubConnection;
    private readonly IClientUtils clientUtils;
    int reconnectCount = 0;
    public SignalRUtils(IClientUtils clientUtils)
    {
        this.clientUtils = clientUtils;
        App.Current.Resources.TryGetValue("serverLocation", out object loc);
        string path = loc.ToString() + "/chat";
        hubConnection = new HubConnectionBuilder()
            .WithUrl(path)
            .WithAutomaticReconnect()
            .Build();
        hubConnection.On<string>("ClientReceiveMessage", message =>
        {
            var mes =  JsonSerializer.Deserialize<Message>(message);
            WeakReferenceMessenger.Default.Send(new MessageReceivedMessage(mes));
        });
        hubConnection.Closed += async (err) =>
        {
            await clientUtils.Alert("连接出现问题,请退出重试");
            await clientUtils.ChangeToLogin();
            
        };
    }
    public async Task TryConnect()
    {
        try
        {
            await Task.Run(async () =>
            {
                while (hubConnection.State != HubConnectionState.Connected)
                {
                    Debug.WriteLine("start connecting to server");
                    try
                    {
                        await hubConnection.StartAsync().WaitAsync(TimeSpan.FromMilliseconds(3000));
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.ToString());
                    }
                }
            }).WaitAsync(TimeSpan.FromMilliseconds(30000));
        }
        catch
        {
            await clientUtils.Alert("连接超时，请检查网络后重试");
            await clientUtils.ChangeToLogin();
        }
    }
    public async Task<bool> TryLogin(string username,string password)
    {
        Debug.Assert(State == HubConnectionState.Connected);
        var res = await hubConnection.InvokeAsync<bool>("ServerLogin", username, password);
        return res;
    }
    public async Task<List<string>> TryGetUserList(string username)
    {
        Debug.Assert(State == HubConnectionState.Connected);
        var tup = await hubConnection.InvokeAsync<List<string>>("ServerGetUserList", username);
        return tup;
    }
    public async Task<string> TryGetUserInfo(string username)
    {
        Debug.Assert(State == HubConnectionState.Connected);
        var res = await hubConnection.InvokeAsync<string>("ServerGetUserInfo", username);
        return res;
    }
    public async Task<ObservableCollection<Message>> TryGetMessages(string username,string targetusername)
    {
        Debug.Assert(State == HubConnectionState.Connected);
        var res = await hubConnection.InvokeAsync<string>("ServerGetAllMessages", username, targetusername);
        var obj = JsonSerializer.Deserialize<ObservableCollection<Message>>(res);
        return obj;
    }
    public async Task<bool> TrySendMessage(string username,string targetusername,string content)
    {
        Debug.Assert(State == HubConnectionState.Connected);
        var res =  await hubConnection.InvokeAsync<bool>("ServerSendMessage", username, targetusername, content,null);
        return res; 
    }
    public async Task<bool> TrySendImage(string username, string targetusername, Stream imgstream)
    {
        Debug.Assert(State == HubConnectionState.Connected);
        using var ms = new MemoryStream();
        imgstream.CopyTo(ms);
        byte[] arg4 = ms.ToArray();
        var res = await hubConnection.InvokeAsync<bool>("ServerSendMessage", username, targetusername, "", arg4);
        return res;
    }
    public HubConnectionState State => hubConnection.State;
}


