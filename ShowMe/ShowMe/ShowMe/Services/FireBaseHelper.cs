﻿using System;
using System.Collections.Generic;
using System.Text;
using Firebase.Database;
using Firebase.Database.Offline;
using Firebase.Database.Query;
using System.Threading.Tasks;
using ShowMe.Models;
using Xamarin.Forms;
using System.Linq;
using ShowMe.ViewModels;
using System.Collections.ObjectModel;
using LiteDB;
using Newtonsoft.Json;

namespace ShowMe.Services
{
    public class FireBaseHelper
    {
        static FirebaseClient Myfirebase = new FirebaseClient(Constants.FireBaseUrl, new FirebaseOptions
        {
            OfflineDatabaseFactory = (t, s) => new OfflineDatabase(t, s),
        });

        public FireBaseHelper()
        {
            MessagingCenter.Subscribe<ShowDetailsViewModel, MyShow>(this, "ChangeToFavorite", async (obj, item) =>
            {

                await UpdateMyShow(item);
            });

            MessagingCenter.Subscribe<ShowDetailsViewModel, MyShow>(this, "ChangeToNotFavorite", async (obj, item) =>
            {

                await UpdateMyShow(item);
            });

            MessagingCenter.Subscribe<HomeWatchListViewModel, MyShow>(this, "IncrementEpisode", async (obj, item) =>
            {

                await UpdateMyShow(item);
            });

            MessagingCenter.Subscribe<ShowDetailsViewModel, MyShow>(this, "ChangeLastEpisodeWatched", async (obj, item) =>
            {

                await UpdateMyShow(item);
            });
            //TODO : use one event called "UpdateMyShowProperties"? with BaseViewModel as sender?

        }


         static public async Task<bool> CheckIfUserExists(string userId)
        {
            var toCheckUser = (await Myfirebase
              .Child("Users")
              .OnceAsync<User>()).Where(a => a.Object.Id == userId).FirstOrDefault();

            if (!(toCheckUser == null)) { return true; }

            return false;
        }

       

        static public async Task AddUser(string userId, string name, string picture)
        {
            User user = new User() { Id = userId, Name = name, Picture = picture };
            await Myfirebase
              .Child("Users")
              .Child(user.Id)
              .PutAsync(user);
        }

        static public async Task ModifyUser(string userId, int TotalMinutesWatched)
        {
            await Myfirebase
              .Child("Users")
              .Child(userId)
              .Child("TotalMinutesWatched")
              .PutAsync(TotalMinutesWatched);
        }

        static public async Task AddShowToUserList(string UserId, MyShow selectedShow)
        {
            await Myfirebase
              .Child("Users_Shows_List")
              .Child(UserId)
              .Child(selectedShow.Title)
              .PutAsync(selectedShow);
        }

        static public async Task<List<MyShow>> GetUserShowList(string userId)
        {
            List<MyShow> showList = new List<MyShow>();
            var shows = await Myfirebase
              .Child("Users_Shows_List")
              .Child(userId)
              .OnceAsync<MyShow>();

            foreach (var show in shows)
            {
                MyShow newShow = show.Object; 
                showList.Add(newShow);
            }

            return showList;
        }

        internal static async Task DeleteShowFromUserList(string UserId, MyShow myShowToDelete)
        {
            await Myfirebase
              .Child("Users_Shows_List")
              .Child(UserId)
              .Child(myShowToDelete.Title)
              .DeleteAsync();
        }


        internal async Task UpdateMyShow(MyShow showToUpdate)
        {
            await Myfirebase
                .Child("Users_Shows_List")
                .Child(App.User.Id)
                .Child(showToUpdate.Title)
                .PutAsync(showToUpdate);
        }
    }
}
