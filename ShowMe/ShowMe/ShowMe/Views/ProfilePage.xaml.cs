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
        ProfileViewModel viewModel = new ProfileViewModel();
        User user = App.User;
        public ProfilePage()
        {
            InitializeComponent();
            BindingContext = App.User;
            user.TotalMinutesWatched = App.User.TotalMinutesWatched;
            user.TotalNbrEpisodesWatched = App.User.TotalNbrEpisodesWatched;
            viewModel.DisplayStatistics(App.User);

        }

        private void OnLogOutClicked(object sender, EventArgs e)
        {
            App.IsLoggedIn = false;
            App.Current.MainPage = new NavigationPage(new LoginPage());
        }

    }
}