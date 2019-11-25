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
            Title = "My profile";

            //When we add a show, we consider the initial NextEpisode to be the first episode to watch 
            MessagingCenter.Subscribe<BaseViewModel, MyShow>(this, "AddToMyShows", (obj, item) =>
            {
                UpdateNumberOfEpisodesWatched(item.LastEpisodeWatched, item.SeasonsList, true, item.FirstEpisodeToWatch);
            });

            //When we delete a show, we consider the initial NextEpisode to be the first episode to watch 
            MessagingCenter.Subscribe<BaseViewModel, MyShow>(this, "DeleteFromMyShows", (obj, item) =>
            {
                UpdateNumberOfEpisodesWatched(item.LastEpisodeWatched, item.SeasonsList, false, item.FirstEpisodeToWatch);
            });

            //When we increment one episode, we consider the NextEpisode as the episode to watch
            MessagingCenter.Subscribe<BaseViewModel, MyShow>(this, "IncrementOneEpisode", (obj, item) =>
            {
                IncrementNumberOfEpisodesWatched(item.NextEpisode().Duration);
            });

            //When we update the LastEpisode number manually, we consider two cases: one where we increased the LastEpisode number, one where we decreased it
            MessagingCenter.Subscribe<BaseViewModel, object[]>(this, "UpdateStats", (obj, item) =>
            {
                EpisodeSeason oldNextEpisodeWatched = item[0] as EpisodeSeason;
                EpisodeSeason oldLastEpisodeWatched = item[1] as EpisodeSeason;
                MyShow newShow = item[2] as MyShow;
                EpisodeSeason lastEpisodeWatched = newShow.LastEpisodeWatched;

                //If the user increased the LastEpisode number seen
                if (lastEpisodeWatched.IsAfter(oldNextEpisodeWatched))
                {
                    UpdateNumberOfEpisodesWatched(lastEpisodeWatched, newShow.SeasonsList, true, oldNextEpisodeWatched);
                }
                //If the user decreased the LastEpisode number seen
                else
                {
                    UpdateNumberOfEpisodesWatched(oldLastEpisodeWatched, newShow.SeasonsList, false, newShow.NextEpisode());
                }
            });
        }

        /// <summary>
        /// Everytime ProfilePage is displayed, make sure MyUser object takes (new) values of App.User
        /// </summary>
        public void Init()
        {
            this.MyUser = App.User;
        }


        /// <summary>
        /// Calculate all the new stats by updtating TotalNbrOfEpisodesWatched and TotalMinutesWatched (App.User attributes)
        /// </summary>
        /// <param name="lastEpisodeWatched"></param> new input of user (where he said he stoppped watching)
        /// <param name="seasonsList"></param> the show's seasons in order to navigate within episodes and seasons
        /// <param name="AddingEpisodes"></param> indicates if we are adding or substracting episodes 
        /// <param name="nextEpisodeSeason"></param> current state for user (what he was supposed to watch next)
        private void UpdateNumberOfEpisodesWatched(EpisodeSeason lastEpisodeWatched, List<Season> seasonsList, bool AddingEpisodes, EpisodeSeason nextEpisodeSeason)
        {
            int variation = AddingEpisodes ? 1 : -1;

            int nextSeasonIndex = seasonsList.IndexOf(seasonsList.First(s => s.Number == nextEpisodeSeason.SeasonNumber));

            // verify that user is currently watching the show
            if (lastEpisodeWatched != null)
            {
                int lastSeasonNumber = lastEpisodeWatched.SeasonNumber;
                int lastEpisodeNumber = lastEpisodeWatched.EpisodeNumber;

                int finalNumber;

                //if change of LastEpisodeWatched occurs within same season
                if (lastSeasonNumber == nextEpisodeSeason.SeasonNumber)
                {
                    finalNumber = lastEpisodeWatched.EpisodeNumber;
                    //j starts at the next episode user was going to watch
                    for (int j = nextEpisodeSeason.EpisodeNumber; j < finalNumber + 1; j++)
                    {
                        if (seasonsList[nextSeasonIndex].EpisodesOfSeason.Exists(e => e.Number == j))
                        {
                            App.User.TotalNbrEpisodesWatched += variation;
                            int index = seasonsList[nextSeasonIndex].EpisodesOfSeason.IndexOf(seasonsList[nextSeasonIndex].EpisodesOfSeason.First(e => e.Number == j));
                            UpdateMinutesInTotalMinutesWatched(seasonsList[nextSeasonIndex].EpisodesOfSeason[index].DurationInMinutes, variation);
                        }
                    }
                }
                else
                {   //Step 1 : start with episodes in nextEpisodeSeason.Season 

                    //Last episode number of nextEpisodeSeason.Season 
                    finalNumber = (int)seasonsList[nextSeasonIndex].EpisodesOfSeason.Max(e => e.Number);

                    for (int j = nextEpisodeSeason.EpisodeNumber; j < finalNumber + 1; j++)
                    {
                        if (seasonsList[nextSeasonIndex].EpisodesOfSeason.Exists(e => e.Number == j))
                        {
                            App.User.TotalNbrEpisodesWatched += variation;
                            //retrieve index of episode j
                            int index = seasonsList[nextSeasonIndex].EpisodesOfSeason.IndexOf(seasonsList[nextSeasonIndex].EpisodesOfSeason.First(e => e.Number == j));
                            UpdateMinutesInTotalMinutesWatched(seasonsList[nextSeasonIndex].EpisodesOfSeason[index].DurationInMinutes, variation);
                        }
                    }

                    //Step 2 : all episodes in all seasons after nextEpisodeSeason.Season and before LastEpisodedWatched.Season
                    int nextSeasonAfterLastSeasonNumber = seasonsList.FindAll(s => s.Number > nextEpisodeSeason.SeasonNumber).Min(s => s.Number);

                    for (int i = nextSeasonAfterLastSeasonNumber; i < lastSeasonNumber; i++)
                    {
                        if (seasonsList.Exists(s => s.Number == i))
                        {
                            //retrieve index of season i
                            int index = seasonsList.IndexOf(seasonsList.First(s => s.Number == i));

                            foreach (Episode episode in seasonsList[index].EpisodesOfSeason)
                            {
                                App.User.TotalNbrEpisodesWatched += variation;
                                UpdateMinutesInTotalMinutesWatched(episode.DurationInMinutes, variation);
                            }
                        }                        
                    }


                    //Step 3 : all episodes until LastEpisodeWatched.Episode in LastEpisodeWached.Season

                    //retrieve LastEpisodeWathed.Season index
                    int indexSeason = seasonsList.FindIndex(season => season.Number == lastSeasonNumber);
                    
                    //start at first episode 
                    int minEpisode = seasonsList[indexSeason].EpisodesOfSeason.Min(s => s.Number);

                    for (int i = minEpisode; i < lastEpisodeNumber + 1; i++)
                    {
                        if (seasonsList[indexSeason].EpisodesOfSeason.Exists(e => e.Number == i))
                        {
                            App.User.TotalNbrEpisodesWatched += variation;
                            //retrieve index of episode
                            int index = seasonsList[indexSeason].EpisodesOfSeason.IndexOf(seasonsList[indexSeason].EpisodesOfSeason.First(e => e.Number == i));
                            UpdateMinutesInTotalMinutesWatched(seasonsList[indexSeason].EpisodesOfSeason[index].DurationInMinutes, variation);
                        }
                    }
                }
                MyUser = App.User;

                //Send Message to Firebase for update
                MessagingCenter.Send<BaseViewModel,User>(this, "UpdateUser", App.User);
            }
        }


        /// <summary>
        /// Increment or decrement TotalMinutesWatched (App.User attribute) with duration of an episode
        /// </summary>
        /// <param name="Duration"></param> duration of the episode
        /// <param name="increasingOrDecreasingVariation"></param> add or remove episode
        public void UpdateMinutesInTotalMinutesWatched(int Duration, int increasingOrDecreasingVariation)
        {

            App.User.TotalMinutesWatched += increasingOrDecreasingVariation * Duration;
        }

        /// <summary>
        /// When users increments one episode in Watchlist page, add this info in stats
        /// </summary>
        /// <param name="Duration"></param> duration of episode watched
        public void IncrementNumberOfEpisodesWatched(int Duration)
        {
            App.User.TotalNbrEpisodesWatched += 1;
            App.User.TotalMinutesWatched += Duration;
            MyUser = App.User;
            MessagingCenter.Send<BaseViewModel, User>(this, "UpdateUser", App.User);
        }

    }
}


 