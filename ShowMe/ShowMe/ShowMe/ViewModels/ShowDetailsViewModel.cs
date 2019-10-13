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
           
        }

        public async Task LoadEpisodes()
        {
            List<Episode> EpisodesList = await service.GetEpisodesListAsync(Show.Id);
            if (EpisodesList != null)
            {
                this.Show.EpisodesList = EpisodesList;
            }
        }


        public async void AddShowToMyShowsCollection(Show showToAdd, string EpisodeInWatch, string SeasonInWatch)
        {
          
            MyShow myShow = new MyShow(showToAdd, false, true, new Dictionary<string, int>{ { "episode", Int32.Parse(EpisodeInWatch) }, { "season", Int32.Parse(SeasonInWatch) } });
            MessagingCenter.Send<ShowDetailsViewModel, MyShow>(this, "AddToMyShows", myShow);
          
            MyShowsCollection.AddToMyShows(myShow);

            await FireBaseHelper.AddShowToUserList(App.User.Id, myShow);
            
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
