using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShowMe.Models;
using ShowMe.ViewModels;
using Xamarin.Forms;

namespace ShowMe.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : TabbedPage
    {
        MainPageViewModel mainPageViewModel;
        public MainPage(User user)
        {
            BindingContext = mainPageViewModel = new MainPageViewModel(user);
            bool test = App.IsLoggedIn; 
            InitializeComponent();
            if (!App.IsLoggedIn)
            {
                Navigation.PushModalAsync(new LoginPage());
            }
                         
        }

      
    }
}
