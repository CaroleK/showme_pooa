using System;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using ShowMe.Models;
using ShowMe.ViewModels;
using Xamarin.Forms;
using System.Threading;
using Xamarin.Forms.Xaml;
using System.Threading.Tasks;

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
            if (EnteredSeason.SelectedIndex == -1 | EnteredEpisode.SelectedIndex == -1)
            {
                DependencyService.Get<IMessage>().Show("Please select both Season and Episode");
            }
            else
            {
                await PopupNavigation.Instance.PopAsync();

                Task.Run(() => OnPopUpClosed()).Wait();
            }
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
            if (!(EnteredSeason.SelectedItem == null))
            {
                EnteredEpisode.ItemsSource = ((Season)EnteredSeason.SelectedItem).EpisodesOfSeason;
            }
            
        }
    }
}