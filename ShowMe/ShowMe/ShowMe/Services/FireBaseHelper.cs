using System;
using System.Collections.Generic;
using System.Text;
using Firebase.Database;
using Firebase.Database.Offline;
using Firebase.Database.Query;
using System.Threading.Tasks;
using ShowMe.Models;
using System.Linq;

namespace ShowMe.Services
{
    class FireBaseHelper
    {
        FirebaseClient Myfirebase = new FirebaseClient("https://showme-pooa.firebaseio.com/", new FirebaseOptions
        {
            OfflineDatabaseFactory = (t, s) => new OfflineDatabase(t, s),
        });

        public async Task<bool> CheckIfUserExists(string userId)
        {
            var toCheckUser = (await Myfirebase
              .Child("Users")
              .OnceAsync<User>()).Where(a => a.Object.Id == userId).FirstOrDefault();

            if (!(toCheckUser == null)) { return true; }

            return false;



        }
        public async Task AddUser(string userId, string name, string picture)
        {

            await Myfirebase
              .Child("Users")
              .PostAsync(new User() { Id = userId, Name = name, Picture = picture });
        }
    }
}
