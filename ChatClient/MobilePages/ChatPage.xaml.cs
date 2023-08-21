using System.Globalization;
using ChatClient.Messages;
using ChatClient.Models;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Maui.Core.Platform;
using System.Diagnostics;

namespace ChatClient.MobilePages;

public partial class ChatPage : ContentPage
{
    ChatModel chatModel;
    public ChatPage(ChatModel chatModel)
    {
        InitializeComponent();
        this.chatModel = chatModel;
        this.BindingContext = chatModel;
        WeakReferenceMessenger.Default.Register<ContentScrollMessage>(this, (r, m) =>
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                ContentCV.ScrollTo(m.Value - 1, position: ScrollToPosition.End);

            });
        });
        WeakReferenceMessenger.Default.Register<ShowKeyboardAgainMessage>(this, async (r, m) =>
        {
            await editor.ShowKeyboardAsync(CancellationToken.None);
        });
        WeakReferenceMessenger.Default.Register<PickImageMessage>(this, (r, m) =>
        {
            m.Reply(MainThread.InvokeOnMainThreadAsync(async () =>
            {
                bool done = true;
                string action = await DisplayActionSheet("拍照or选取", "Cancel", null, "拍照", "从相册选取", "从文件选取");
                Stream stream = null;
                switch (action)
                {
                    case "拍照":
                        if (MediaPicker.Default.IsCaptureSupported)
                        {
                            FileResult photofile = await MediaPicker.Default.CapturePhotoAsync();
                            stream = await photofile.OpenReadAsync();
                        }
                        else
                        {
                            done = false;
                        }
                        break;
                    case "从相册选取":
                        FileResult photofile1 = await MediaPicker.Default.PickPhotoAsync();
                        stream = await photofile1.OpenReadAsync();
                        break;
                    case "从文件选取":
                        try
                        {
                            var result = await FilePicker.PickAsync();
                            if (result != null)
                            {
                                if (result.FileName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase) ||
                                    result.FileName.EndsWith("png", StringComparison.OrdinalIgnoreCase))
                                {
                                    stream = await result.OpenReadAsync();
                                }
                            }
                            else
                                done = false;
                        }
                        catch
                        {
                            done = false;
                        }
                        break;
                    default:
                        done = false;
                        break;
                }
                return (done, stream);
            }));
        });
        editor.Unfocused += (s, e) =>
        {
            //((Entry)s).Focus();
        };
    }

    void editor_Focused(System.Object sender, Microsoft.Maui.Controls.FocusEventArgs e)
    {
    }

    async void ContentPage_Loaded(System.Object sender, System.EventArgs e)
    {
        await editor.ShowKeyboardAsync(CancellationToken.None);
    }

    void ContentCV_Scrolled(System.Object sender, Microsoft.Maui.Controls.ItemsViewScrolledEventArgs e)
    {
        //Debug.WriteLine(e.VerticalDelta);
    }
}
