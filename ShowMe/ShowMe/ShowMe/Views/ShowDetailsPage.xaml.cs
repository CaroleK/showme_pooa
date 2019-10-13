using System;
using System.Collections.Generic;
using System.Linq;
using ShowMe.Models;
using ShowMe.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Rg.Plugins.Popup.Services;
using static ShowMe.Views.AddShowPopUp;


namespace ShowMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShowDetailsPage : ContentPage
    {
        ShowDetailsViewModel viewModel;
        MyShow myShow = null;
        AddShowPopUp _modalPage;

        public ShowDetailsPage(ShowDetailsViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = this.viewModel = viewModel;
         
            if (viewModel.Show is MyShow)
            {
                Btn_AddToMyShows.IsVisible = false;
                Btn_AddToFavorite.IsVisible = true;
                if (((MyShow)viewModel.Show).IsFavorite){
                    Btn_AddToFavorite.Source = "red_heart.png";
                }
                Btn_DeleteFromMyShows.IsVisible = true;
            }
        }

        async void OnClickAddToMyShows(object sender, EventArgs e)
        {
         
            bool userStartedWatchingShow = await DisplayAlert("Show added to your list!", "Did you start watching this show?", "Yes", "No");

            _modalPage = new AddShowPopUp(this.viewModel); 
            if (userStartedWatchingShow)
            {
                _modalPage.PopUpClosed += AddShowPopUpClosed;
                await PopupNavigation.Instance.PushAsync(_modalPage);
            }
            else {
                myShow = new MyShow(this.viewModel.Show, false, true, null);
                viewModel.AddShowToMyShowsCollection(myShow);
                Btn_AddToMyShows.IsVisible = false;
                Btn_AddToFavorite.IsVisible = true;
                Btn_DeleteFromMyShows.IsVisible = true;
            };
        }

        private void AddShowPopUpClosed(object sender, PopUpArgs e)
        {
            myShow = new MyShow(this.viewModel.Show, false, true, new Dictionary<string, int> { { "episode", e.EpisodeInWatch }, { "season", e.SeasonInWatch } });
            viewModel.AddShowToMyShowsCollection(myShow);
            Btn_AddToMyShows.IsVisible = false;
            Btn_AddToFavorite.IsVisible = true;
            Btn_DeleteFromMyShows.IsVisible = true;
            _modalPage.PopUpClosed -= AddShowPopUpClosed;

        }

        async void OnClickDeleteFromMyShows(object sender, EventArgs e)
        {
            await DisplayAlert("Show deleted", "The show was successfully deleted from your list!", "Ok");

            MyShow myShowToDelete = MyShowsCollection.Instance.First(item => item.Id == this.viewModel.Show.Id);
            viewModel.DeleteShowFromMyShowsCollection(myShowToDelete);
            Btn_AddToMyShows.IsVisible = true;
            Btn_AddToFavorite.IsVisible = false;
            Btn_DeleteFromMyShows.IsVisible = false;
        }

        private void OnAboutClicked(object sender, EventArgs e)
        {
            AboutTab.IsVisible = true;
            EpisodesTab.IsVisible = false;
            AboutButton.TextColor = Color.Salmon;
            EpisodesButton.TextColor = Color.Gray;
        }

        private void OnEpisodesClicked(object sender, EventArgs e)
        {
            AboutTab.IsVisible = false;
            EpisodesTab.IsVisible = true;
            AboutButton.TextColor = Color.Gray;
            EpisodesButton.TextColor = Color.Salmon;
        }

        private void OnHeartTappedGestureRecognizer(object sender, EventArgs args)
        {
            var imageSender = (Image)sender;
            imageSender.Source = "red_heart.png";
            viewModel.AddShowToFavorites(this.viewModel.Show);

        }

        public Command TapCommand {
            get
            {
                return new Command(isFavoriteBool => {
                    if ((bool) isFavoriteBool) {
                        Btn_AddToFavorite.Source = "red_heart.png";
                        viewModel.AddShowToFavorites(this.viewModel.Show);
                    }
                });
            }
        }
    }
}