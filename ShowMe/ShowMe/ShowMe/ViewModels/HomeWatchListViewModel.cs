using ShowMe.Models;
using ShowMe.Services;
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
        public TvMazeService service = new TvMazeService();
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

        public bool IncrementEpisode(MyShow myShow)
        {
            Dictionary<string, int> currentLEW = myShow.LastEpisodeWatched;
            if (Show.AreEpisodeDictionariesEqual(currentLEW, myShow.LastEpisode))
            {
                return false;
            }
            else
            {
                Dictionary<string, int> newLEW = new Dictionary<string, int> { { "episode", 1 }, { "season", 1 } }; 
                List<Season> seasonsList = service.GetSeasonsListAsync(myShow.Id).Result;
                if (seasonsList == null)
                {
                    return false; 
                }
                if (currentLEW["episode"] == Season.FindSeasonBySeasonNumber(seasonsList, currentLEW["season"]).NumberOfEpisodes)
                {
                    newLEW["season"] = currentLEW["season"] + 1;
                }
                else
                {
                    newLEW["season"] = currentLEW["season"];
                    newLEW["episode"] = currentLEW["episode"] + 1;
                }
                myShow.LastEpisodeWatched = newLEW;
                return true; 
            }
        }
    }
}