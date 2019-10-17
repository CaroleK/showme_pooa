using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShowMe.Models
{
    public class Season
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("number")]
        public int Number { get; set; }

        [JsonProperty("episodeOrder")]
        public int NumberOfEpisodes { get; set; }

        public List<Episode> EpisodesOfSeason { get; set; }

        /// <summary>
        /// In a list of seasons, find the object with the required season number
        /// </summary>
        /// <param name="seasonList">The list of seasons</param>
        /// <param name="seasonNumber">The required season number</param>
        /// <returns>The matching season object</returns>
        static public Season FindSeasonBySeasonNumber(List<Season> seasonList, int seasonNumber)
        {
            foreach (Season s in seasonList)
            {
                if (s.Number == seasonNumber)
                {
                    return s;
                }
            }
            return null; 
        }
    }
}
