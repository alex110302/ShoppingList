using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList.Views;

public partial class MainPage : ContentPage
{
    private LoginPage _loginPage = new LoginPage();
    
    public MainPage()
    {
        InitializeComponent();
        Title = "Shopping List Pro";
        Loaded += MainPage_Loaded;
        _loginPage.Unloaded += LoginPage_Unloaded;
    }

    private void LoginPage_Unloaded(object sender, EventArgs e)
    {
        OnAppStart();
    }

    private void MainPage_Loaded(object sender, EventArgs e)
    {
        OnAppStart();
    }

    public void OnAppStart()
    {
        if (string.IsNullOrEmpty(App.SessionKey))  Navigation.PushModalAsync(new NavigationPage(_loginPage));
    }

    private void BtnLogout_OnClicked(object sender, EventArgs e)
    {
        App.SessionKey = "";
        OnAppStart();
    }
}