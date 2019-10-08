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
            if (MyShows.Any(e => e.Id==show.Id))
            {
                MyShow myshow = MyShows.First(e => e.Id == show.Id);
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
            MyShow myShow = new MyShow(showToAdd, false, true, new Dictionary<string, int>{ { "episode", 1 }, { "season", 1 } });
            MessagingCenter.Send<ShowDetailsViewModel, MyShow>(this, "AddToMyShows", myShow);
            await FireBaseHelper.AddShowToUserList(user.Id, myShow);
            
        }

        public async void DeleteShowFromMyShowsCollection(MyShow myShowToDelete)
        {
            MessagingCenter.Send<ShowDetailsViewModel, MyShow>(this, "DeleteFromMyShows", myShowToDelete);
            await FireBaseHelper.DeleteShowFromUserList(user.Id, myShowToDelete);
            
        }

        public void AddShowToFavorites(Show myToBeFavoriteShow)
        {
            MyShow myFavoriteShow = MyShows.FirstOrDefault(x => x.Id == myToBeFavoriteShow.Id);
            myFavoriteShow.IsFavorite = true;
            MessagingCenter.Send<ShowDetailsViewModel, MyShow>(this, "ChangeToFavorite", myFavoriteShow);

        }
    }
}
