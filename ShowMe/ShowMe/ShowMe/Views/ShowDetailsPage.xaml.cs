using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShowMe.Models;
using ShowMe.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ShowMe.Services;
using Rg.Plugins.Popup.Services;

namespace ShowMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShowDetailsPage : ContentPage
    {
        ShowDetailsViewModel viewModel;
        FireBaseHelper fireBaseHelper = new FireBaseHelper();

        public ShowDetailsPage(ShowDetailsViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = this.viewModel = viewModel;
            if (viewModel.Show is MyShow)
            {
                Btn_AddToMyShows.IsVisible = false;
                Btn_AddToFavorite.IsVisible = true;
                Btn_DeleteFromMyShows.IsVisible = true;
            }
        }

        async void OnClickAddToMyShows(object sender, EventArgs e)
        {
            bool userStartedWatchingShow = await DisplayAlert("Show added to your list!", "Did you start watching this show?", "Yes", "No");
            
            if (userStartedWatchingShow)
            {
                await PopupNavigation.Instance.PushAsync(new AddShowPopUp());
            }

            viewModel.AddShowToMyShowsCollection(this.viewModel.Show);
            Btn_AddToMyShows.IsVisible = false;
            Btn_AddToFavorite.IsVisible = true;
            Btn_DeleteFromMyShows.IsVisible = true;
        }

        async void OnClickDeleteFromMyShows(object sender, EventArgs e)
        {
            await DisplayAlert("Show deleted", "The show was successfully deleted from your list!", "Ok");

            MyShow myShowToDelete = this.viewModel.Show as MyShow;
            viewModel.DeleteShowFromMyShowsCollection(myShowToDelete);
            Btn_AddToMyShows.IsVisible = true;
            Btn_AddToFavorite.IsVisible = false;
            Btn_DeleteFromMyShows.IsVisible = false;
        }

        private void OnAboutClicked(object sender, EventArgs e)
        {
            AboutTab.IsVisible = true;
            EpisodesTab.IsVisible = false;
        }

        private void OnEpisodesClicked(object sender, EventArgs e)
        {
            AboutTab.IsVisible = false;
            EpisodesTab.IsVisible = true;
        }

        private void OnHeartTappedGestureRecognizer(object sender, EventArgs args)
        {
            var imageSender = (Image)sender;
            imageSender.Source = "red_heart.png";
            viewModel.AddShowToFavorites(this.viewModel.Show);

        }
    }
}