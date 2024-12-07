using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ShoppingList.Models;

namespace ShoppingList.Views;

public partial class NewAccountPage : ContentPage
{
    public NewAccountPage()
    {
        InitializeComponent();
        Title = "Create New Account";
    }

    private async void BtnCreateAccount_OnClicked(object sender, EventArgs e)
    {
        if (PrePostCheck())
        {
            //api stuff
            string data = JsonConvert.SerializeObject(new UserAccount(txtUser.Text, txtPassword.Text, txtEmail.Text));
            HttpClient client = new HttpClient();
            var response = await client.PostAsync(new Uri("https://joewetzel.com/fvtc/account/createuser"),
                new StringContent(data, Encoding.UTF8, "application/json"));
            var accountStatus = response.Content.ReadAsStringAsync().Result;

            //post post check... hahaha post post
            //does the user exist?
            if (accountStatus == "user exists")
            {
                await DisplayAlert("Error", "Sorry name is already in use.", "Ok");
                return;
            }

            //is email in use
            if (accountStatus == "email exists")
            {
                await DisplayAlert("Error", "Email is already in use.", "Ok");
                return;
            }
            
            if (accountStatus == "complete") SetSessionKey(data, client);
            else
            {
                await DisplayAlert("Error", "Sorry an error accorded creating your account.", "Ok");
                return;
            }
        }
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
    
    
    
    private bool PrePostCheck()
    {
        //check username length && if username is filled in
        if (string.IsNullOrEmpty(txtUser.Text))
        {
            DisplayAlert("No data", "please enter a username.", "Ok");
            return false;
        }
        if (txtUser.Text.Length < 6)
        {
            DisplayAlert("Bad Data", "Please make username that is at least 6 characters long.", "Ok");
            return false;
        }
        //check if passwords match
        if (string.IsNullOrEmpty(txtPassword.Text) || string.IsNullOrEmpty(txtPasswordReEntry.Text))
        {
            DisplayAlert("No data", "please enter a password.", "Ok");
            return false;
        }
        if (txtPassword.Text.Length < 6)
        {
            DisplayAlert("Bad Data", "Please make password that is at least 6 characters long.", "Ok");
            return false;
        }
        if (0 != string.Compare(txtPassword.Text, txtPasswordReEntry.Text))
        {
            DisplayAlert("Bad Data", "Please make sure that both passwords match", "Ok");
            return false;
        }
        //check if its a valid email address ( @ before . and it needs both) 
        if (string.IsNullOrEmpty(txtEmail.Text))
        {
            DisplayAlert("No data", "please enter a email.", "Ok");
            return false;
        }
        if (!txtEmail.Text.Contains("@") || !txtEmail.Text.Contains("."))
        {
            DisplayAlert("Bad data", "Please make sure that the email has an @ or .", "Ok");
            return false;
        }
        if (txtEmail.Text.IndexOf("@") > txtEmail.Text.IndexOf("."))
        {
            DisplayAlert("Bad data", "Please make sure that the @ is before the .", "Ok");
            return false;
        }

        return true;
    }
}