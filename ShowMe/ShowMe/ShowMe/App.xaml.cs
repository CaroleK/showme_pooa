using System;
using Xamarin.Forms;
using ShowMe.Views;
using ShowMe.Models;

namespace ShowMe
{
    public partial class App : Application
    {
        static public User User { get; set; }
        
        public App()
        {
            if (!IsLoggedIn)
            {
                InitializeComponent();
                MainPage = new NavigationPage(new LoginPage());
            }
            else
            {
                MainPage = new NavigationPage(new MainPage());
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
