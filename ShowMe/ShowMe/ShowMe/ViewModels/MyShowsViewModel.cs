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
            // TODO
            ShowsToDisplay.Clear();
            foreach (MyShow ms in MyShowsCollection.Instance)
            {
                ShowsToDisplay.Add(ms);
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
