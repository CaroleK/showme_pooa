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
        public ShowDetailsViewModel(Show show = null)
        {
            Title = show?.Title;
            Show = show;
        }

        public async void AddShowToMyShowsCollection(Show showToAdd)
        {
            MyShow myShow = new MyShow(showToAdd, false, true, new Dictionary<string, int>{ { "episode", 1 }, { "season", 1 } });
            await FireBaseHelper.AddShowToUserList(user.Id, myShow);
            MessagingCenter.Send<ShowDetailsViewModel, MyShow>(this, "AddToMyShows", myShow);
        }

        public async void DeleteShowFromMyShowsCollection(MyShow myShowToDelete)
        {
            await FireBaseHelper.DeleteShowFromUserList(user.Id, myShowToDelete);
            MessagingCenter.Send<ShowDetailsViewModel, MyShow>(this, "DeleteFromMyShows", myShowToDelete);
        }

        public void AddShowToFavorites(Show myToBeFavoriteShow)
        {
            MyShow myFavoriteShow = MyShows.FirstOrDefault(x => x.Id == myToBeFavoriteShow.Id);
            myFavoriteShow.IsFavorite = true;
        }
    }
}
