using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace ShowMe.Models
{
    public class Show
    {

        [JsonProperty("name")]
        public string Title { get; set; } = "No title";

        [JsonProperty("summary")]
        public string Description { get; set; } = "No description availble";

        [JsonProperty("image")]
        public Dictionary<string, string> Image { get; set; } = null;

        [JsonProperty("genres")]
        public string[] Genres { get; set; } = null;

        public string ImageMedium => (Image !=null) ? (Image["medium"]).Replace("http", "https") : "";

        public string GenresInString => ((Genres !=null)&&(Genres.Length > 0)) ? string.Join(", ", Genres) : "";

        public override string ToString()
        {
            return Title;
        }

        public Show(string title, string description)
        {
            Title = title;
            Description = description;
        }
    }
}
