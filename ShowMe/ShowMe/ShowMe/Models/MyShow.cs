using Newtonsoft.Json;
using ShowMe.Services;
using ShowMe.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShowMe.Models
{
    /// <summary>
    /// Class that describes a show that's part of a user's list
    /// Inherits from class Show, with additionnal attributes 
    /// </summary>
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
        
        // Turns the LastEpisodeWatched dictionnary into a  readable string in the form "Not started watching", "S1E1" or "S2E24 (finished)"
        [JsonProperty("LastEpisodeWatchedInString")]
        public string LastEpisodeWatchedInString 
            => (LastEpisodeWatched != null) ? 
            "S" + LastEpisodeWatched["season"] + "E" + LastEpisodeWatched["episode"] + (AreEpisodeDictionariesEqual(LastEpisode,LastEpisodeWatched) ?
                " (Finished)"
                : "")
            : "Not started watching";

        // Adds "Last Watched: " at the beginning of LastEpisodeWatchedInString, useful for MyShowsPage 
        [JsonProperty("LastEpisodeWatchedInFullString")]
        public string LastEpisodeWatchedInFullString => "Last watched: " + LastEpisodeWatchedInString; 
        
        // The next episode the user should watch
        public string NextEpisodeInString {
            get
            {
                Dictionary<string, int> NE = this.NextEpisode();
                if (NE == null)
                {
                    return "";
                }
                return "S" + NE["season"] + "E" + NE["episode"];
            }
        }

        /// <summary>
        /// Constructor to build MyShow with all attributes
        /// Can be used by the desiarization of TVMazeAPI json data
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <param name="language"></param>
        /// <param name="genres"></param>
        /// <param name="url"></param>
        /// <param name="description"></param>
        /// <param name="image"></param>
        /// <param name="isFavorite"></param>
        /// <param name="mustNotify"></param>
        /// <param name="lastEpisode"></param>
        /// <param name="lastEpisodeWatched"></param>
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

        /// <summary>
        /// Constructor to build MyShow from a Show, with all additionnal attributes
        /// </summary>
        /// <param name="show"></param>
        /// <param name="isFavorite"></param>
        /// <param name="mustNotify"></param>
        /// <param name="lastEpisodeWatched"></param>
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

        /// <summary>
        /// Finds the next episode to watch depending on this show's seasons list and episode list
        /// </summary>
        /// <returns>Null or a dictionnary of the {{"epidose",1},{"season",1}} format</returns>
        public Dictionary<string, int> NextEpisode()
        {
            Dictionary<string, int> currentLEW = this.LastEpisodeWatched;
            if (Show.AreEpisodeDictionariesEqual(currentLEW, this.LastEpisode))
            {
                return null;
            }
            else
            {
                TvMazeService service = new TvMazeService();
                Dictionary<string, int> newLEW = new Dictionary<string, int> { { "episode", 1 }, { "season", 1 } };
                List<Season> seasonsList = Task.Run(() => service.GetSeasonsListAsync(this.Id)).Result;
                if (seasonsList == null)
                {
                    return null;
                }
                if (currentLEW["episode"] == Season.FindSeasonBySeasonNumber(seasonsList, currentLEW["season"]).NumberOfEpisodes)
                {
                    newLEW["season"] = currentLEW["season"] + 1;
                }
                else
                {
                    newLEW["season"] = currentLEW["season"];
                    newLEW["episode"] = currentLEW["episode"] + 1;
                }

                return newLEW;
            }
        }
    }
}
