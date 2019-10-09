using ShowMe.Services;
using ShowMe.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace ShowMe.Models
{
    public sealed class MyShowsCollection
    {
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
                        Task.Run(()=>FetchMyShows()).Wait();
                    }
                    return instance;
                }
            }
        }

        public async static void FetchMyShows()
        {
            List<MyShow> s = await FireBaseHelper.GetUserShowList(BaseViewModel.user.Id);
            foreach (MyShow myShow in s)
            {
                MyShowsCollection.Instance.Add(myShow);
            }
        }

        public static void AddToMyShows(MyShow ms)
        {
            Instance.Add(ms);
        }

        public static void RemoveFromMyShows(MyShow ms)
        {
            if (Instance.Contains(ms))
            {
                Instance.Remove(ms);
            }
        }

    }
}
