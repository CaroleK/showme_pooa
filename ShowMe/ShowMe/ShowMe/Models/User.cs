using Newtonsoft.Json;
using ShowMe.Services;
using ShowMe.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ShowMe.Models
{
    /// <summary>
    /// A class that describes a user, as found in Google+ API
    /// </summary>
    [JsonObject]
    public class User
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("verified_email")]
        public bool VerifiedEmail { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("given_name")]
        public string GivenName { get; set; }

        [JsonProperty("family_name")]
        public string FamilyName { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("picture")]
        public string Picture { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        /// <summary>
        /// Statistics parameters displays in the profile page
        /// </summary>
        public int TotalNbrEpisodesWatched { get; set; }

        public int  TotalMinutesWatched { get; set; }

        public int daysWatched => (TotalMinutesWatched / 1440);
        public int hoursWatchedWhenSubstractingDays => (TotalMinutesWatched % 1440) / 60;
        public int minutesWatchedWhenSubstractingDaysAndHours => ((TotalMinutesWatched % 1400) % 60);

        public User()
        {
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
                IncrementEpisodeInNumberOfEpisodesWatched(item.LastEpisodeWatched.Duration);
            });
        }

        private void UpdateNumberOfEpisodesWatched(EpisodeSeason lastEpisodeWatched, List<Season> seasonsList, bool AddingEpisodes)
        {
            int variation = AddingEpisodes ? 1 : -1;

            if (lastEpisodeWatched != null)
            {
                TotalNbrEpisodesWatched = App.User.TotalNbrEpisodesWatched;

                int lastSeasonNumber = lastEpisodeWatched.SeasonNumber;
                int lastEpisodeNumber = lastEpisodeWatched.EpisodeNumber;

                for (int i = 1; i < lastSeasonNumber; i++)
                {
                    int index = seasonsList.IndexOf(seasonsList.First(s => s.Number == i));
                    foreach (Episode episode in seasonsList[index].EpisodesOfSeason)
                    {
                        TotalNbrEpisodesWatched += variation;
                        UpdateMinutesInTotalMinutesWatched(episode.DurationInMinutes, variation);
                    }
                }
                int indexSeason = seasonsList.IndexOf(seasonsList.First(s => s.Number == lastSeasonNumber));
                for (int i = 1; i < lastEpisodeNumber + 1; i++)
                {
                    TotalNbrEpisodesWatched += variation;
                    int index = seasonsList[indexSeason].EpisodesOfSeason.IndexOf(seasonsList[indexSeason].EpisodesOfSeason.First(e => e.Number == i));
                    UpdateMinutesInTotalMinutesWatched(seasonsList[indexSeason].EpisodesOfSeason[index].DurationInMinutes, variation);
                }
                //Send Message to Firebase for update
                MessagingCenter.Send(this, "UpdateUser", App.User);
            }
        }


        public void UpdateMinutesInTotalMinutesWatched(int Duration, int increasingOrDecreasingVariation)
        {

            App.User.TotalMinutesWatched += increasingOrDecreasingVariation*Duration;
        }

        public void IncrementEpisodeInNumberOfEpisodesWatched(int Duration)
        {
            App.User.TotalNbrEpisodesWatched += 1;
            App.User.TotalMinutesWatched +=  Duration;
            //MessagingCenter.Send(this, "UpdateUser", App.User);
        }

    }
}

