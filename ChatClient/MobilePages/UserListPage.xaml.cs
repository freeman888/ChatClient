using ChatClient.Models;

namespace ChatClient.MobilePages;

public partial class UserListPage : ContentPage
{
	private readonly UserListModel userListModel;
	public UserListPage(UserListModel userListModel)
	{
		this.userListModel = userListModel;
		this.BindingContext = userListModel;
		InitializeComponent();
	}

    void ContentPage_Appearing(object sender, EventArgs e)
    {

    }

    void Button_Clicked(System.Object sender, System.EventArgs e)
    {
    }
}
