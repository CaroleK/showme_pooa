﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShowMe.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShowMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyShowsPage : ContentPage
    {
        MyShowsViewModel myShowsViewModel;

        public MyShowsPage()
        {
            InitializeComponent();
            BindingContext = myShowsViewModel = new MyShowsViewModel();
        }
    }
}