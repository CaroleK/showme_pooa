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

        /// <summary>
        /// The event called when the user taps on the checkbox in one of the rows of the watchlist.
        /// Turns the checkbox to green, increment last episode watched for this show, and after 1 second, refreshes watchlist.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnCheckBoxTappedGestureRecognizer(object sender, EventArgs e)
        {
            var imageSender = (Image)sender;
            
            // Fill the checkbox in green
            imageSender.Source = "green_checkbox.png";
            // Disable recognition of further tap gestures
            imageSender.GestureRecognizers.Clear();

            // Get the show corresponding to row tapped
            int showId = (int)((TappedEventArgs)e).Parameter;
            MyShow ms = MyShowsCollection.GetByIdFromMyShows(showId);

            // Increment episode
            viewModel.IncrementEpisode(ms);

            // Transition to next episode after 2 seconds
            await Task.Delay(750);
            viewModel.TransitionEpisode(ms);

        }
    }    
}