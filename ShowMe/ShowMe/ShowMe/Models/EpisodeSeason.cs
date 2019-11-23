using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShowMe.Models
{
    /// <summary>
    /// Class that describes an episode really simply, with just its number, season and duration
    /// </summary>
    public class EpisodeSeason
    {
        [JsonProperty("episode")]
        public int EpisodeNumber { get; set; }
        [JsonProperty("season")]
        public int SeasonNumber { get; set; }
        public int Duration { get; set; }

        public EpisodeSeason(int episodeNumber, int seasonNumber)
        {
            EpisodeNumber = episodeNumber;
            SeasonNumber = seasonNumber;
        }

        /// <summary>
        /// Override of the Equals method for EpisodeSeason objects
        /// Two EpisodeSeason objects are equals if they have same episode number and season number
        /// </summary>
        /// <param name="obj">The object to compare to</param>
        /// <returns>True if objects are equals, false otherwise</returns>
        public override bool Equals(object obj)
        {
            EpisodeSeason es = obj as EpisodeSeason;
            if (obj != null)
            {
                return ((EpisodeNumber == es.EpisodeNumber) && (SeasonNumber == es.SeasonNumber)); 
            }
            return base.Equals(obj);
        }

        /// <summary>
        /// Determines whether a given EpisodeSeason comes after (meaning has higher episode number or season number) than this one
        /// </summary>
        /// <param name="obj">The object to compare to</param>
        /// <returns>True is given EpisodeSeason comes after, false otherwise</returns>
        public bool IsAfter(object obj)
        {
            EpisodeSeason es = obj as EpisodeSeason;
            try
            {
                if (SeasonNumber > es.SeasonNumber)
                {
                    return true;
                }
                else if (SeasonNumber < es.SeasonNumber){
                    return false;
                }
                else
                {
                    return (EpisodeNumber < es.EpisodeNumber)? false : true;
                }
            }
            catch (Exception) 
            {
                return false;
            }
            ;
        }

    }
}
