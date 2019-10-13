using HtmlAgilityPack;
using Newtonsoft.Json;
using ShowMe.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace ShowMe.Models
{
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

        [JsonProperty("schedule")]
        public SearchSchedule Schedule { get; set; } 

        /*[JsonProperty("rating")]
        public double Rating { get; set; } = 0.0;*/

        [JsonProperty("summary")]
        public string Description { get; set; } = "No description availble";

        public string Summary { get { return ConvertDescriptionFromHtmlToString(this.Description); } set { } }
        
        [JsonProperty("image")]
        public Dictionary<string, string> Image { get; set; } = null;

        // Last episode that exists for this show
        // Must be like {{"epidose",1},{"season",1}}
        public Dictionary<string,int> LastEpisode { get; set; }

        public string ImageMedium => (Image != null) ? (Image["medium"]).Replace("http", "https") : "";

        public string GenresInString => ((Genres != null) && (Genres.Length > 0)) ? string.Join(", ", Genres) : "";

        public List<Episode> EpisodesList { get; set; }

        public override string ToString()
        {
            return Title;
        }

        static public bool AreEpisodeDictionariesEqual(Dictionary<string, int> dic1, Dictionary<string, int> dic2)
        {
            if ((dic1 != null) && (dic2 != null))
            {
                return ((dic1["episode"] == dic2["episode"]) && (dic1["season"] == dic2["season"]));
            }
            else
            {
                return false;
            }
        }

        public string ConvertDescriptionFromHtmlToString(string description)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(this.Description);
            string result = doc.DocumentNode.FirstChild.InnerHtml;
            result = Regex.Replace(result, "<.*?>", String.Empty);
            return result;
        }
    
    }
}
