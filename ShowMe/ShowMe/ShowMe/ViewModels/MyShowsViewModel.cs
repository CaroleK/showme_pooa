using ShowMe.Models;
using ShowMe.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ShowMe.ViewModels
{
    class MyShowsViewModel : BaseViewModel
    {
        // TO MODIFY ONCE WE HAVE DATABASE
        public ObservableCollection<MyShow> ShowsToDisplay { get; set; } = new ObservableCollection<MyShow>();
        public ObservableCollection<string> FilterOptions { get; }

        string selectedFilter = "All";
        public string SelectedFilter
        {
            get => selectedFilter;
            set
            {
                if (SetProperty(ref selectedFilter, value))
                {
                    FilterItems();
                }
            }
        }

        TvMazeService service = new TvMazeService();
        public  MyShowsViewModel() 
        {
            Title = "Browse MyShows";

            Init();

            FilterOptions = new ObservableCollection<string>
                {
                    "All",
                    "Not started",
                    "In progress",
                    "Finished"
                };
        }

        public void ToggleFavorite(bool onlyFavorites)
        {
            ObservableCollection<MyShow> currentShows = new ObservableCollection<MyShow>();
            foreach (MyShow ms in ShowsToDisplay)
            {
                currentShows.Add(ms);
            }
            ShowsToDisplay.Clear();

            if (onlyFavorites)
            {
                foreach (MyShow ms in currentShows)
                {
                    if (ms.IsFavorite)
                    {
                        ShowsToDisplay.Add(ms);
                    }
                }
            }
            else
            {
                FilterItems();
            }
        }

        static public bool AreEpisodeDictionariesEqual(Dictionary<string, int> dic1, Dictionary<string, int> dic2)
        {
            if ((dic1 != null) && (dic2 != null))
            {
                return ((dic1["episode"] == dic2["episode"]) && (dic1["season"] == dic2["season"]));
            }
            else
            {
                return false; 
            }            
        }

        public void FilterItems()
        {
            ShowsToDisplay.Clear();

            var MyShows = MyShowsCollection.Instance;

            switch (selectedFilter)
            {
                case "All":
                    foreach (MyShow ms in MyShows)
                    {
                        ShowsToDisplay.Add(ms);
                    }
                    break;
                case "Not started":
                    foreach (MyShow ms in MyShows)
                    {
                        if (ms.LastEpisodeWatched == null)
                        {
                            ShowsToDisplay.Add(ms);
                        }
                    }
                    break;
                case "Finished":
                    foreach (MyShow ms in MyShows)
                    {
                        if ((ms.LastEpisodeWatched != null) && (AreEpisodeDictionariesEqual(ms.LastEpisodeWatched,ms.LastEpisode)))
                        {
                            ShowsToDisplay.Add(ms);
                        }
                    }
                    break;
                case "In progress":
                    foreach (MyShow ms in MyShows)
                    {
                        if ((ms.LastEpisodeWatched != null) && !(AreEpisodeDictionariesEqual(ms.LastEpisodeWatched, ms.LastEpisode)))
                        {
                            ShowsToDisplay.Add(ms);
                        }
                    }
                    break;
                default:
                    foreach (MyShow ms in MyShows)
                    {
                        ShowsToDisplay.Add(ms);
                    }
                    break;
            }     
            
            //ToggleFavorite()
            
        }

        public void Init()
        {
            ShowsToDisplay.Clear();
            var MyShows = MyShowsCollection.Instance;
            foreach (MyShow ms in MyShows)
            {
                ShowsToDisplay.Add(ms);
            }
        }

    }
}
