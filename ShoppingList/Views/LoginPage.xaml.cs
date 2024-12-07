using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ShoppingList.Models;

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
        string data = JsonConvert.SerializeObject(new UserAccount(txtUser.Text, txtPassword.Text));
        HttpClient client = new HttpClient();
        SetSessionKey(data, client);
        //user info
        //U: Testalexuser
        //P: password
    }
    
    private async void SetSessionKey(string data, HttpClient client)
    {
        var response = await client.PostAsync(new Uri("https://joewetzel.com/fvtc/account/login"),
            new StringContent(data, Encoding.UTF8, "application/json"));
        var sessionKey = response.Content.ReadAsStringAsync().Result;

        if (!string.IsNullOrEmpty(sessionKey) && sessionKey.Length < 50)
        {
            App.SessionKey = sessionKey;
            Navigation.PopModalAsync();
        }
        else await DisplayAlert("Error", "There was an error logging you in.", "Ok");
    }
    
    private void BtnCreateAccount_OnClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new NewAccountPage());
    }
}