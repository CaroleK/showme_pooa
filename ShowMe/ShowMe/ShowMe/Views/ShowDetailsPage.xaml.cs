using System;
using System.Linq;
using ShowMe.Models;
using ShowMe.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Rg.Plugins.Popup.Services;
using static ShowMe.Views.AddShowPopUp;
using ShowMe.Services;

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
        /// and gives the possibility to the user to add the show to his list "MyShows" and then to his favorites.
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
                Btn_Favorite.IsVisible = true;
                if (((MyShow)viewModel.Show).IsFavorite){
                    Btn_Favorite.Source = "red_heart.png";
                }
                Btn_Notification.IsVisible = true;
                if (!((MyShow)viewModel.Show).MustNotify)
                {
                    Btn_Notification.Source = "empty_bell.png";
                }
                Btn_DeleteFromMyShows.IsVisible = true;
                DisplayLastEpisode.IsVisible = true;
                Btn_EditLastEpisodeWatched.IsVisible = true;
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
                //Add standard MyShow object with status "Not started watching" to MyShowsCollection
                myShow = new MyShow(this.viewModel.Show, false, true, null);
                viewModel.AddShowToMyShowsCollection(myShow);
            };
            //Change UI
            Btn_AddToMyShows.IsVisible = false;
            Btn_Favorite.IsVisible = true;
            Btn_Notification.IsVisible = true;
            Btn_DeleteFromMyShows.IsVisible = true;
            DisplayLastEpisode.IsVisible = true;
            Btn_EditLastEpisodeWatched.IsVisible = true;
            
            //ToastMessage
            DependencyService.Get<IMessage>().Show("Show added to your list");
        }

        /// <summary>
        /// Actions to do after user closed AddShowPopUp where he specified where he stopped watching show
        /// </summary>
        private void AddShowPopUpClosed(object sender, PopUpArgs e)
        {
            //Add personnalized MyShow object to MyShowsCollection
            myShow = new MyShow(this.viewModel.Show, false, true, new EpisodeSeason(e.EpisodeInWatch, e.SeasonInWatch));
            viewModel.AddShowToMyShowsCollection(myShow);

            //Unsubscribe to event
            _addShowPopUpPage.PopUpClosed -= AddShowPopUpClosed;
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
        /// In result it will either Add or Remove show from favorites
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
                DependencyService.Get<IMessage>().Show("Show removed from your favorites");
            }
            else {
                imageSender.Source = "red_heart.png";
                viewModel.AddShowToFavorites(myShow);
                DependencyService.Get<IMessage>().Show("Show added to your favorites");
            };
        }

        /// <summary>
        /// Actions to do when Bell Icon is tapped
        /// In result, it will set MustNotify attribute of MyShow to true or false
        /// </summary>
        private void OnBellTappedGestureRecognizer(object sender, EventArgs args)
        {
            //Retrieve myShow object in MyShowsCollection
            MyShow myShow = MyShowsCollection.Instance.FirstOrDefault(x => x.Id == this.viewModel.Show.Id);

            var imageSender = (Image)sender;

            if (myShow.MustNotify)
            {
                imageSender.Source = "empty_bell.png";
                DependencyService.Get<IMessage>().Show("No more alerts for this show!");
            }
            else
            {
                imageSender.Source = "full_bell.png";
                DependencyService.Get<IMessage>().Show("You will receive alerts for this show");
            };
            viewModel.ChangeNotifyValue(myShow);
        }

        /// <summary>
        /// Back-code event triggered when user clicks on "Delete from MyShows"
        /// </summary>
        private async void OnGarbageTappedGestureRecognizer(object sender, EventArgs e)
        {
            bool userWantsToDelete = await DisplayAlert("Delete", "Are you sure?", "Yes", "No");

            if (userWantsToDelete)
            {
                DependencyService.Get<IMessage>().Show("Show was deleted from your list");

                MyShow myShowToDelete = MyShowsCollection.Instance.First(item => item.Id == this.viewModel.Show.Id);
                viewModel.DeleteShowFromMyShowsCollection(myShowToDelete);

                //Adapt UI
                Btn_AddToMyShows.IsVisible = true;
                Btn_Favorite.IsVisible = false;
                Btn_Notification.IsVisible = false;
                Btn_DeleteFromMyShows.IsVisible = false;
                DisplayLastEpisode.IsVisible = false;
                Btn_EditLastEpisodeWatched.IsVisible = false;
            }
        }

        private async void OnPencilTappedGestureRecognizer(object sender, EventArgs e)
        {
            _addShowPopUpPage = new AddShowPopUp(this.viewModel);
            //Subscribe to event AddShowPopUpClosed before pushing addShowPopUpPage
            _addShowPopUpPage.PopUpClosed += ChangeLastWatchedPopUpClosed;
            await PopupNavigation.Instance.PushAsync(_addShowPopUpPage);
        }

        private void ChangeLastWatchedPopUpClosed(object sender, PopUpArgs e)
        {
            viewModel.modifyMyShow(e.EpisodeInWatch, e.SeasonInWatch);

            //Unsubscribe to event
            _addShowPopUpPage.PopUpClosed -= ChangeLastWatchedPopUpClosed;

        }
    }
}