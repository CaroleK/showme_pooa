using System;
using System.Collections.Generic;
using System.Text;
using ShowMe.Services;
using ShowMe.Models;
using ShowMe.Views;
using System.Linq;
using Xamarin.Forms;


namespace ShowMe.ViewModels
{
    public class ShowDetailsViewModel : BaseViewModel
    {
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
           
        }

        public async void AddShowToMyShowsCollection(Show showToAdd)
        {
            // If user has not started watching, LastEpisodeWatched should be null
            MyShow myShow = new MyShow(showToAdd, false, true, new Dictionary<string, int>{ { "episode", 1 }, { "season", 1 } });

            // You might wanna subscribe to this in a viewModel
            MessagingCenter.Send<ShowDetailsViewModel, MyShow>(this, "AddToMyShows", myShow);
            // Add to local collection instance
            MyShowsCollection.AddToMyShows(myShow);

            // Add to cloud storage
            await FireBaseHelper.AddShowToUserList(user.Id, myShow);
            
        }

        public async void DeleteShowFromMyShowsCollection(MyShow myShowToDelete)
        {
            // You might wanna subscribe to this in a viewModel
            MessagingCenter.Send<ShowDetailsViewModel, MyShow>(this, "DeleteFromMyShows", myShowToDelete);

            // Remove from local collection instance
            MyShowsCollection.RemoveFromMyShows(myShowToDelete);

            // Remove from cloud storage
            await FireBaseHelper.DeleteShowFromUserList(user.Id, myShowToDelete);
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
