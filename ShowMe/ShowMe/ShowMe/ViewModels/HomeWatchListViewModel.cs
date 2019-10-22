using ShowMe.Models;
using ShowMe.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ShowMe.ViewModels
{
    public class HomeWatchListViewModel : ContentPage
    {
        public TvMazeService service = new TvMazeService();
        public FireBaseHelper MyFireBaseHelper = new FireBaseHelper(); 
        public ObservableCollection<MyShow> ShowsToDisplay { get; set; } = new ObservableCollection<MyShow>();
        public HomeWatchListViewModel()
        {
        }

        public void Init()
        {
            ShowsToDisplay.Clear();
            var MyShows = MyShowsCollection.Instance;

            // Display only shows in progress
            foreach (MyShow ms in MyShows)
            {
                if (!(Show.AreEpisodeDictionariesEqual(ms.LastEpisodeWatched, ms.LastEpisode)))
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
            Dictionary<string, int> newLEW = myShow.NextEpisode();
            myShow.LastEpisodeWatched = newLEW;
            MyShowsCollection.ModifyShowInMyShows(myShow);
            MessagingCenter.Send<HomeWatchListViewModel, MyShow>(this, "IncrementEpisode", myShow);
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
                DependencyService.Get<IMessage>().Show("You finisehd \"" + ms.Title + "\" ! You can still find it if your list of shows."  );
            }
            
        }
    }
}