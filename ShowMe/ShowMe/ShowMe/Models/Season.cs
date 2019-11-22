using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShowMe.Models
{
    /// <summary>
    /// Class that describes a season, as found in TVMazeAPI
    /// </summary>
    public class Season
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("number")]
        public int Number { get; set; }

        [JsonProperty("episodeOrder")]
        public int ? NumberOfEpisodes { get; set; }

        public List<Episode> EpisodesOfSeason { get; set; }
    }
}
