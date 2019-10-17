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
            BindingContext = ProfileViewModel.User;
        }
    }
}