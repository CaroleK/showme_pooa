using Newtonsoft.Json;
using ShowMe.ViewModels;
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

        // Last episode that the user watched for this show
        // Must be like {{"epidose",1},{"season",1}}
        [JsonProperty("LastEpisodeWatched")]
        public Dictionary<string, int> LastEpisodeWatched { get; set; }
        
        [JsonProperty("LastEpisodeWatchedInString")]
        public string LastEpisodeWatchedInString 
            => (LastEpisodeWatched != null) ? 
            "Last watched: S" + LastEpisodeWatched["season"] + "E" + LastEpisodeWatched["episode"] + (MyShowsViewModel.AreEpisodeDictionariesEqual(LastEpisode,LastEpisodeWatched) ?
                " (Finished)"
                : "")
            : "Not started watching";

        [JsonConstructor]
        public MyShow(int id, string title, string language, string[] genres, string url, string description, Dictionary<string, string> image , bool isFavorite, bool mustNotify, Dictionary<string, int> lastEpisode, Dictionary<string, int> lastEpisodeWatched)
        { 
            Id = id;
            Title = title;
            Language = language;
            Genres = genres;
            Url = url;
            Description = description;
            Image = image;
            LastEpisode = lastEpisode;

            IsFavorite = isFavorite;
            MustNotify = mustNotify;
            LastEpisodeWatched = lastEpisodeWatched;
        }

        public MyShow (Show show, bool isFavorite, bool mustNotify, Dictionary<string, int> lastEpisodeWatched)
        {            
            Id = show.Id;
            Title = show.Title;
            Language = show.Language;
            Genres = show.Genres;
            Url = show.Url;
            Description = show.Description;
            Image = show.Image;
            LastEpisode = show.LastEpisode;

            IsFavorite = isFavorite;
            MustNotify = mustNotify;
            LastEpisodeWatched = lastEpisodeWatched; 
        }

        public void IncrementLastEpisode()
        {
            //TODO
        }        
    }
}
