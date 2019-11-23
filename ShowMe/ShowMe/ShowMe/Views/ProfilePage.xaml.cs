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

        /// <summary>
        /// When page appears, be sure to init correctly
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.Init();
        }

        /// <summary>
        /// Actions to take when user clicks on log out button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLogOutClicked(object sender, EventArgs e)
        {
            App.IsLoggedIn = false;
            App.Current.MainPage = new NavigationPage(new LoginPage());
        }

    }
}