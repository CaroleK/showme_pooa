using Newtonsoft.Json;
using ShowMe.Models;
using ShowMe.Services;
using ShowMe.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShowMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {

        public LoginPage()
        {
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            return false;
        }

        void OnLoginClicked(object sender, EventArgs e)
        {
            string clientId = Constants.AndroidClientId;
            string redirectUri = Constants.AndroidRedirectUrl;

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    clientId = Constants.iOSClientId;
                    redirectUri = Constants.iOSRedirectUrl;
                    break;

                case Device.Android:
                    clientId = Constants.AndroidClientId;
                    redirectUri = Constants.AndroidRedirectUrl;
                    break;
            }

            // TODO: Unable the annoying "customtabs" message

            var authenticator = new OAuth2Authenticator(
                clientId,
                null,
                Constants.Scope,
                new Uri(Constants.AuthorizeUrl),
                new Uri(redirectUri),
                new Uri(Constants.AccessTokenUrl),
                null,
                true);

            authenticator.Completed += OnAuthCompleted;
            authenticator.Error += OnAuthError;

            AuthenticationState.Authenticator = authenticator;

            Xamarin.Auth.Presenters.OAuthLoginPresenter presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
            presenter.Login(authenticator);
        }

        async void OnAuthCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {
            if (sender is OAuth2Authenticator authenticator)
            {
                authenticator.Completed -= OnAuthCompleted;
                authenticator.Error -= OnAuthError;
            }

            if (e.IsAuthenticated)
            {
                LoginActivityIndicator.IsRunning = true;
                LoginActivityIndicator.IsEnabled = true;
                LoginActivityIndicator.IsVisible = true;

                await FetchOrCreateUser(e.Account);

                var token = e.Account.Properties["access_token"];
                App.SaveToken(token);

                ToMainPage();
            }
        }

        async Task FetchOrCreateUser(Xamarin.Auth.Account account)
        {
            // If the user is authenticated, request their basic user data from Google
            // UserInfoUrl = https://www.googleapis.com/oauth2/v2/userinfo
            var request = new OAuth2Request("GET", new Uri(Constants.UserInfoUrl), null, account);
            var response = await request.GetResponseAsync();
            if (response != null)
            {
                // Deserialize the data and store it in the account store
                // The users email address will be used to identify data in SimpleDB
                string userJson = await response.GetResponseTextAsync();
                User user = JsonConvert.DeserializeObject<User>(userJson);
                Task<bool> task = FireBaseHelper.CheckIfUserExists(user.Id);
                await task;

                if (!task.Result)
                {
                    await FireBaseHelper.AddUser(user.Id, user.Email, user.Picture); ;
                }
                BaseViewModel.User = user;
                App.User = user;
            }
        }

        void OnAuthError(object sender, AuthenticatorErrorEventArgs e)
        {
            var authenticator = sender as OAuth2Authenticator;
            if (authenticator != null)
            {
                authenticator.Completed -= OnAuthCompleted;
                authenticator.Error -= OnAuthError;
            }

            Debug.WriteLine("Authentication error: " + e.Message);
        }
        async void ToMainPage()
        {
            await Navigation.PushAsync(new MainPage());
        }
    }
}
