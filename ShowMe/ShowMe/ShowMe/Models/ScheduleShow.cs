using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;


namespace ShowMe.Models
{
    public class ScheduleShow
    {
        [JsonProperty("id")]
        public int ? IdEpisode { get; set; }

        [JsonProperty("name")]
        public string TitleEpisode { get; set; } = "No title";

        [JsonProperty("airtime")]
        public string Airtime { get; set; }

        [JsonProperty("show")]
        public Show Show { get; set; }
        
        public string TextInformation => Show.Title + " || " + TitleEpisode;
        public string DetailInformation => Airtime + " || " + Show.Network.NetworkName;

        public ScheduleShow() { 
        }

    }
    public class PageScheduleShow : ObservableCollection<ScheduleShow>
    {
        public string TitleDate { get; set; }
        public PageScheduleShow() : base()
        {
            
        }

    }
}

