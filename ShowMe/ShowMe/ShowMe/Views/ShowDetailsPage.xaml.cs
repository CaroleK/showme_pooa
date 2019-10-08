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
        }

        async void OnClickAddToMyShows(object sender, EventArgs e)
        {
            bool userStartedWatchingShow = await DisplayAlert("Show added to your list!", "Did you start watching this show?", "Yes", "No");
            
            if (userStartedWatchingShow)
            {
                await PopupNavigation.Instance.PushAsync(new AddShowPopUp());
            }

            //TODO : get last episode from popups, for now default values
            MyShow myShow = new MyShow(this.viewModel.Show, false, true, new Episode(1, 1, this.viewModel.Show.Id));
            await FireBaseHelper.AddShowToUserList(ShowDetailsViewModel.user.Id, myShow);

            MessagingCenter.Send<ShowDetailsPage, MyShow>(this, "AddToMyShows", myShow);
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

        }
    }
}