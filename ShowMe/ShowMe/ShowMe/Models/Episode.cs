using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShowMe.Models
{
    public class Episode
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("summary")]
        public string Description { get; set; }

        [JsonProperty("season")]
        public int Season { get; set; }

        [JsonProperty("number")]
        public int Number { get; set; }

        [JsonProperty("image")]
        public Dictionary<string, string> Image { get; set; } = null;

        public string ImageMedium => (Image != null) ? (Image["medium"]).Replace("http", "https") : "";


        [JsonConstructor]
        public Episode(int season, int number)
        {
            Season = season;
            Number = number;
        }
    }
}
