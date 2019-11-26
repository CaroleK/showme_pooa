﻿using ShowMe.Models;
using ShowMe.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShowMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    /*Une fois la connexion vers une base de données effectuée, vous pouvez exécuter une requête et récupérer son résultat en utilisant l'objet Command.
    La création d'un objet Command nécessite l'instanciation d'un objet SqlCommand. 
    DesignTimeVisible Obtient ou définit une valeur indiquant si l'objet command doit être visible dans un contrôle du concepteur Windows Form.*/
    [DesignTimeVisible(false)]
    public partial class DiscoverPage : ContentPage
    {
        DiscoverViewModel viewModel;
        public DiscoverPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new DiscoverViewModel();
        }

        /// <summary>
        /// Defines what happens when user clicks on one of the shows in the list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            
            Show selectedItem = e.SelectedItem as Show;
            if (selectedItem == null)
            {
                return;
            }
            await Navigation.PushAsync(new ShowDetailsPage(new ShowDetailsViewModel(selectedItem)));

            // Manually deselect item
            DiscoverListView.SelectedItem = null;
        }

        /// <summary>
        /// If user scrolls down to the bottom, load more elements
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Handle_ItemAppearing (object sender, ItemVisibilityEventArgs e)
        {
            if (String.IsNullOrEmpty(DiscoverSearchBar.Text))
            {
                var itemTypeObject = e.Item as Show;
                if (viewModel.Shows.Last() == itemTypeObject)
                {
                    Task.Run(() => viewModel.ExecuteLoadShowCommand());
                }
            }

        }

        /// <summary>
        /// When user clicks on search button, fetch results
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Discover_SearchButtonPressed(object sender, EventArgs e)
        {
            // Displays shows depending on the search
            Task.Run(() => viewModel.ExecuteSearchShowCommand(DiscoverSearchBar.Text));
        }

        /// <summary>
        /// If the search bar is empty, display random shows
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Discover_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                // After erasing the search : displays shows by incrementing a counter
                viewModel.Shows.Clear();
                Task.Run(() => viewModel.ExecuteLoadShowCommand());
                
            }
        }
    }
}


