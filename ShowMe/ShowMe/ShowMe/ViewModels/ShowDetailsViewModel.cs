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
    public class ShowDetailsViewModel : BaseViewModel
    {
        public TvMazeService service = new TvMazeService();

        private Show _show { get; set; }

        public Show Show {
            get { return _show; }
            set { _show = value; OnPropertyChanged(); }
        }
        public Season SelectedSeason { get; set; }

        public ShowDetailsViewModel(Show show = null) : base()
        {
            var s = MyShowsCollection.Instance;
            if (s.Any(e => e.Id == show.Id))
            {
                MyShow myShow = MyShowsCollection.Instance.First(e => e.Id == show.Id);
                Title = myShow?.Title;
                Show = myShow;
            }
            else
            {
                Title = show?.Title;
                Show = show;
                Task.Run(() => LoadEpisodes()).Wait();
                Task.Run(() => LoadSeasons()).Wait();
                Task.Run(() => LoadActors()).Wait();
            }

        }

        public async Task LoadEpisodes()
        {
            List<Episode> EpisodesList = await service.GetEpisodesListAsync(Show.Id);
            if (EpisodesList != null)
            {
                this.Show.EpisodesList = EpisodesList;
            }
        }

        public async Task LoadSeasons()
        {
            List<Season> SeasonsList = await service.GetSeasonsListAsync(Show.Id);
            if (SeasonsList != null)
            {
                this.Show.SeasonsList = SeasonsList;
                // Add relevant episodes to the season's episodes list
                foreach (Season s in SeasonsList)
                {
                    s.EpisodesOfSeason = this.Show.EpisodesList.Where(e => e.Season == s.Number).ToList();
                }
            }
        }

        public async Task LoadActors()
        {
            List<Actor> ActorsList = await service.GetCastAsync(Show.Id);
            if (ActorsList != null)
            {
                this.Show.Cast = ActorsList;
            }
        }

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

        public void DeleteShowFromMyShowsCollection(MyShow myShowToDelete)
        {
            //Send message to FireBase and App.User for stats
            MessagingCenter.Send<BaseViewModel, MyShow>(this, "DeleteFromMyShows", myShowToDelete);
            
            MyShowsCollection.RemoveFromMyShows(myShowToDelete);

        }

        public void AddShowToFavorites(MyShow myToBeFavoriteShow)
        {
            myToBeFavoriteShow.IsFavorite = true;
            MessagingCenter.Send<BaseViewModel, MyShow>(this, "UpdateMyShow", myToBeFavoriteShow);
        }

        public void RemoveShowFromFavorites(MyShow myNoLongerFavoriteShow)
        {
            myNoLongerFavoriteShow.IsFavorite = false;
            MessagingCenter.Send<BaseViewModel, MyShow>(this, "UpdateMyShow", myNoLongerFavoriteShow);

        }

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

        public void modifyMyShow(int episodeInWatch, int seasonInWatch)
        {
            MyShow myShow = MyShowsCollection.Instance.FirstOrDefault(x => x.Id == this.Show.Id);
            myShow.LastEpisodeWatched = new EpisodeSeason(episodeInWatch, seasonInWatch);
            this.Show = myShow;
            MyShowsCollection.ModifyShowInMyShows(myShow);
            MessagingCenter.Send<BaseViewModel, MyShow>(this, "UpdateMyShow", myShow);
            //TODO : change user stats
        }
    }
}
