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
    public partial class HomeUpcommingPage : ContentPage
    {
        HomeUpcommingViewModel viewModel;
        public HomeUpcommingPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new HomeUpcommingViewModel();
        }

        protected override void OnAppearing()
        {
            
            base.OnAppearing();
            viewModel.Init();
        }
    }
}