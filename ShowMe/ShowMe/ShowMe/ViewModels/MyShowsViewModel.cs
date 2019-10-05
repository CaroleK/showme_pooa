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
        public ObservableCollection<Show> MyShowsCollection { get; set; } = new ObservableCollection<Show>();
        TvMazeService service = new TvMazeService();

        public MyShowsViewModel()
        {
            MessagingCenter.Subscribe<ShowDetailsPage, Show>(this, "AddToMyShows", (obj, item) =>
            {
                Show newSelectedShow = item as Show;
                MyShowsCollection.Add(newSelectedShow);
            });
        }

       
    }
}
