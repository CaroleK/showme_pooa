using Newtonsoft.Json;
using ShowMe.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace ShowMe.Models
{
    /// <summary>
    /// Class that describes a show, as found in TVMazeAPI
    /// </summary>
    public class Show
    {
        [JsonProperty("id")]
        public int Id { get; set; } = 0;

        [JsonProperty("name")]
        public string Title { get; set; } = "No title";

        [JsonProperty("language")]
        public string Language { get; set; } = "No language";

        [JsonProperty("genres")]
        public string[] Genres { get; set; } = null;

        [JsonProperty("officialSite")]
        public string Url { get; set; } = "No site";

        [JsonProperty("network")]
        public SearchNetwork Network { get; set; }

        [JsonProperty("summary")]
        public string Description { get; set; } = "No description availble";

        public string Summary { get { return ConvertDescriptionFromHtmlToString(this.Description); } set { } }
        
        [JsonProperty("image")]
        public Dictionary<string, string> Image { get; set; } = null;



        // Last episode that exists for this show
        // Must be like {{"epidose",1},{"season",1}}
        public EpisodeSeason LastEpisode { get; set; }

        // Retrieves the (safe) url of the medium-sized image
        public string ImageMedium => ((Image != null)&&(Image["medium"] != null)) ? (Image["medium"]).Replace("http", "https") : "no_image_available.jpg";

        // Explode the array Genres into a string, with item separated by commas
        public string GenresInString => ((Genres != null) && (Genres.Length > 0)) ? string.Join(", ", Genres) : "";

        public List<Episode> EpisodesList { get; set; }

        public List<Season> SeasonsList { get; set; }

        public List<Actor> Cast { get; set; }

        public override string ToString()
        {
            return Title;
        }

        /// <summary>
        /// Parse the HTML description to prettier text
        /// </summary>
        /// <param name="description">The HTML-formatted text</param>
        /// <returns>The text without formatting and tags</returns>
        public string ConvertDescriptionFromHtmlToString(string description)
        {
            string result = Regex.Replace(description, "<.*?>", String.Empty);
            return result;
        }
    
    }
}
