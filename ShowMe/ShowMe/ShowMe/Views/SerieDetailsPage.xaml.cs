using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShowMe.Models;
using ShowMe.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShowMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SerieDetailsPage : ContentPage
    {
        SerieDetailsViewModel viewModel;

        public SerieDetailsPage(SerieDetailsViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        void OnClickAddFavorites(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "AddFavorite", viewModel.Serie);
        }
    }
}