using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShowMe.Models
{
    /// <summary>
    /// Class that describes an actor, as found in TVMazeAPI
    /// </summary>
    public class Actor
    {
        [JsonProperty("name")]
        public string Name { get; private set; }

        [JsonProperty("image")]
        private Dictionary<string, string> Image { get; set; } = null;

        // Retrieves the (safe) url of the medium-sized image
        public string ImageMedium => (Image != null) ? (Image["medium"]).Replace("http", "https") : "";

    }
}
