using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShowMe.Models
{
    class Season
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("number")]
        public int Number { get; set; }

        [JsonProperty("episodeOrder")]
        public int NumberOfEpisodes { get; set; }
    }
}
