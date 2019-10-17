﻿using ShowMe.Services;
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
                        List<MyShow> s = FireBaseHelper.GetUserShowList(App.User.Id).Result;
                        foreach (MyShow myShow in s)
                        {
                            instance.Add(myShow);
                        }
                    }
                    return instance;
                }
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

    }
}
