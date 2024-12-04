using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
        Title = "Login";
    }

    private void BtnLogin_OnClicked(object sender, EventArgs e)
    {
        App.SessionKey = "aaa";

        Navigation.PopModalAsync();
    }

    private void BtnCreateAccount_OnClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new NewAccountPage());
    }
}