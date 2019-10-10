using ShowMe.Models;
using ShowMe.Services;
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
                        if (ms.LastEpisodeWatched.Equals(ms.LastEpisode))
                        {
                            ShowsToDisplay.Add(ms);
                        }
                    }
                    break;
                case "In Progress":
                    foreach (MyShow ms in MyShows)
                    {
                        if ((ms.LastEpisodeWatched != null) &&(ms.LastEpisodeWatched.Equals(ms.LastEpisode)))
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
