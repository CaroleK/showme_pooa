using ShowMe.Models;
using ShowMe.Services;
using ShowMe.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
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

        public async Task MyShowsList()
        {
            for (int i = 0; i < 10; i++)
            {
                Show s = await service.GetShowAsync("https://api.tvmaze.com/shows/" + i);
                if (s != null)
                {
                    MyShows.Add(s);
                }
            }
        }






    }
}
