using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShowMe.ViewModels;
using ShowMe.Models;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShowMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyShowsPage : ContentPage
    {
        MyShowsViewModel viewModel;

        public MyShowsPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new MyShowsViewModel();
        }

        /// <summary>
        /// Defines what happens when user clicks on one of the shows in the list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Show selectedItem = e.SelectedItem as Show;
            if (selectedItem == null)
            {
                return;
            }
            await Navigation.PushAsync(new ShowDetailsPage(new ShowDetailsViewModel(selectedItem)));

            // Manually deselect item
            MyShowsListView.SelectedItem = null;
        }

        /// <summary>
        /// Overrides the base OnAppearing method to be sure to refresh the list of shows to display each time the user makes the page appear
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.Init();
        }
    }
}