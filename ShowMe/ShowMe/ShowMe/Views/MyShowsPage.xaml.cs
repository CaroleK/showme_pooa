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

        async void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Show selectedItem = e.SelectedItem as Show;
            if (selectedItem == null)
            {
                return;
            }
            await Navigation.PushAsync(new ShowDetailsPage(new ShowDetailsViewModel(selectedItem)));

            // Manually deselecy item
            MyShowsListView.SelectedItem = null;
        }

        private void ShowPicker_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.Init();
        }
    }
}