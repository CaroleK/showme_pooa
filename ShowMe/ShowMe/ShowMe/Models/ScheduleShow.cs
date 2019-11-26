using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;


namespace ShowMe.Models
{
    /// <summary>
    /// Class that describes the schedule of an Episode, as found in TVMazeAPI
    /// </summary>
    public class ScheduleShow
    {
        [JsonProperty("id")]
        public int ? IdEpisode { get; private set; }

        [JsonProperty("name")]
        public string TitleEpisode { get; private set; } = "No title";

        [JsonProperty("airtime")]
        public string Airtime { get; private set; }

        [JsonProperty("airdate")]
        public string Airdate { get; private set; }

        [JsonProperty("show")]
        public Show Show { get; private set; }
        
        public string TextInformation => Show.Title + " - " + TitleEpisode;
        public string DetailInformation => Airtime + " || " + Show.Network.NetworkName;
    }

    /// <summary>
    /// Class that describes a list of ScheduleShows
    /// Useful to describe all shows that will air of a given date and display it with headers in ListViews
    /// </summary>
    public class PageScheduleShow : ObservableCollection<ScheduleShow>
    {
        public string TitleDate { get; set; }
        public PageScheduleShow() : base()
        {
            
        }

    }
}

