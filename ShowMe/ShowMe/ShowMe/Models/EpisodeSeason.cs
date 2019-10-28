using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShowMe.Models
{
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

        public override bool Equals(object obj)
        {
            EpisodeSeason es = obj as EpisodeSeason;
            if (obj != null)
            {
                return ((EpisodeNumber == es.EpisodeNumber) && (SeasonNumber == es.SeasonNumber)); 
            }
            return base.Equals(obj);
        }
    }
}
