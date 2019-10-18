using ShowMe.Models;
using ShowMe.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShowMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomeWatchListPage : ContentPage
    {
        HomeWatchListViewModel viewModel; 
        public HomeWatchListPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new HomeWatchListViewModel();
        }

        /// <summary>
        /// Overrides the base OnAppearing method to be sure to refresh the list of shows to display each time the user makes the page appear
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.Init();
        }

        private void OnCheckBoxTappedGestureRecognizer(object sender, EventArgs e)
        {
            var imageSender = (Image)sender;
            imageSender.Source = "green_checkbox.png";

            int showId = (int) ((TappedEventArgs)e).Parameter;
            MyShow ms = MyShowsCollection.GetByIdFromMyShows(showId);

            //Thread.Sleep(1000); 
            viewModel.IncrementEpisode(ms);

        }
    }    
}