using System;
using System.Collections.Generic;
using System.Text;
using ShowMe.Services;
using ShowMe.Models;
using ShowMe.Views;
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

        public async void AddShowToShowList(Show showToAdd)
        {
            MyShow myShow = new MyShow(showToAdd, false, true, new Episode(1, 1, showToAdd.Id));
            await FireBaseHelper.AddShowToUserList(user.Id, myShow);
            MessagingCenter.Send<ShowDetailsViewModel, MyShow>(this, "AddToMyShows", myShow);
        }

        public async void DeleteShowFromShowList(MyShow myShowToDelete)
        {
            await FireBaseHelper.DeleteShowFromUserList(user.Id, myShowToDelete);
            MessagingCenter.Send<ShowDetailsViewModel, MyShow>(this, "DeleteFromMyShows", myShowToDelete);
        }
    }
}
