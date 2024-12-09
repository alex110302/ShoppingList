using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ShoppingList.Models;

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
        lstData.Refreshing += delegate
        {
            LoadData();
            lstData.IsRefreshing = false;
        };
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
        if (string.IsNullOrEmpty(App.SessionKey)) Navigation.PushModalAsync(new NavigationPage(_loginPage));
        else LoadData();
    }

    private async void BtnLogout_OnClicked(object sender, EventArgs e)
    {
        string data = JsonConvert.SerializeObject(new UserAccount(App.SessionKey));
        HttpClient client = new HttpClient();
        await client.PostAsync(new Uri("https://joewetzel.com/fvtc/account/logout"),
            new StringContent(data, Encoding.UTF8, "application/json"));
        
        App.SessionKey = "";
        OnAppStart();
    }

    private async void BtnAdd_OnClicked(object sender, EventArgs e)
    {
        string data = JsonConvert.SerializeObject(new UserData( null, txtInput.Text, App.SessionKey));
        HttpClient client = new HttpClient();
        await client.PostAsync(new Uri("https://joewetzel.com/fvtc/account/data"),
            new StringContent(data, Encoding.UTF8, "application/json"));

        txtInput.Text = string.Empty;
        LoadData();
    }

    public async void LoadData()
    {
        var response = await new HttpClient().GetAsync(new Uri($"https://joewetzel.com/fvtc/account/data/{App.SessionKey}"));
        var wsJson = response.Content.ReadAsStringAsync().Result;
        var userDataObject = JsonConvert.DeserializeObject<UserDataCollection>(wsJson);

        lstData.ItemsSource = userDataObject.UserDataItems;
    }

    private async void MenuItem_OnClicked(object sender, EventArgs e)
    {
        var dataID = ((MenuItem)sender).CommandParameter.ToString();
        var data = JsonConvert.SerializeObject(new UserData(dataID, null, App.SessionKey));

        var client = new HttpClient();
        
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Delete,
            RequestUri = new Uri("https://joewetzel.com/fvtc/account/data"),
            Content = new StringContent(data, Encoding.UTF8, "application/json")
        };

        await client.SendAsync(request);
        
        LoadData();
    }

    private async void BtnClearList_OnClicked(object sender, EventArgs e)
    {
        var data = JsonConvert.SerializeObject(new UserData(null, null, App.SessionKey));
        var test = App.SessionKey;
        var client = new HttpClient();
        
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Delete,
            RequestUri = new Uri("https://joewetzel.com/fvtc/account/data"),
            Content = new StringContent(data, Encoding.UTF8, "application/json")
        };

        await client.SendAsync(request);
        
        LoadData();
    }
}