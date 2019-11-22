using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ShowMe.Models;
using ShowMe.Services;
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

        // Instanciates FirebaseHelper once and for all that listens to relevant messages
        FireBaseHelper MyFireBaseHelper = new FireBaseHelper();

        public MainPage()
        {
            BindingContext = mainPageViewModel = new MainPageViewModel();
            
            InitializeComponent();
            if (!App.IsLoggedIn)
            {
                Navigation.PushModalAsync(new LoginPage());
            }
                         
        }

        // Both properties are useful to make navigation's logic smoother 
        protected Page currentPage = null;
        protected Page lastPage = null;

        /// <summary>
        /// Triggered when user navigates to new tab
        /// Implements base.OnCurrentPageChanged()
        /// Resets the tab the user just exited, in order to make navigation more intuitive 
        /// </summary>
        protected async override void OnCurrentPageChanged()
        {
            // Updates the last visited 
            lastPage = currentPage;

            // CurrentPage is the new tab the user just clicked on 
            currentPage = CurrentPage;

            // Resets the tab the user just exited to its root 
            if (lastPage != null)
            {
                await lastPage.Navigation.PopToRootAsync();
            }

            base.OnCurrentPageChanged();
        }

    }
}
