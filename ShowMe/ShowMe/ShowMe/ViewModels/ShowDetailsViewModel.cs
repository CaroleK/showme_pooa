using System;
using System.Collections.Generic;
using System.Text;
using ShowMe.Services;
using ShowMe.Models;
using ShowMe.Views;
using System.Linq;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Threading;

namespace ShowMe.ViewModels
{
    /// <summary>
    /// View Model associated with the ShowDetail Page
    /// </summary>
    public class ShowDetailsViewModel : BaseViewModel
    {
        // TV maze service
        public TvMazeService service = new TvMazeService();

        // Show 
        private Show _show { get; set; }
        public Show Show {
            get { return _show; }
            set { _show = value; OnPropertyChanged(); }
        }

        // Selected season
        public Season SelectedSeason { get; set; }

        public ShowDetailsViewModel(Show show = null) : base()
        {
            var showsCollection = MyShowsCollection.Instance;
            //If show is in MyShowsCollection, display this object
            if (showsCollection.Any(e => e.Id == show.Id))
            {
                MyShow myShow = MyShowsCollection.Instance.First(e => e.Id == show.Id);
                Title = myShow?.Title;
                Show = myShow;
            }
            else
            {
                Title = show?.Title;
                Show = show;
                Task.Run(() => LoadEpisodesAndSeasons()).Wait();
                Task.Run(() => LoadActors()).Wait();
            }

        }


        /// <summary>
        /// Loads all episodes and seasons for this show
        /// And stores them in Show object
        /// </summary>
        /// <returns>Void task</returns>
        public async Task LoadEpisodesAndSeasons()
        {
            Tuple<List<Episode>, List<Season>> tuple = await service.GetEpisodesAndSeasonsListAsync(Show.Id);
            List<Episode> episodesList = tuple.Item1;
            List<Season> seasonsList = tuple.Item2;

            if (episodesList != null)
            {
                this.Show.EpisodesList = episodesList;
            }

            if (seasonsList != null)
            {
                this.Show.SeasonsList = seasonsList;
                // Add relevant episodes to the season's episodes list
                foreach (Season s in seasonsList)
                {
                    s.EpisodesOfSeason = episodesList.Where(e => e.Season == s.Number).ToList();
                }
            }
        }

        /// <summary>
        /// Loads all actors for this show
        /// And stores them in Show object
        /// </summary>
        /// <returns>Void task</returns>
        public async Task LoadActors()
        {
            List<Actor> ActorsList = await service.GetCastAsync(Show.Id);
            if (ActorsList != null)
            {
                this.Show.Cast = ActorsList;
            }
        }

        /// <summary>
        /// Called when the user chooses to add this show to his/her collection
        /// Retrieves and stores the last episode of this show before adding it to the collection
        /// </summary>
        /// <param name="myShowToAdd">The show to add</param>
        public void AddShowToMyShowsCollection(MyShow myShowToAdd)
        {
            // Find the last episode for this show, now that we're adding it to MyShows list we'll need this attribute
            // First find the max season with episodes in it
            Season maxSeason = null; 
            foreach (Season season in this.Show.SeasonsList)
            {
                if ((maxSeason == null) || (season.Number > maxSeason.Number))
                {
                    if ((season.EpisodesOfSeason != null) && (season.EpisodesOfSeason.Count > 0))
                    {
                        maxSeason = season;
                    }                    
                }
            }

            // Then find max episode in max season
            EpisodeSeason lastEpisode = new EpisodeSeason(maxSeason.EpisodesOfSeason.Max(e => e.Number), maxSeason.Number);
            myShowToAdd.LastEpisode = lastEpisode; 
            
            // Update Show attribute to update UI with OnPropertyChanged
            this.Show = myShowToAdd;

            //Send message to FireBase and App.User for stats
            MessagingCenter.Send<BaseViewModel, MyShow>(this, "AddToMyShows", myShowToAdd);

            MyShowsCollection.AddToMyShows(myShowToAdd);
        }

        /// <summary>
        /// Called when user chooses to delete this show from his/her collection
        /// </summary>
        /// <param name="myShowToDelete">The show to delete</param>
        public void DeleteShowFromMyShowsCollection(MyShow myShowToDelete)
        {
            //Send message to FireBase and App.User for stats
            MessagingCenter.Send<BaseViewModel, MyShow>(this, "DeleteFromMyShows", myShowToDelete);
            
            MyShowsCollection.RemoveFromMyShows(myShowToDelete);

        }

        /// <summary>
        /// Called when user chooses to add this show from his/her favorite collection
        /// </summary>
        /// <param name="myShowToDelete">The show to update</param>
        public void AddShowToFavorites(MyShow myToBeFavoriteShow)
        {
            myToBeFavoriteShow.IsFavorite = true;
            MessagingCenter.Send<BaseViewModel, MyShow>(this, "UpdateMyShow", myToBeFavoriteShow);
        }

        /// <summary>
        /// Called when user chooses to remove this show from his/her favorite collection
        /// </summary>
        /// <param name="myShowToDelete">The show to update</param>
        public void RemoveShowFromFavorites(MyShow myNoLongerFavoriteShow)
        {
            myNoLongerFavoriteShow.IsFavorite = false;
            MessagingCenter.Send<BaseViewModel, MyShow>(this, "UpdateMyShow", myNoLongerFavoriteShow);

        }

        /// <summary>
        /// Called when user chooses to modify the notification setting for this show
        /// </summary>
        /// <param name="myShowToDelete">The show to update</param>
        public void ChangeNotifyValue(MyShow myShowToUpdate)
        {
            if (myShowToUpdate.MustNotify)
            {
                myShowToUpdate.MustNotify = false;
            }
            else
            {
                myShowToUpdate.MustNotify = true;
            }
            MessagingCenter.Send<BaseViewModel, MyShow>(this, "UpdateMyShow", myShowToUpdate);

        }

        /// <summary>
        /// Called when user chooses to change the last episode watched for this show
        /// </summary>
        /// <param name="episodeInWatch">The last episode watched</param>
        /// <param name="seasonInWatch">The season this episode belongs to</param>        
        public void modifyMyShow(int episodeInWatch, int seasonInWatch)
        {
            MyShow myShow = MyShowsCollection.Instance.FirstOrDefault(x => x.Id == this.Show.Id);
            
            EpisodeSeason newLastEpisodeWatched = new EpisodeSeason(episodeInWatch, seasonInWatch);

            EpisodeSeason oldNextEpisodeWatched = myShow.NextEpisode();
            EpisodeSeason oldLastEpisodeWatched = myShow.LastEpisodeWatched;
           
            myShow.LastEpisodeWatched = newLastEpisodeWatched;
            // Update Show attribute to update UI with OnPropertyChanged
            this.Show = myShow;
            
            MyShowsCollection.ModifyShowInMyShows(myShow);


            object[] oldNEWAndShow = new object[] { oldNextEpisodeWatched, oldLastEpisodeWatched, myShow };

            MessagingCenter.Send<BaseViewModel, MyShow>(this, "UpdateMyShow", myShow);
            MessagingCenter.Send<BaseViewModel, object[]>(this, "UpdateStats", oldNEWAndShow);
           
        }
    }
}
