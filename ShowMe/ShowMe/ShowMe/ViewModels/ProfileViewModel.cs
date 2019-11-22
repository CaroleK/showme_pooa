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
                UpdateNumberOfEpisodesWatched(item.LastEpisodeWatched, item.SeasonsList, true, new EpisodeSeason(1, 1));
            });

            MessagingCenter.Subscribe<BaseViewModel, MyShow>(this, "DeleteFromMyShows", (obj, item) =>
            {
                UpdateNumberOfEpisodesWatched(item.LastEpisodeWatched, item.SeasonsList, false, new EpisodeSeason(1, 1));
            });

            MessagingCenter.Subscribe<BaseViewModel, MyShow>(this, "IncrementOneEpisode", (obj, item) =>
            {
                IncrementOrDecrementNumberOfEpisodesWatched(item.NextEpisode().Duration);
            });

            MessagingCenter.Subscribe<BaseViewModel, object[]>(this, "UpdateStats", (obj, item) =>
            {
                EpisodeSeason oldNextEpisodeWatched = item[0] as EpisodeSeason;
                EpisodeSeason oldLastEpisodeWatched = item[1] as EpisodeSeason;
                MyShow newShow = item[2] as MyShow;
                EpisodeSeason lastEpisodeWatched = newShow.LastEpisodeWatched;

                if (lastEpisodeWatched.IsAfter(oldNextEpisodeWatched))
                {
                    UpdateNumberOfEpisodesWatched(lastEpisodeWatched, newShow.SeasonsList, true, oldNextEpisodeWatched);
                }
                else
                {
                    UpdateNumberOfEpisodesWatched(oldLastEpisodeWatched, newShow.SeasonsList, false, newShow.NextEpisode());
                }
            });
        }

        public void Init()
        {
            this.MyUser = App.User;
        }

        private void UpdateNumberOfEpisodesWatched(EpisodeSeason lastEpisodeWatched, List<Season> seasonsList, bool AddingEpisodes, EpisodeSeason nextEpisodeSeason)
        {
            int variation = AddingEpisodes ? 1 : -1;

            int nextSeasonIndex = seasonsList.IndexOf(seasonsList.First(s => s.Number == nextEpisodeSeason.SeasonNumber));
            //int initialEpisodeIndex = seasonsList[initialSeasonIndex].EpisodesOfSeason.IndexOf(seasonsList[initialSeasonIndex].EpisodesOfSeason.First(s => s.Number == initialLastEW.EpisodeNumber)); 

            if (lastEpisodeWatched != null)
            {
                int lastSeasonNumber = lastEpisodeWatched.SeasonNumber;
                int lastEpisodeNumber = lastEpisodeWatched.EpisodeNumber;

                int finalNumber;
                finalNumber = (int)seasonsList[nextSeasonIndex].EpisodesOfSeason.LastOrDefault().Number;

                if (lastSeasonNumber == nextEpisodeSeason.SeasonNumber)
                {
                    finalNumber = lastEpisodeWatched.EpisodeNumber;
                    for (int j = nextEpisodeSeason.EpisodeNumber; j < finalNumber + 1; j++)
                    {
                        App.User.TotalNbrEpisodesWatched += variation;
                        int index = seasonsList[nextSeasonIndex].EpisodesOfSeason.IndexOf(seasonsList[nextSeasonIndex].EpisodesOfSeason.First(e => e.Number == j));
                        UpdateMinutesInTotalMinutesWatched(seasonsList[nextSeasonIndex].EpisodesOfSeason[index].DurationInMinutes, variation);
                    }

                }
                else
                {


                    for (int j = nextEpisodeSeason.EpisodeNumber; j < finalNumber + 1; j++)
                    {
                        App.User.TotalNbrEpisodesWatched += variation;
                        int index = seasonsList[nextSeasonIndex].EpisodesOfSeason.IndexOf(seasonsList[nextSeasonIndex].EpisodesOfSeason.First(e => e.Number == j));
                        UpdateMinutesInTotalMinutesWatched(seasonsList[nextSeasonIndex].EpisodesOfSeason[index].DurationInMinutes, variation);
                    }


                    for (int i = nextEpisodeSeason.SeasonNumber + 1; i < lastSeasonNumber; i++)
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

        public void IncrementOrDecrementNumberOfEpisodesWatched(int Duration)
        {
            App.User.TotalNbrEpisodesWatched += 1;
            App.User.TotalMinutesWatched += Duration;
            MyUser = App.User;
            MessagingCenter.Send<BaseViewModel, User>(this, "UpdateUser", App.User);
        }

    }
}


 