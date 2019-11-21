using ShowMe.Models;
using ShowMe.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ShowMe.ViewModels
{
    public class HomeWatchListViewModel : BaseViewModel
    {
        public TvMazeService service = new TvMazeService();
        public ObservableCollection<MyShow> ShowsToDisplay { get; set; } = new ObservableCollection<MyShow>();

        private bool _isEmptyShowsToDisplay;

        public bool isEmptyShowsToDisplay { get {return _isEmptyShowsToDisplay; } set {
                _isEmptyShowsToDisplay = value;  OnPropertyChanged(); }
            } 

        public HomeWatchListViewModel()
        {
            ShowsToDisplay.CollectionChanged += OnShowsToDisplayChanged;
        }

        private void OnShowsToDisplayChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            isEmptyShowsToDisplay = (ShowsToDisplay.Count() > 0) ? false : true;
        }

        /// <summary>
        /// Init is called on page appearance
        /// Clears previous list and display shows in progress
        /// </summary>
        public void Init()
        {
            ShowsToDisplay.Clear();
            var MyShows = MyShowsCollection.Instance;

            // Display only shows in progress
            foreach (MyShow ms in MyShows)
            {
                if (ms.LastEpisodeWatched == null)
                {
                    ShowsToDisplay.Add(ms);
                }
                else if ((ms.LastEpisodeWatched != null) && !(ms.LastEpisodeWatched.Equals(ms.LastEpisode)))
                {
                        ShowsToDisplay.Add(ms);
                }
            }
        }        

        /// <summary>
        /// Deals with the logic when an episode has been watched
        /// </summary>
        /// <param name="myShow">The MyShow whose episode has been watched</param>
        public void IncrementEpisode(MyShow myShow)
        {
            EpisodeSeason newLEW = myShow.NextEpisode();
            myShow.LastEpisodeWatched = newLEW;
            MyShowsCollection.ModifyShowInMyShows(myShow);

            //Inform FireBase and App.User for stats
            MessagingCenter.Send<BaseViewModel, MyShow>(this, "UpdateMyShow", myShow);
            
            //TODO: make messaging center work
            App.User.TotalNbrEpisodesWatched += 1;
            App.User.TotalMinutesWatched += newLEW.Duration;
            Task.Run(()=> FireBaseHelper.UpdateUser(App.User.Id, App.User));
            //MessagingCenter.Send<BaseViewModel, MyShow>(this, "IncrementOneEpisode", myShow);
        }

        /// <summary>
        /// Updates the watch list after an episode has been watched
        /// </summary>
        /// <param name="ms">The MyShow whose episode has been watched</param>
        public void TransitionEpisode(MyShow ms)
        {
            int index = ShowsToDisplay.IndexOf(ms);
            ShowsToDisplay.Remove(ms);
            if (ms.NextEpisode() != null)
            {
                ShowsToDisplay.Insert(index, ms);
            }
            // If next episode is empty, the user finished watching the show
            else
            {
                DependencyService.Get<IMessage>().Show("You finished \"" + ms.Title + "\" ! You can still find it if your list of shows."  );
            }
            
        }
    }
}