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
        MyShowsViewModel myShowsViewModel;

        public MyShowsPage()
        {
            InitializeComponent();
            BindingContext = myShowsViewModel = new MyShowsViewModel();
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
    }
}