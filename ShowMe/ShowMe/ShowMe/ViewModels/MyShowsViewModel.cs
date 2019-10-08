using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using ShowMe.Views;
using ShowMe.Models;
using ShowMe.Services;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ShowMe.ViewModels
{
    class MyShowsViewModel : BaseViewModel
    {
        // TO MODIFY ONCE WE HAVE DATABASE
        public ObservableCollection<MyShow> ShowsToDisplay { get; set; } = new ObservableCollection<MyShow>();

        TvMazeService service = new TvMazeService();
        public  MyShowsViewModel()
        {
            Title = "Browse Favorites";

            MessagingCenter.Subscribe<ShowDetailsPage, MyShow>(this, "AddToMyShows", (obj, item) =>
            {               
                MyShows.Add(item);
                ShowsToDisplay.Add(item);
            });
        }

        

    }
}
