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
            await fireBaseHelper.AddShowToUserList(ShowDetailsViewModel.user.Id, this.viewModel.Show);
            bool userStartedWatchingShow = await DisplayAlert("Show added to your list!", "Did you start watching this show?", "Yes", "No");
            
            if (userStartedWatchingShow)
            {
                await PopupNavigation.Instance.PushAsync(new AddShowPopUp());
            }
            MessagingCenter.Send(this, "AddFavorite", viewModel.Show);
        }
    }
}