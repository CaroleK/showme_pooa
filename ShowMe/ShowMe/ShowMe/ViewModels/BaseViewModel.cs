using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using ShowMe.Models;

namespace ShowMe.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        string title = string.Empty;
        public static User user { set; get; }
        //protected User User { set { };  get { return _user; } }

        public ObservableCollection<MyShow> MyShows { get; set; } = new ObservableCollection<MyShow>();
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

        public BaseViewModel()
        {
            MessagingCenter.Subscribe<ShowDetailsViewModel, MyShow>(this, "AddToMyShows", (obj, item) =>
            {
                MyShows.Add(item);
            });

            MessagingCenter.Subscribe<ShowDetailsViewModel, MyShow>(this, "DeleteFromMyShows", (obj, item) =>
            {
                MyShows.Remove(item);
            });
        }
    }
}
