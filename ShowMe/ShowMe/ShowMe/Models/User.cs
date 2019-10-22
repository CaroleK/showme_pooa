using Newtonsoft.Json;
using ShowMe.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public async Task AddMinutestoTotalMinutesWatched(Dictionary<string,int> lastEpisodeWatched, List<Season> seasonsList)
        {
            int lastSeasonNumber = lastEpisodeWatched["season"];
            int lastEpisodeNumber = lastEpisodeWatched["episode"];
            
            for (int i=1; i<lastSeasonNumber; i++)
            {
                int index = seasonsList.IndexOf(seasonsList.First(s => s.Number == i));
                foreach (Episode episode in seasonsList[index].EpisodesOfSeason)
                {
                    TotalMinutesWatched += episode.DurationInMinutes;
                }

            }
            int indexSeason = seasonsList.IndexOf(seasonsList.First(s => s.Number == lastSeasonNumber));
            for (int i=1; i<lastEpisodeNumber+1; i++)
            {
                int index = seasonsList[indexSeason].EpisodesOfSeason.IndexOf(seasonsList[indexSeason].EpisodesOfSeason.First(e => e.Number == i));
                TotalMinutesWatched += seasonsList[indexSeason].EpisodesOfSeason[index].DurationInMinutes;
            }

            await FireBaseHelper.ModifyUser(Id, TotalMinutesWatched);
        }
    }
}

