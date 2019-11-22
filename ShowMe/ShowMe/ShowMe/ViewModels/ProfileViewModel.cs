using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShowMe.Models;
using ShowMe.Services;
using Xamarin.Forms;

namespace ShowMe.ViewModels
{
    /// <summary>
    /// View Model associated with the Profile Page
    /// </summary>
    public class ProfileViewModel : BaseViewModel
    {
        // Current logged in user
        private User _myUser { get; set; }
        public User MyUser
        {
            get { return _myUser; }
            set { _myUser = value; OnPropertyChanged(); }
        }

       
        public ProfileViewModel()
        {
            this.MyUser = App.User;
            
            MessagingCenter.Subscribe<BaseViewModel, MyShow>(this, "AddToMyShows", (obj, item) =>
            {
                UpdateNumberOfEpisodesWatched(item.LastEpisodeWatched, item.SeasonsList, true);
            });

            MessagingCenter.Subscribe<BaseViewModel, MyShow>(this, "DeleteFromMyShows", (obj, item) =>
            {
                UpdateNumberOfEpisodesWatched(item.LastEpisodeWatched, item.SeasonsList, false);
            });

            MessagingCenter.Subscribe<BaseViewModel, MyShow>(this, "IncrementOneEpisode", (obj, item) =>
            {
                IncrementOrDecrementNumberOfEpisodesWatched(1, item.LastEpisodeWatched.Duration);
            });

            MessagingCenter.Subscribe<BaseViewModel, object[]>(this, "UpdateStats", (obj, item) =>
            {
                EpisodeSeason oldLastEpisodeWatched = item[0] as EpisodeSeason;
                MyShow newShow = item[1] as MyShow;
             
                int differenceInEpisodes = newShow.LastEpisodeWatched.IsApartInNumberOfEpisodes(oldLastEpisodeWatched, newShow);
                //because EpisodeSeason object has an empty Duration attribute (To fix)
                IncrementOrDecrementNumberOfEpisodesWatched(differenceInEpisodes, newShow.SeasonsList[oldLastEpisodeWatched.SeasonNumber].EpisodesOfSeason[oldLastEpisodeWatched.EpisodeNumber].DurationInMinutes);
            });
        }

        public void Init()
        {
            this.MyUser = App.User;
        }

        private void UpdateNumberOfEpisodesWatched(EpisodeSeason lastEpisodeWatched, List<Season> seasonsList, bool AddingEpisodes)
        {
            int variation = AddingEpisodes ? 1 : -1;

            if (lastEpisodeWatched != null)
            {
                int lastSeasonNumber = lastEpisodeWatched.SeasonNumber;
                int lastEpisodeNumber = lastEpisodeWatched.EpisodeNumber;

                for (int i = 1; i < lastSeasonNumber; i++)
                {
                    int index = seasonsList.IndexOf(seasonsList.First(s => s.Number == i));
                    foreach (Episode episode in seasonsList[index].EpisodesOfSeason)
                    {
                        App.User.TotalNbrEpisodesWatched += variation;
                        UpdateMinutesInTotalMinutesWatched(episode.DurationInMinutes, variation);
                    }
                }
                int indexSeason = seasonsList.IndexOf(seasonsList.First(s => s.Number == lastSeasonNumber));
                for (int i = 1; i < lastEpisodeNumber + 1; i++)
                {
                    App.User.TotalNbrEpisodesWatched += variation;
                    int index = seasonsList[indexSeason].EpisodesOfSeason.IndexOf(seasonsList[indexSeason].EpisodesOfSeason.First(e => e.Number == i));
                    UpdateMinutesInTotalMinutesWatched(seasonsList[indexSeason].EpisodesOfSeason[index].DurationInMinutes, variation);
                }
                MyUser = App.User;

                //Send Message to Firebase for update
                MessagingCenter.Send<BaseViewModel,User>(this, "UpdateUser", App.User);
            }
        }


        public void UpdateMinutesInTotalMinutesWatched(int Duration, int increasingOrDecreasingVariation)
        {

            App.User.TotalMinutesWatched += increasingOrDecreasingVariation * Duration;
        }

        public void IncrementOrDecrementNumberOfEpisodesWatched(int numberOfEpisodes, int Duration)
        {
            App.User.TotalNbrEpisodesWatched += numberOfEpisodes;
            App.User.TotalMinutesWatched += numberOfEpisodes*Duration;
            MyUser = App.User;
            MessagingCenter.Send<BaseViewModel, User>(this, "UpdateUser", App.User);
        }

    }
}


 