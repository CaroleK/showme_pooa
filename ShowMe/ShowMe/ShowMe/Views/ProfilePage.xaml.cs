using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShowMe.Models;
using ShowMe.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShowMe.Views
{

    public partial class ProfilePage : ContentPage
    {
        ProfileViewModel viewModel;

        public ProfilePage()
        {
            InitializeComponent();
            this.BindingContext = viewModel = new ProfileViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.Init();
        }

        private void OnLogOutClicked(object sender, EventArgs e)
        {
            App.IsLoggedIn = false;
            App.Current.MainPage = new NavigationPage(new LoginPage());
        }

    }
}