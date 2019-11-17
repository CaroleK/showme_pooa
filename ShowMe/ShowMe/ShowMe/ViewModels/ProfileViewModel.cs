using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using ShowMe.Models;

namespace ShowMe.ViewModels
{
    public class ProfileViewModel : BaseViewModel
    {
        private User _myUser { get; set; }

        public User MyUser
        {
            get { return _myUser; }
            set { _myUser = value; OnPropertyChanged(); }
        }

       
        public ProfileViewModel()
        {
            this.MyUser = App.User;
        }

        public void Init()
        {
            this.MyUser = App.User;

        }

    }
}
