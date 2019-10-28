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

        public FireBaseHelper MyFireBaseHelper = new FireBaseHelper();

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

        public async void AddShowToMyShowsCollection(MyShow myShowToAdd)
        {
            // Fetch the last episode for this show, now that we're adding it to MyShows list we'll need this attribute
            myShowToAdd.LastEpisode = await service.GetLastEpisodeInShow(myShowToAdd.Id);

            //Update Show attribute to update UI with OnPropertyChanged
            this.Show = myShowToAdd;

            MessagingCenter.Send<ShowDetailsViewModel, MyShow>(this, "AddToMyShows", myShowToAdd);

            MyShowsCollection.AddToMyShows(myShowToAdd);

            //TODO : change method to not async by subscribing FBHelper
            await FireBaseHelper.AddShowToUserList(App.User.Id, myShowToAdd);

        }

        public async void DeleteShowFromMyShowsCollection(MyShow myShowToDelete)
        {
            MessagingCenter.Send<ShowDetailsViewModel, MyShow>(this, "DeleteFromMyShows", myShowToDelete);
            
            MyShowsCollection.RemoveFromMyShows(myShowToDelete);

            //TODO : change method to not async by subscribing FBHelper
            await FireBaseHelper.DeleteShowFromUserList(App.User.Id, myShowToDelete);

        }

        public void AddShowToFavorites(MyShow myToBeFavoriteShow)
        {
            myToBeFavoriteShow.IsFavorite = true;
            MessagingCenter.Send<ShowDetailsViewModel, MyShow>(this, "ChangeToFavorite", myToBeFavoriteShow);
        }

        public void RemoveShowFromFavorites(MyShow myNoLongerFavoriteShow)
        {
            myNoLongerFavoriteShow.IsFavorite = false;
            MessagingCenter.Send<ShowDetailsViewModel, MyShow>(this, "ChangeToNotFavorite", myNoLongerFavoriteShow);

        }

        public void modifyMyShow(int episodeInWatch, int seasonInWatch)
        {
            MyShow myShow = MyShowsCollection.Instance.FirstOrDefault(x => x.Id == this.Show.Id);
            myShow.LastEpisodeWatched = new Dictionary<string, int> { { "episode", episodeInWatch }, { "season", seasonInWatch } };
            this.Show = myShow;
            MyShowsCollection.ModifyShowInMyShows(myShow);
            MessagingCenter.Send<ShowDetailsViewModel, MyShow>(this, "ChangeLastEpisodeWatched", myShow);
        }
    }
}
