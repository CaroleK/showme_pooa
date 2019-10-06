using ShowMe.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShowMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyShowsPage : ContentPage
    {
        MyShowsViewModel viewModel;
        public MyShowsPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new MyShowsViewModel();
        }
    }
}