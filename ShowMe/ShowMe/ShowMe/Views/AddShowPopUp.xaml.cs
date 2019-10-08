using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShowMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddShowPopUp : PopupPage
    {
        public AddShowPopUp()
        {
            InitializeComponent();
        }


        private async void OnSave(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }
    }
}