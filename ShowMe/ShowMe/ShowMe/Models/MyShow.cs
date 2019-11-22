﻿using Newtonsoft.Json;
using ShowMe.Services;
using ShowMe.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
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
        // Must be like {{"episode",1},{"season",1}}
        [JsonProperty("LastEpisodeWatched")]
        public EpisodeSeason LastEpisodeWatched { get; set; }
        
        // Turns the LastEpisodeWatched dictionnary into a  readable string in the form "Not started watching", "S1E1" or "S2E24 (finished)"
        [JsonProperty("LastEpisodeWatchedInString")]
        public string LastEpisodeWatchedInString 
            => (LastEpisodeWatched != null) ? 
            "S" + LastEpisodeWatched.SeasonNumber + "E" + LastEpisodeWatched.EpisodeNumber + (LastEpisode.Equals(LastEpisodeWatched) ?
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
                EpisodeSeason NE = this.NextEpisode();
                if (NE == null)
                {
                    return "";
                }
                return "S" + NE.SeasonNumber + "E" + NE.EpisodeNumber;
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
        public MyShow(int id, string title, string language, string[] genres, string url, string description, Dictionary<string, string> image , bool isFavorite, bool mustNotify, EpisodeSeason lastEpisode, EpisodeSeason lastEpisodeWatched)
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
        public MyShow (Show show, bool isFavorite, bool mustNotify, EpisodeSeason lastEpisodeWatched)
        {            
            Id = show.Id;
            Title = show.Title;
            Language = show.Language;
            Genres = show.Genres;
            Url = show.Url;
            Description = show.Description;
            Image = show.Image;
            LastEpisode = show.LastEpisode;
            EpisodesList = show.EpisodesList;
            SeasonsList = show.SeasonsList;
            Cast = show.Cast;
            IsFavorite = isFavorite;
            MustNotify = mustNotify;
            LastEpisodeWatched = lastEpisodeWatched;
    
        }

        /// <summary>
        /// Finds the next episode to watch depending on this show's seasons list and episode list
        /// </summary>
        /// <returns>Null or a dictionnary of the {{"epidose",1},{"season",1}} format</returns>
        public EpisodeSeason NextEpisode()
        {
            EpisodeSeason currentLEW = this.LastEpisodeWatched;
            List<Season> seasonsList = this.SeasonsList;

            TvMazeService service = new TvMazeService();
            EpisodeSeason newLEW = new EpisodeSeason(1, 1);

            if (seasonsList == null)
            {
                return null;
            }
            if (currentLEW == null)
            {
                int minSeason = seasonsList.Min(s => s.Number);
                int indexSeason = seasonsList.FindIndex(season => season.Number == minSeason);
                int minEpisode = seasonsList[indexSeason].EpisodesOfSeason.Min(e => e.Number);
                int indexEpisode = seasonsList[indexSeason].EpisodesOfSeason.FindIndex(episode => episode.Number == minEpisode);
                newLEW.EpisodeNumber = minEpisode;
                newLEW.SeasonNumber = minSeason;
                newLEW.Duration = seasonsList[indexSeason].EpisodesOfSeason[indexEpisode].DurationInMinutes;
                return newLEW;
            }
            else if (currentLEW.Equals(this.LastEpisode))
            {
                return null;
            }
            else
            {
                int lastSeasonNumber = this.LastEpisodeWatched.SeasonNumber;
                int lastEpisodeNumber = this.LastEpisodeWatched.EpisodeNumber;

                if (currentLEW.EpisodeNumber == Season.FindSeasonBySeasonNumber(seasonsList, currentLEW.SeasonNumber).NumberOfEpisodes)
                {

                    int indexSeason = seasonsList.FindIndex(s => s.Number == currentLEW.SeasonNumber + 1);
                    int minEpisode = seasonsList[indexSeason].EpisodesOfSeason.Min(e => e.Number);
                    int indexEpisode = seasonsList[indexSeason].EpisodesOfSeason.FindIndex(e => e.Number == minEpisode);
                    newLEW.SeasonNumber = currentLEW.SeasonNumber + 1;
                    newLEW.EpisodeNumber = minEpisode; 
                    newLEW.Duration = seasonsList[indexSeason].EpisodesOfSeason[indexEpisode].DurationInMinutes;
                }
                else
                {

                    int indexSeason = seasonsList.FindIndex(s => s.Number == currentLEW.SeasonNumber);
                    int indexEpisode = seasonsList[indexSeason].EpisodesOfSeason.FindIndex(s => s.Number == currentLEW.EpisodeNumber + 1);
                    newLEW.SeasonNumber = currentLEW.SeasonNumber;
                    newLEW.EpisodeNumber = currentLEW.EpisodeNumber + 1;
                    newLEW.Duration = seasonsList[indexSeason].EpisodesOfSeason[indexEpisode].DurationInMinutes;
                }

                return newLEW;
            }
        }
    }
}
