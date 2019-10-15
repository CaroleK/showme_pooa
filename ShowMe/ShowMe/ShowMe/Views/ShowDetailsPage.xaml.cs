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
        AddShowPopUp _addShowPopUpPage;

        /// <summary>
        /// UI Page that shows to the user all the details of a Show (in 2 tabs called About and Episodes) 
        /// and the gives the possibility to the user to add the show to his list "MyShows" and then to his favorites.
        /// </summary>
        /// <param name="viewModel">ShowDetailsViewModel associated with this View </param>
        public ShowDetailsPage(ShowDetailsViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = this.viewModel = viewModel;
         
            if (viewModel.Show is MyShow)
            {
                // create first state of UI elements that match the status of the show (in MyShows or not, favorite or not) when View first clicked
                Btn_AddToMyShows.IsVisible = false;
                Btn_AddToFavorite.IsVisible = true;
                if (((MyShow)viewModel.Show).IsFavorite){
                    Btn_AddToFavorite.Source = "red_heart.png";
                }
                Btn_DeleteFromMyShows.IsVisible = true;
            }
        }

        /// <summary>
        /// Back-code event triggered when user clicks on "Add to MyShows" button
        /// </summary>
        async void OnClickAddToMyShows(object sender, EventArgs e)
        {
            //First pop-up
            bool userStartedWatchingShow = await DisplayAlert("Show added to your list!", "Did you start watching this show?", "Yes", "No");
            
            if (userStartedWatchingShow)
            {
                _addShowPopUpPage = new AddShowPopUp(this.viewModel);
                //Subscribe to event AddShowPopUpClosed before pushing addShowPopUpPage
                _addShowPopUpPage.PopUpClosed += AddShowPopUpClosed;
                await PopupNavigation.Instance.PushAsync(_addShowPopUpPage);
            }
            else {
                //Add stantard MyShow object with status "Not started watching" to MyShowsCollection
                myShow = new MyShow(this.viewModel.Show, false, true, null);
                viewModel.AddShowToMyShowsCollection(myShow);
                
                //Adapt UI
                Btn_AddToMyShows.IsVisible = false;
                Btn_AddToFavorite.IsVisible = true;
                Btn_DeleteFromMyShows.IsVisible = true;
            };
        }

        /// <summary>
        /// Actions to do after user closed AddShowPopUp where he specified where he stopped watching show
        /// </summary>
        private void AddShowPopUpClosed(object sender, PopUpArgs e)
        {
            //Add personnalized MyShow object to MyShowsCollection
            myShow = new MyShow(this.viewModel.Show, false, true, new Dictionary<string, int> { { "episode", e.EpisodeInWatch }, { "season", e.SeasonInWatch } });
            viewModel.AddShowToMyShowsCollection(myShow);
           
            //Adapt UI
            Btn_AddToMyShows.IsVisible = false;
            Btn_AddToFavorite.IsVisible = true;
            Btn_DeleteFromMyShows.IsVisible = true;

            //Unsubscribe to event
            _addShowPopUpPage.PopUpClosed -= AddShowPopUpClosed;

        }

        /// <summary>
        /// Back-code event triggered when user clicks on "Delete from MyShows"
        /// </summary>
        async void OnClickDeleteFromMyShows(object sender, EventArgs e)
        {
            bool userWantsToDelete = await DisplayAlert("Delete?", "Are you sure?", "Yes", "No");

            if (userWantsToDelete)
            {
                await DisplayAlert("Show deleted", "The show was successfully deleted from your list!", "Ok");

                MyShow myShowToDelete = MyShowsCollection.Instance.First(item => item.Id == this.viewModel.Show.Id);
                viewModel.DeleteShowFromMyShowsCollection(myShowToDelete);

                //Adapt UI
                Btn_AddToMyShows.IsVisible = true;
                Btn_AddToFavorite.IsVisible = false;
                Btn_DeleteFromMyShows.IsVisible = false;
            }
        }

        /// <summary>
        /// Make "About" tab appear when "About" button is clicked
        /// "About" tab contains the summary of the show, its cast, etc.
        /// </summary>
        private void OnAboutClicked(object sender, EventArgs e)
        {
            AboutTab.IsVisible = true;
            EpisodesTab.IsVisible = false;
            AboutButton.TextColor = Color.Salmon;
            EpisodesButton.TextColor = Color.Gray;
        }

        /// <summary>
        /// Make "Episodes" tab appear when "Episodes" button is clicked
        /// "Episodes" tab contains all the names and images of the episodes of the show
        /// </summary>
        private void OnEpisodesClicked(object sender, EventArgs e)
        {
            AboutTab.IsVisible = false;
            EpisodesTab.IsVisible = true;
            AboutButton.TextColor = Color.Gray;
            EpisodesButton.TextColor = Color.Salmon;
        }

        /// <summary>
        /// Actions to do when Heart icon is tapped
        /// In result it will Add or Remove show from favorites
        /// </summary>
        private void OnHeartTappedGestureRecognizer(object sender, EventArgs args)
        {
            //Retrieve myShow object in MyShowsCollection
            MyShow myShow = MyShowsCollection.Instance.FirstOrDefault(x => x.Id == this.viewModel.Show.Id);
            
            var imageSender = (Image)sender;
            
            if (myShow.IsFavorite)
            {
                imageSender.Source = "empty_heart.png";
                viewModel.RemoveShowFromFavorites(myShow);
            }
            else {
                imageSender.Source = "red_heart.png";
                viewModel.AddShowToFavorites(myShow);
            };
        }
    }
}