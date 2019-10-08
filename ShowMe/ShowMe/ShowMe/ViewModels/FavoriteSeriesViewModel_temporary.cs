using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using ShowMe.Models;
using ShowMe.Services;
using ShowMe.Views;
using Xamarin.Forms;

namespace ShowMe.ViewModels
{
    class FavoriteSeriesViewModel_temporary : BaseViewModel
    {
        // TO MODIFY ONCE WE HAVE DATABASE
        public ObservableCollection<Show> Favorites { get; set; } = new ObservableCollection<Show>();
        TvMazeService service = new TvMazeService();
        public FavoriteSeriesViewModel_temporary()
        {
            Title = "Browse Favorites";

            MessagingCenter.Subscribe<ShowDetailsPage, Show>(this, "AddFavorite", (obj, item) =>
            {
                Show newFavorite = item as Show;
                Favorites.Add(newFavorite);
            });
            Task.Run(() => FavoriteList());

        }

        public async Task FavoriteList()
        {
            for (int i = 0; i < 10; i++)
            {
                Show s = await service.GetShowAsync(i);
                if (s != null)
                {
                    Favorites.Add(s);
                }
            }
        }






    }
}
