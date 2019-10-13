using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using ShowMe.Models;
using ShowMe.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShowMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddShowPopUp : PopupPage
    {
        public class PopUpArgs : EventArgs
        {
            public string SeasonInWatch { get; set; }
            public string EpisodeInWatch { get; set; }
        }
        
        public delegate void PopUpClosedDelegate(object source, PopUpArgs eventArgs);

        public event PopUpClosedDelegate PopUpClosed;

        public AddShowPopUp()
        {
            InitializeComponent();
        }


        private async void OnSave(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
            OnPopUpClosed();
        }

        protected virtual void OnPopUpClosed()
        {
            if (PopUpClosed != null)
            {
                PopUpClosed(this, new PopUpArgs() { SeasonInWatch = EnteredSeason.Text, EpisodeInWatch = EnteredEpisode.Text });
            };
        }
    }
}