using Newtonsoft.Json;
using ShowMe.Models;
using ShowMe.Services;
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
        Account account;
        FireBaseHelper fireBaseHelper = new FireBaseHelper();
        //AccountStore store;

        public LoginPage()
        {
            InitializeComponent();

            //store = AccountStore.Create();
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

            //account = store.FindAccountsForService(Constants.AppName).FirstOrDefault();

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
            var authenticator = sender as OAuth2Authenticator;
            if (authenticator != null)
            {
                authenticator.Completed -= OnAuthCompleted;
                authenticator.Error -= OnAuthError;
            }

            User user = null;
            if (e.IsAuthenticated)
            {
                // If the user is authenticated, request their basic user data from Google
                // UserInfoUrl = https://www.googleapis.com/oauth2/v2/userinfo
                var request = new OAuth2Request("GET", new Uri(Constants.UserInfoUrl), null, e.Account);
                var response = await request.GetResponseAsync();
                if (response != null)
                {
                    // Deserialize the data and store it in the account store
                    // The users email address will be used to identify data in SimpleDB
                    string userJson = await response.GetResponseTextAsync();
                    user = JsonConvert.DeserializeObject<User>(userJson);
                    Task<bool> task = fireBaseHelper.CheckIfUserExists(user.Id);
                    await task;
                   
                    if (!task.Result)
                    {
                        await fireBaseHelper.AddUser(user.Id, user.Email , user.Picture); ;
                    }

                }

                if (account != null)
                {
                    //store.Delete(account, Constants.AppName);
                }

                //await store.SaveAsync(account = e.Account, Constants.AppName);
                await DisplayAlert("Email address", user.Email, "OK");
                //ToMainPage();
                var token = e.Account.Properties["access_token"];
                App.SaveToken(token);
                //App.CurrentApp.SuccessfulLoginAction();
                ToMainPage();
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
            await Navigation.PopModalAsync();
        }
    }
}
