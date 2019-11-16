using System;
using Xamarin.Forms;
using ShowMe.Views;
using ShowMe.Models;
using Xamarin.Essentials;

namespace ShowMe
{
    public partial class App : Application
    {
        static public User User { get; set; }

        public App()
        {
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            var access = Connectivity.NetworkAccess;
            InitializeComponent();

            if (access == NetworkAccess.None)
            {
                MainPage = new NavigationPage(new NoInternetPage());
            }
            else if (!IsLoggedIn)
            {
                MainPage = new NavigationPage(new LoginPage());
            }
            else
            {
                MainPage = new NavigationPage(new MainPage());
            }
        }

        void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {

            var access = e.NetworkAccess;
            InitializeComponent();

            if (access == NetworkAccess.None)
            {
                MainPage = new NavigationPage(new NoInternetPage());
            }
            else
            {
                if (!IsLoggedIn)
                {
                    MainPage = new NavigationPage(new LoginPage());
                }
                else
                {
                    MainPage = new NavigationPage(new MainPage());
                }
            }
        }

        protected override void OnStart()
        {
            
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        public static bool IsLoggedIn
        {
            get { return !string.IsNullOrWhiteSpace(_Token); } set { }
        }

        static string _Token;
        public static string Token
        {
            get { return _Token; }
        }

        public static void SaveToken(string token)
        {
            _Token = token;
        }
    }
}
