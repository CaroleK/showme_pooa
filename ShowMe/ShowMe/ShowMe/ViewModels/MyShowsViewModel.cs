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

        bool onlyFavorites;
        public bool OnlyFavorites
        {
            get
            {
                return onlyFavorites;
            }
            set
            {
                onlyFavorites = value;
                ToggleFavorite();
            }
        }

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

        public void ToggleFavorite()
        {       
            if (OnlyFavorites)
            {
                // Keep all the relevant shows (depending on filter)
                ObservableCollection<MyShow> currentShows = new ObservableCollection<MyShow>();
                foreach (MyShow ms in ShowsToDisplay)
                {
                    currentShows.Add(ms);
                }

                // Reset displayed shows
                ShowsToDisplay.Clear(); 


                // Among relevant shows, display only favorites
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
                // Display all relevant shows according to filter
                FilterItems();
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
                        if ((ms.LastEpisodeWatched != null) && (Show.AreEpisodeDictionariesEqual(ms.LastEpisodeWatched,ms.LastEpisode)))
                        {
                            ShowsToDisplay.Add(ms);
                        }
                    }
                    break;
                case "In progress":
                    foreach (MyShow ms in MyShows)
                    {
                        if ((ms.LastEpisodeWatched != null) && !(Show.AreEpisodeDictionariesEqual(ms.LastEpisodeWatched, ms.LastEpisode)))
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

            // we have the correct shows regarding the watched episodes, now adapt in case user only wants favorites
            if (OnlyFavorites)
            {
                ToggleFavorite();
            }          
        }

        public void Init()
        {
            ShowsToDisplay.Clear();
            var MyShows = MyShowsCollection.Instance;
            foreach (MyShow ms in MyShows)
            {
                ShowsToDisplay.Add(ms);
            }
            
            FilterItems();
        }

    }
}
