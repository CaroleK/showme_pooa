﻿using System;
using System.Collections.Generic;
using System.Text;
using ShowMe.Models;
using ShowMe.ViewModels;

namespace ShowMe.ViewModels
{
    class MainPageViewModel
    {

        public MainPageViewModel(User user)
        {
            BaseViewModel.user = user;
        }
    }
}
