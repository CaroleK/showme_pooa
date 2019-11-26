using Newtonsoft.Json;
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
        [JsonProperty("LastEpisodeWatchedInFullString")]
        public string LastEpisodeWatchedInFullString 
            => (LastEpisodeWatched != null) ? 
            "Last watched: S" + LastEpisodeWatched.SeasonNumber + "E" + LastEpisodeWatched.EpisodeNumber + (LastEpisode.Equals(LastEpisodeWatched) ?
                " (Finished)"
                : "")
            : "Not started watching";

        public EpisodeSeason FirstEpisodeToWatch { get { return GetFirstEpisodeSeason(); } set { } }

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
        /// Can be used by the deserialization of TVMazeAPI json data
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
        public MyShow(int id, string title, string language, string[] genres, string description, Dictionary<string, string> image , bool isFavorite, bool mustNotify, EpisodeSeason lastEpisode, EpisodeSeason lastEpisodeWatched)
        { 
            Id = id;
            Title = title;
            Language = language;
            Genres = genres;
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
        /// <returns>Null or a dictionnary of the {{"episode",1},{"season",1}} format</returns>
        public EpisodeSeason NextEpisode()
        {
            // Where the user left off
            EpisodeSeason currentLEW = this.LastEpisodeWatched;

            // Seasons List
            List<Season> seasonsList = this.SeasonsList;

            TvMazeService service = new TvMazeService();

            // First episode
            EpisodeSeason newLEW = GetFirstEpisodeSeason();
            FirstEpisodeToWatch = newLEW;

            // Shouldn't happen
            if (seasonsList == null)
            {
                return null;
            }
            // If user has not started watching, return firt episode of show 
            if (currentLEW == null)
            {
                return newLEW;
            }
            // If last episode watched is last episode of show, return null
            else if (currentLEW.Equals(this.LastEpisode))
            {
                return null;
            }
            else
            {
                // Where the user left off
                int lastSeasonNumber = currentLEW.SeasonNumber;
                int lastEpisodeNumber = currentLEW.EpisodeNumber;
                int indexLastSeason = seasonsList.FindIndex(s => s.Number == lastSeasonNumber);

                // If user left off at last episode of a season (ie maximum episode number), move on to next season
                if (lastEpisodeNumber == seasonsList[indexLastSeason].EpisodesOfSeason.Max(e => e.Number))
                {
                    // The next season number is the minimum season number that is greater than current season number
                    newLEW.SeasonNumber = seasonsList.FindAll(s => s.Number > lastSeasonNumber).Min(s => s.Number);
                    int indexSeason = seasonsList.FindIndex(s => s.Number == newLEW.SeasonNumber);

                    // The first episode of this season is the one with minimum number
                    int minEpisode = seasonsList[indexSeason].EpisodesOfSeason.Min(e => e.Number);                 
                    int indexEpisode = seasonsList[indexSeason].EpisodesOfSeason.FindIndex(e => e.Number == minEpisode);
                    newLEW.EpisodeNumber = minEpisode; 

                    // Store duration
                    newLEW.Duration = seasonsList[indexSeason].EpisodesOfSeason[indexEpisode].DurationInMinutes;
                }
                // The user left off somewhere in the middle of a season
                else
                {
                    // Still in the same season                    
                    newLEW.SeasonNumber = currentLEW.SeasonNumber;

                    // The next episode number is the minimum episode number that is greater than current episode number
                    newLEW.EpisodeNumber = seasonsList[indexLastSeason].EpisodesOfSeason.FindAll(e => e.Number > lastEpisodeNumber).Min(e => e.Number);
                    int indexEpisode = seasonsList[indexLastSeason].EpisodesOfSeason.FindIndex(s => s.Number == newLEW.EpisodeNumber);
                    
                    // Store duration
                    newLEW.Duration = seasonsList[indexLastSeason].EpisodesOfSeason[indexEpisode].DurationInMinutes;
                }

                return newLEW;
            }
        }

        /// <summary>
        /// Computes the first episode of show (minimal season and minimal episode)
        /// </summary>
        /// <returns>An EpisodeSeason object representing first episode of show</returns>
        private EpisodeSeason GetFirstEpisodeSeason()
        {
            List<Season> seasonsList = this.SeasonsList;

            if (this.SeasonsList == null)
            {
                return null;
            }
            else
            {
                int minSeason = seasonsList.Min(s => s.Number);
                int indexSeason = seasonsList.FindIndex(season => season.Number == minSeason);
                int minEpisode = seasonsList[indexSeason].EpisodesOfSeason.Min(e => e.Number);
                int indexEpisode = seasonsList[indexSeason].EpisodesOfSeason.FindIndex(episode => episode.Number == minEpisode);

                EpisodeSeason FirstEpisodeSeason = new EpisodeSeason(minEpisode, minSeason);
                FirstEpisodeSeason.Duration = seasonsList[indexSeason].EpisodesOfSeason[indexEpisode].DurationInMinutes;

                return FirstEpisodeSeason;
            }
        }
    }
}
