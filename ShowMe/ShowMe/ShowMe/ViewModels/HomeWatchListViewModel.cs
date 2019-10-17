using ShowMe.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace ShowMe.ViewModels
{
    public class HomeWatchListViewModel : ContentPage
    {
        public ObservableCollection<MyShow> ShowsToDisplay { get; set; } = new ObservableCollection<MyShow>();
        public HomeWatchListViewModel()
        {
            Init();
        }

        public void Init()
        {
            ShowsToDisplay.Clear();
            var MyShows = MyShowsCollection.Instance;

            // Display only shows in progress
            foreach (MyShow ms in MyShows)
            {
                if ((ms.LastEpisodeWatched != null) && !(Show.AreEpisodeDictionariesEqual(ms.LastEpisodeWatched, ms.LastEpisode)))
                {
                    ShowsToDisplay.Add(ms);
                }
            }
        }
    }
}