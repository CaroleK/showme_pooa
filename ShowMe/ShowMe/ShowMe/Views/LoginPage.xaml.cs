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

        /// <summary>
        /// Makes it impossible to navigate with back button on login page
        /// </summary>
        /// <returns></returns>
        protected override bool OnBackButtonPressed()
        {
            return false;
        }

        /// <summary>
        /// Actions to take when user cooses to log in
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnLoginClicked(object sender, EventArgs e)
        {
            // retrieves client Id and the redirecting url constants, used to login with Google
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

            // Authentification
            var authenticator = new OAuth2Authenticator(
                clientId,
                null,
                Constants.Scope,
                new Uri(Constants.AuthorizeUrl),
                new Uri(redirectUri),
                new Uri(Constants.AccessTokenUrl),
                null,
                true);

            // Events linked to authenification
            authenticator.Completed += OnAuthCompleted;
            authenticator.Error += OnAuthError;

            // Save the authentificator instance
            AuthenticationState.Authenticator = authenticator;

            // Begin the process
            Xamarin.Auth.Presenters.OAuthLoginPresenter presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
            presenter.Login(authenticator);
        }

        /// <summary>
        /// Called when authentification process completes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void OnAuthCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {
            if (sender is OAuth2Authenticator authenticator)
            {
                authenticator.Completed -= OnAuthCompleted;
                authenticator.Error -= OnAuthError;
            }

            if (e.IsAuthenticated)
            {
                // Display loading indicator
                LoginActivityIndicator.IsRunning = true;
                LoginActivityIndicator.IsEnabled = true;
                LoginActivityIndicator.IsVisible = true;
                LoginActivityIndicatorLayout.IsVisible = true;
                LoginActivityIndicatorLayout.IsEnabled = true;

                // Try to retrieve logged in user
                bool success = await FetchOrCreateUser(e.Account);

                // If successful, move on to main page
                if (success)
                {
                    var token = e.Account.Properties["access_token"];
                    App.SaveToken(token);

                    ToMainPage();
                    return;
                }
            }

            // If something went wrong, send user back to Login page
            App.IsLoggedIn = false;
            App.Current.MainPage = new NavigationPage(new LoginPage());
            DependencyService.Get<IMessage>().Show("Sorry, authentification failed.");
        }

        /// <summary>
        /// Checks database for existing user with correct ID, or creates a new user otherwise
        /// </summary>
        /// <param name="account"></param>
        /// <returns>True is a user was successfully fetched or created, false otherwise</returns>
        async Task<bool> FetchOrCreateUser(Xamarin.Auth.Account account)
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

                User loggedinUser;

                if (!task.Result)
                {
                    bool success = await FireBaseHelper.AddUser(user.Id, user.Name, user.Picture);
                    loggedinUser = success ? user : null;
                   
                }
                // Retrieve User information (especially data about episodes watched)
                else
                {
                    loggedinUser = Task.Run(()=>FireBaseHelper.RetrieveUser(user.Id)).Result;                    
                }

                // Check if a problem occured leading to a null user
                if (loggedinUser == null)
                {                    
                    return false;
                }
                else
                {
                    // Re-initialize MyShowsCollection for user that just logged in
                    App.User = loggedinUser;
                    MyShowsCollection.Instance = null;
                    return true;
                }                
            }
            return false;
        }

        /// <summary>
        /// Called if an error occured during the authentification process
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnAuthError(object sender, AuthenticatorErrorEventArgs e)
        {
            var authenticator = sender as OAuth2Authenticator;
            if (authenticator != null)
            {
                authenticator.Completed -= OnAuthCompleted;
                authenticator.Error -= OnAuthError;
            }

            DependencyService.Get<IMessage>().Show("Sorry, authentification failed.");
            Debug.WriteLine("Authentication error: " + e.Message);
        }

        /// <summary>
        /// Move on to main page
        /// </summary>
        async void ToMainPage()
        {
            await Navigation.PushAsync(new MainPage());

            // Hide loading screen
            LoginActivityIndicator.IsRunning = false;
            LoginActivityIndicator.IsEnabled = false;
            LoginActivityIndicator.IsVisible = false;
            LoginActivityIndicatorLayout.IsVisible = false;
            LoginActivityIndicatorLayout.IsEnabled = false;

            // Warn that user has logged in, triggers alarmManager for notifications scheduling
            MessagingCenter.Send<LoginPage>(this, "UserLoggedIn");
        }
    }
}
