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
        public ObservableCollection<Show> MyShows { get; set; } = new ObservableCollection<Show>();
        TvMazeService service = new TvMazeService();
        public MyShowsViewModel()
        {
            Title = "Browse Favorites";

            MessagingCenter.Subscribe<ShowDetailsPage, Show>(this, "AddFavorite", (obj, item) =>
            {
                Show newFavorite = item as Show;
                MyShows.Add(newFavorite);
            });
            Task.Run(() => MyShowsList());
    }
}
