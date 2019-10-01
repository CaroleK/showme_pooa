using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using ShowMe.Models;
using ShowMe.Views;
using Xamarin.Forms;

namespace ShowMe.ViewModels 
{
    class FavoriteSeriesViewModel_temporary : BaseViewModel
    {
        public ObservableCollection<Show> Favorites { get; set; } = new ObservableCollection<Show>();

        public FavoriteSeriesViewModel_temporary()
        {
            Title = "Browse Favorites";

            MessagingCenter.Subscribe<SerieDetailsPage, Show>(this, "AddFavorite", (obj, item) =>
            {
                Show newFavorite = item as Show;
                Favorites.Add(newFavorite);
            });
        }
    }
}
