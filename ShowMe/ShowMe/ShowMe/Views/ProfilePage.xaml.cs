using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShowMe.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShowMe.Views
{

    public partial class ProfilePage : ContentPage
    {
        ProfileViewModel viewModel = new ProfileViewModel();

        public ProfilePage()
        {
            InitializeComponent();
            BindingContext = App.User;
            viewModel.DisplayStatistics(App.User);
        }

        private void OnLogOutClicked(object sender, EventArgs e)
        {
            App.IsLoggedIn = false;
            App.Current.MainPage = new NavigationPage(new LoginPage());
        }

    }
}