using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShowMe.Models
{
    public class Actor
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("image")]
        public Dictionary<string, string> Image { get; set; } = null;

        public string ImageMedium => (Image != null) ? (Image["medium"]).Replace("http", "https") : "";

    }
}
