using System;
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
        // The firebase client
        static FirebaseClient Myfirebase = new FirebaseClient(Constants.FireBaseUrl, new FirebaseOptions
        {
            OfflineDatabaseFactory = (t, s) => new OfflineDatabase(t, s),
        });

        /// <summary>
        /// Constructor for the helper
        /// Helper initialized with subscrictions to database events
        /// </summary>
        public FireBaseHelper()
        {
            MessagingCenter.Subscribe<ShowDetailsViewModel, MyShow>(this, "ChangeToFavorite", async (obj, item) =>
            {

                await UpdateMyShow(App.User.Id, item);
            });

            MessagingCenter.Subscribe<ShowDetailsViewModel, MyShow>(this, "ChangeToNotFavorite", async (obj, item) =>
            {

                await UpdateMyShow(App.User.Id, item);
            });

            MessagingCenter.Subscribe<HomeWatchListViewModel, MyShow>(this, "IncrementEpisode", async (obj, item) =>
            {

                await UpdateMyShow(App.User.Id, item);
            });

            MessagingCenter.Subscribe<ShowDetailsViewModel, MyShow>(this, "ChangeLastEpisodeWatched", async (obj, item) =>
            {

                await UpdateMyShow(App.User.Id, item);
            });

            MessagingCenter.Subscribe<ShowDetailsViewModel, MyShow>(this, "UpdateMyShow", async (obj, item) =>
            {

                await UpdateMyShow(App.User.Id, item);
            });
            //TODO : use one event called "UpdateMyShowProperties"? with BaseViewModel as sender?

        }

        /// <summary>
        /// Check is the given id matches with an existing user
        /// </summary>
        /// <param name="userId">The user id to check for</param>
        /// <returns>Boolean task, true is user exists, fasle otherwise</returns>
        static public async Task<bool> CheckIfUserExists(string userId)
        {
            var toCheckUser = (await Myfirebase
              .Child("Users")
              .OnceAsync<User>()).Where(a => a.Object.Id == userId).FirstOrDefault();

            if (!(toCheckUser == null)) { return true; }

            return false;
        }


        /// <summary>
        /// Add new user to databse
        /// </summary>
        /// <param name="userId">The user id</param>
        /// <param name="name">The user's name</param>
        /// <param name="picture">The user's profile picture</param>
        /// <returns>Void task</returns>
        static public async Task AddUser(string userId, string name, string picture)
        {
            User user = new User() { Id = userId, Name = name, Picture = picture };
            await Myfirebase
              .Child("Users")
              .Child(user.Id)
              .PutAsync(user);
        }

        static public async Task ModifyMinutesWatchedUser(string userId, int TotalMinutesWatched)
        {
            await Myfirebase
              .Child("Users")
              .Child(userId)
              .Child("TotalMinutesWatched")
              .PutAsync(TotalMinutesWatched);
        }

        static public async Task ModifyNbrEpisodesWatchedUser(string userId, int TotalNbrEpisodesWatched)
        {
            await Myfirebase
              .Child("Users")
              .Child(userId)
              .Child("TotalNbrEpisodesWatched")
              .PutAsync(TotalNbrEpisodesWatched);
        }

        /// <summary>
        /// Add a new show to user's show list
        /// </summary>
        /// <param name="UserId">The user id</param>
        /// <param name="selectedShow">The show to add</param>
        /// <returns>Void task</returns>
        static public async Task AddShowToUserList(string UserId, MyShow selectedShow)
        {
            await Myfirebase
              .Child("Users_Shows_List")
              .Child(UserId)
              .Child(selectedShow.Title)
              .PutAsync(selectedShow);
        }

        /// <summary>
        /// Retrieves the user show list
        /// </summary>
        /// <param name="userId">The user id</param>
        /// <returns>A task with a list of MyShow objects</returns>
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

        /// <summary>
        /// Deletes given show from user's list
        /// </summary>
        /// <param name="UserId">The user id</param>
        /// <param name="myShowToDelete">The show to delete</param>
        /// <returns>Void task</returns>
        internal static async Task DeleteShowFromUserList(string UserId, MyShow myShowToDelete)
        {
            await Myfirebase
              .Child("Users_Shows_List")
              .Child(UserId)
              .Child(myShowToDelete.Title)
              .DeleteAsync();
        }

        /// <summary>
        /// Updates given show in the user's list
        /// </summary>
        /// <param name="UserId">The user id</param>
        /// <param name="showToUpdate">The show to update</param>
        /// <returns>Void task</returns>
        internal async Task UpdateMyShow(string UserId, MyShow showToUpdate)
        {
            await Myfirebase
                .Child("Users_Shows_List")
                .Child(UserId)
                .Child(showToUpdate.Title)
                .PutAsync(showToUpdate);
        }
    }
}
