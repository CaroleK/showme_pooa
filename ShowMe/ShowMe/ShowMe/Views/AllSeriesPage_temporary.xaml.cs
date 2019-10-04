using System;
using System.ComponentModel;
using System.Threading.Tasks;
using ShowMe.Models;
using ShowMe.ViewModels;
using Xamarin.Forms;

namespace ShowMe.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class AllSeriesPage : ContentPage
    {
        AllSeriesViewModel_temporary viewModel;

        public AllSeriesPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new AllSeriesViewModel_temporary();
        }

        async void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Show selectedItem = e.SelectedItem as Show;
            if (selectedItem == null)
                return;

            await Navigation.PushAsync(new ShowDetailsPage(new ShowDetailsViewModel(selectedItem)));

            // Manually deselect item.
            AllSeriesListView.SelectedItem = null;
        }

        void OnClickLoadMore(object sender, EventArgs e)
        {
            Task.Run(() => viewModel.ExecuteLoadSeriesCommand());
        }



        private void AllSeriesSearch_SearchButtonPressed(object sender, EventArgs e)
        {

            Task.Run(() => viewModel.ExecuteSearchSeriesCommand(AllSeriesSearch.Text));
        }

        private void AllSeriesSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                viewModel.Series.Clear();
                Task.Run(() => viewModel.ExecuteLoadSeriesCommand());
                Btn_LoadMore.IsVisible = true;
            }
        }

        private void AllSeriesSearch_Focused(object sender, FocusEventArgs e)
        {
            Btn_LoadMore.IsVisible = false;
        }

        private void AllSeriesSearch_Unfocused(object sender, FocusEventArgs e)
        {

        }
    }
}