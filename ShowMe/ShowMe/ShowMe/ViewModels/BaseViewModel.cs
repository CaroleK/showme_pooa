using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using ShowMe.Models;
using ShowMe.Services;
using ShowMe.Views;
using Xamarin.Forms;

namespace ShowMe.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        string title = string.Empty;
        public ObservableCollection<MyShow> MyShows { get; set; } = new ObservableCollection<MyShow>();

        public static User user { set; get; }
        //protected User User { set { };  get { return _user; } }

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName]string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }
         public BaseViewModel()
        {
            FetchMyShows();
            MessagingCenter.Subscribe<ShowDetailsPage, MyShow>(this, "AddToMyShows", (obj, item) =>
            {
                MyShows.Add(item);
            });
        }

        public async void FetchMyShows()
        {
            List<MyShow> s = await FireBaseHelper.GetUserShowList(user.Id);
            foreach (MyShow myShow in s)
            {
                MyShows.Add(myShow);
            }
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
