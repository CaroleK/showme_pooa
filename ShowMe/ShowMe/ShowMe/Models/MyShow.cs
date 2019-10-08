using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShowMe.Models
{
    public class MyShow : Show
    {
        [JsonProperty("IsFavorite")]
        public bool IsFavorite { get; set; }
        
        [JsonProperty("MustNotify")]
        public bool MustNotify { get; set; }
        
        [JsonProperty("LastEpisode")]
        public Dictionary<string, int> LastEpisodeWatched { get; set; }
        
        [JsonProperty("LastEpisodeInString")]
        public string LastEpisodeInString => (LastEpisode != null) ? "Last watched: S" + LastEpisode["season"] + "E" + LastEpisode["episode"] : "Not started watching";

        [JsonConstructor]
        public MyShow(int id, string title, string language, string[] genres, string url, string description, Dictionary<string, string> image , bool isFavorite, bool mustNotify, Dictionary<string, int> lastEpisode)
        { 
            Id = id;
            Title = title;
            Language = language;
            Genres = genres;
            Url = url;
            Description = description;
            Image = image;

            IsFavorite = isFavorite;
            MustNotify = mustNotify;
            LastEpisodeWatched = lastEpisode;
        }

        public MyShow (Show show, bool isFavorite, bool mustNotify, Dictionary<string, int> lastEpisode)
        {            
            Id = show.Id;
            Title = show.Title;
            Language = show.Language;
            Genres = show.Genres;
            Url = show.Url;
            Description = show.Description;
            Image = show.Image;
            
            IsFavorite = isFavorite;
            MustNotify = mustNotify;
            LastEpisodeWatched = lastEpisode; 
        }

        public void IncrementLastEpisode()
        {
            //TODO
        }        
    }
}
