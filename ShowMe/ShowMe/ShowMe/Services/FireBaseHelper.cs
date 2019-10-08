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
    class FireBaseHelper
    {
        static FirebaseClient Myfirebase = new FirebaseClient(Constants.FireBaseUrl, new FirebaseOptions
        {
            OfflineDatabaseFactory = (t, s) => new OfflineDatabase(t, s),
        });

        //public FireBaseHelper()
        //{
        //    MessagingCenter.Subscribe<ShowDetailsViewModel, MyShow>(this, "ChangeToFavorite", (obj, item) => {

        //        UpdateMyShow(item);
        //    };


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

            await Myfirebase
              .Child("Users")
              .PostAsync(new User() { Id = userId, Name = name, Picture = picture });
        }

        static public async Task AddShowToUserList(string UserId, MyShow selectedShow)
        {
            await Myfirebase
              .Child("Users_Shows_List")
              .Child(UserId)
              .PostAsync(selectedShow);
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
              .DeleteAsync();
        }
    }
}
