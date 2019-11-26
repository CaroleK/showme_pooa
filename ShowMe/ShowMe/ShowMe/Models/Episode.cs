using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShowMe.Models
{
    /// <summary>
    /// Class that describes an Episode, as found in TVMazeAPI
    /// </summary>
    public class Episode
    {
        [JsonProperty("name")]
        public string Name { get; private set; }

        // Not used in this version, but could be useful for improvements
        [JsonProperty("summary")]
        private string Description { get; set; }

        [JsonProperty("season")]
        public int Season { get; private set; }

        [JsonProperty("number")]
        public int Number { get; private set; }

        [JsonProperty("image")]
        private Dictionary<string, string> Image { get; set; } = null;

        [JsonProperty ("runtime")]
        public int DurationInMinutes {get; private set;}

        // Retrieves the (safe) url of the medium-sized image
        public string ImageMedium => (Image != null) ? (Image["medium"]).Replace("http", "https") : "";


        [JsonConstructor]
        public Episode(int season, int number)
        {
            Season = season;
            Number = number;
        }
    }
}
