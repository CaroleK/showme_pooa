using ShowMe.Services;
using ShowMe.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace ShowMe.Models
{
    /// <summary>
    /// Class to describe a collection of MyShow objects.
    /// Only has static methods
    /// And allows access to an ObservableCollection of MyShow, that implements a singleton pattern
    /// </summary>
    public sealed class MyShowsCollection
    {
        // Singleton pattern
        private static readonly object padlock = new object();
        private static ObservableCollection<MyShow> instance = null;
        public static ObservableCollection<MyShow> Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new ObservableCollection<MyShow>();                        
                        FetchMyShows();
                    }
                    return instance;
                }
            }
            set {
                instance = value;
            }
        }

        /// <summary>
        /// Retrieves the list of logged in user from Firebase database
        /// </summary>
        public static void FetchMyShows()
        {
            List<MyShow> s = Task.Run(() => FireBaseHelper.GetUserShowList(App.User.Id)).Result;
            foreach (MyShow myShow in s)
            {
                MyShowsCollection.Instance.Add(myShow);
            }
        }

        /// <summary>
        /// Adds a show MyShow to the Instance of the user's MyShow list
        /// </summary>
        /// <param name="ms">The show to add</param>
        public static void AddToMyShows(MyShow ms)
        {
            lock (padlock)
            {
                Instance.Add(ms);
            }
                
        }

        /// <summary>
        /// Removes a show MyShow from the Instance of the user's MyShow list
        /// If the show isn't in the list, does nothing
        /// </summary>
        /// <param name="ms">The show to remove</param>
        public static void RemoveFromMyShows(MyShow ms)
        {
            lock (padlock)
            {
                if (Instance.Contains(ms))
                {
                    Instance.Remove(ms);
                }
            }
        }

        /// <summary>
        /// Retrieves a show MyShow from the Instance of the user's MyShow list by its ID
        /// </summary>
        /// <param name="id">The ID of the MyShow to retrieve</param>
        /// <returns>The matching MyShow, or null if no show matches the ID</returns>
        public static MyShow GetByIdFromMyShows(int id)
        {
            foreach (MyShow myShow in Instance)
            {
                if (myShow.Id == id)
                {
                    return myShow;
                }                
            }

            return null;
        }

        /// <summary>
        /// Modifies a show MyShow in the Instance of the user's MyShow list
        /// If the show isn't in the list, does nothing
        /// </summary>
        /// <param name="ms">The show to modify</param>
        public static void ModifyShowInMyShows(MyShow newMyShow)
        {
            lock (padlock)
            {
                for (int i = 0; i < Instance.Count; i++)
                {
                    if (Instance[i].Id == newMyShow.Id)
                    {
                        Instance[i] = newMyShow;
                    }
                }
            }
        }
        
    }
}
