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
using Xamarin.Forms.Internals;
using System.Diagnostics;

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
            MessagingCenter.Subscribe<BaseViewModel, MyShow>(this, "UpdateMyShow", async (obj, item) =>
            {

                await UpdateMyShow(App.User.Id, item);
            });

            MessagingCenter.Subscribe<BaseViewModel, MyShow>(this, "AddToMyShows", async (obj, item) =>
            {

                await AddShowToUserList(App.User.Id, item);
            });

            MessagingCenter.Subscribe<BaseViewModel, MyShow>(this, "DeleteFromMyShows", async (obj, item) =>
            {
                await DeleteShowFromUserList(App.User.Id, item);
            });

            MessagingCenter.Subscribe<BaseViewModel, User>(this, "UpdateUser", async (obj, item) =>
            {
                await UpdateUser(App.User.Id, item);
            });
        }

        /// <summary>
        /// Check is the given id matches with an existing user
        /// </summary>
        /// <param name="userId">The user id to check for</param>
        /// <returns>Boolean task, true is user exists, fasle otherwise</returns>
        static public async Task<bool> CheckIfUserExists(string userId)
        {
            try
            {
                var toCheckUser = (await Myfirebase
                  .Child("Users")
                  .OnceAsync<User>()).Where(a => a.Object.Id == userId).FirstOrDefault();

                if (!(toCheckUser == null)) { return true; }
            }
            catch (Exception e)
            {
                Debug.WriteLine("\tERROR {0}", e.Message);
            }            

            return false;
        }

        static public async Task<User> RetrieveUser(string userId)
        {
            try
            {
                var toCheckUser = (await Myfirebase
                  .Child("Users")
                  .OnceAsync<User>()).Where(a => a.Object.Id == userId).FirstOrDefault();

                User identifiedUser = toCheckUser.Object;
                return identifiedUser;
            }
            catch (Exception e)
            {
                Debug.WriteLine("\tERROR {0}", e.Message);
                return null;
            }

        }


        /// <summary>
        /// Add new user to databse
        /// </summary>
        /// <param name="userId">The user id</param>
        /// <param name="name">The user's name</param>
        /// <param name="picture">The user's profile picture</param>
        /// <returns>Bool task that's true if addition succeded, false otherwise</returns>
        static public async Task<bool> AddUser(string userId, string name, string picture)
        {
            try
            {
                User user = new User() { Id = userId, Name = name, Picture = picture };
                await Myfirebase
                  .Child("Users")
                  .Child(user.Id)
                  .PutAsync(user);
                return true;
            }
            
            catch (Exception e)
            {                
                Debug.WriteLine("\tERROR {0}", e.Message);
                return false;
            }
        }

        static public async Task UpdateUser(string userId, User user)
        {
            try
            {
                await Myfirebase
                  .Child("Users")
                  .Child(userId)
                  .PutAsync(user);
            }
            catch (Exception e)
            {
                Debug.WriteLine("\tERROR {0}", e.Message);
                DependencyService.Get<IMessage>().Show("Sorry, a problem occured... We failed to save your changes in our database.");
            }

        }

        /// <summary>
        /// Add a new show to user's show list
        /// </summary>
        /// <param name="UserId">The user id</param>
        /// <param name="selectedShow">The show to add</param>
        /// <returns>Void task</returns>
        static public async Task AddShowToUserList(string UserId, MyShow selectedShow)
        {
            try
            {
                await Myfirebase
                  .Child("Users_Shows_List")
                  .Child(UserId)
                  .Child(selectedShow.Title)
                  .PutAsync(selectedShow);
            }
            catch (Exception e)
            {
                Debug.WriteLine("\tERROR {0}", e.Message);
                DependencyService.Get<IMessage>().Show("Sorry, a problem occured... We failed to save your changes in our database.");
            }
            
        }

        /// <summary>
        /// Retrieves the user show list
        /// </summary>
        /// <param name="userId">The user id</param>
        /// <returns>A task with a list of MyShow objects</returns>
        static public async Task<List<MyShow>> GetUserShowList(string userId)
        {
            List<MyShow> showList = new List<MyShow>();
            try
            {
                var shows = await Myfirebase
                  .Child("Users_Shows_List")
                  .Child(userId)
                  .OnceAsync<MyShow>();

                foreach (var show in shows)
                {
                    MyShow newShow = show.Object;
                    showList.Add(newShow);
                }
            }
            catch (Exception e)
            {
                DependencyService.Get<IMessage>().Show("Sorry, a problem occured... Your shows couldn't be retrieved.");
                Debug.WriteLine("\tERROR {0}", e.Message);
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
            try
            {
                await Myfirebase
                  .Child("Users_Shows_List")
                  .Child(UserId)
                  .Child(myShowToDelete.Title)
                  .DeleteAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine("\tERROR {0}", e.Message);
                DependencyService.Get<IMessage>().Show("Sorry, a problem occured... We failed to save your changes in our database.");
            }
            
        }

        /// <summary>
        /// Updates given show in the user's list
        /// </summary>
        /// <param name="UserId">The user id</param>
        /// <param name="showToUpdate">The show to update</param>
        /// <returns>Void task</returns>
        internal async Task UpdateMyShow(string UserId, MyShow showToUpdate)
        {
            try
            {
                await Myfirebase
                    .Child("Users_Shows_List")
                    .Child(UserId)
                    .Child(showToUpdate.Title)
                    .PutAsync(showToUpdate);
            }
            catch (Exception e)
            {
                Debug.WriteLine("\tERROR {0}", e.Message);
                DependencyService.Get<IMessage>().Show("Sorry, a problem occured... We failed to save your changes in our database.");
            }            
        }
    }
}
