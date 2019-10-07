using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShowMe.Models
{
    public class Episode
    {
        [JsonProperty("Season")]
        public int Season { get; set; }
        [JsonProperty("Number")]
        public int Number { get; set; }
        [JsonProperty("ShowId")]
        public int ShowId { get; set; }

        [JsonConstructor]
        public Episode(int season, int number, int showId)
        {
            Season = season;
            Number = number;
            ShowId = showId;
        }
    }
}
