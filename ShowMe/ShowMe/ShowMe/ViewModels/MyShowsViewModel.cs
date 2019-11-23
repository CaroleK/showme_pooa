using ShowMe.Models;
using ShowMe.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ShowMe.ViewModels
{
    /// <summary>
    /// View Model associated with the MyShows Page
    /// </summary>
    class MyShowsViewModel : BaseViewModel
    {
        // The list of shows binded in the UI
        public ObservableCollection<MyShow> ShowsToDisplay { get; set; } = new ObservableCollection<MyShow>();

        // Filter options
        public ObservableCollection<string> FilterOptions { get; }

        // Favorite options selected or not
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

        // Selected filter
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

        public  MyShowsViewModel() 
        {
            Title = "Browse my shows";

            FilterOptions = new ObservableCollection<string>
                {
                    "All",
                    "Not started",
                    "In progress",
                    "Finished"
                };
        }

        /// <summary>
        /// Called when the property to only show favorite shows is changed
        /// Display shows according to settings
        /// </summary>
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

        /// <summary>
        /// Called when the filter settings is changed
        /// Displays shows according to filter
        /// </summary>
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
                        if ((ms.LastEpisodeWatched != null) && (ms.LastEpisodeWatched.Equals(ms.LastEpisode)))
                        {
                            ShowsToDisplay.Add(ms);
                        }
                    }
                    break;
                case "In progress":
                    foreach (MyShow ms in MyShows)
                    {
                        if ((ms.LastEpisodeWatched != null) && !(ms.LastEpisodeWatched.Equals(ms.LastEpisode)))
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

        /// <summary>
        /// Init is called on page appearance
        /// Clears previous list and displays user's show according to settings
        /// </summary>
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
