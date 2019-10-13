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

        public Show Show { get; set; }
        public Season SelectedSeason { get; set; }

        public ShowDetailsViewModel(Show show = null) : base()
        {
            var s = MyShowsCollection.Instance;
            if (s.Any(e => e.Id==show.Id))
            {
                MyShow myshow = MyShowsCollection.Instance.First(e => e.Id == show.Id);
                Title = myshow?.Title;
                Show = myshow;
            }
            else
            {
                Title = show?.Title;
                Show = show;
            }
            Task.Run(() => LoadEpisodes()).Wait();
            Task.Run(() => LoadSeasons()).Wait();

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
            }
            foreach (Season s in SeasonsList)
            {
                s.EpisodesOfSeason = this.Show.EpisodesList.Where(e => e.Season == s.Number).ToList();
            }
        }


        public async void AddShowToMyShowsCollection(MyShow myShowToAdd)
        {
            MessagingCenter.Send<ShowDetailsViewModel, MyShow>(this, "AddToMyShows", myShowToAdd);

            MyShowsCollection.AddToMyShows(myShowToAdd);

            await FireBaseHelper.AddShowToUserList(App.User.Id, myShowToAdd);

        }

        public async void DeleteShowFromMyShowsCollection(MyShow myShowToDelete)
        {
            MessagingCenter.Send<ShowDetailsViewModel, MyShow>(this, "DeleteFromMyShows", myShowToDelete);
            
            MyShowsCollection.RemoveFromMyShows(myShowToDelete);
            
            await FireBaseHelper.DeleteShowFromUserList(App.User.Id, myShowToDelete);
            
            MyShowsCollection.RemoveFromMyShows(myShowToDelete);

        }

        public void AddShowToFavorites(Show myToBeFavoriteShow)
        {
            MyShow myFavoriteShow = MyShowsCollection.Instance.FirstOrDefault(x => x.Id == myToBeFavoriteShow.Id);
            myFavoriteShow.IsFavorite = true;
            MessagingCenter.Send<ShowDetailsViewModel, MyShow>(this, "ChangeToFavorite", myFavoriteShow);

        }
    }
}
