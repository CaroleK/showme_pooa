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

        public override bool Equals(object obj)
        {
            EpisodeSeason es = obj as EpisodeSeason;
            if (obj != null)
            {
                return ((EpisodeNumber == es.EpisodeNumber) && (SeasonNumber == es.SeasonNumber)); 
            }
            return base.Equals(obj);
        }

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

        public int IsApartInNumberOfEpisodes(EpisodeSeason es, Show show)
        {
            int difference = 0;
            int AddOrRemove = 1;
            if (SeasonNumber == es.SeasonNumber)
            {
                difference = EpisodeNumber - es.EpisodeNumber;
            }
            
            else 
            {
                if (SeasonNumber < es.SeasonNumber)
                {
                    int tempSeasonNumber = SeasonNumber;
                    int tempEpisodeNumber = EpisodeNumber;
                    SeasonNumber = es.SeasonNumber;
                    EpisodeNumber = es.EpisodeNumber;
                    es.SeasonNumber = tempSeasonNumber;
                    es.EpisodeNumber = tempEpisodeNumber;
                    AddOrRemove = -1;
                }

                difference += (int) show.SeasonsList[es.SeasonNumber].NumberOfEpisodes - es.EpisodeNumber;
                
                if (!(SeasonNumber == es.SeasonNumber +1))
                {
                    for (int s = es.SeasonNumber + 1; s < SeasonNumber; s++)
                    {
                        difference += (int)show.SeasonsList[s].NumberOfEpisodes;
                    }
                }

                difference += EpisodeNumber;
                
            }
            return difference*AddOrRemove;
        }
    }
}
