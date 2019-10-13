using System;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using ShowMe.Models;
using ShowMe.ViewModels;
using Xamarin.Forms.Xaml;

namespace ShowMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddShowPopUp : PopupPage
    {
        public class PopUpArgs : EventArgs
        {
            public int SeasonInWatch { get; set; }
            public int EpisodeInWatch { get; set; }
        }
        
        public delegate void PopUpClosedDelegate(object source, PopUpArgs eventArgs);

        public event PopUpClosedDelegate PopUpClosed;

        public AddShowPopUp(ShowDetailsViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }


        private async void OnSave(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
            OnPopUpClosed();
        }

        protected virtual void OnPopUpClosed()
        {
            if (PopUpClosed != null)
            {
                PopUpClosed(this, new PopUpArgs() { SeasonInWatch = EnteredSeason.SelectedIndex + 1, EpisodeInWatch = EnteredEpisode.SelectedIndex +1 });
            };
        }

        public void OnSeasonValueSelected(object sender, EventArgs e)
        {
            //Method called every time Season picker selection is changed.
            EnteredEpisode.ItemsSource = ((Season) EnteredSeason.SelectedItem).EpisodesOfSeason;
        }
    }
}