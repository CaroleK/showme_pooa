using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;


namespace ShowMe.Models
{
    public class ScheduleShow
    {
        [JsonProperty("id")]
        public int IdEpisode { get; set; } = 0;

        [JsonProperty("name")]
        public string TitleEpisode { get; set; } = "No title";

        [JsonProperty("airdate")]
        public string Airdate { get; set; }

        [JsonProperty("airtime")]
        public string Airtime { get; set; }

        [JsonProperty("season")]
        public int Season { get; set; } = 0;

        [JsonProperty("number")]
        public int Number { get; set; } = 0;

        [JsonProperty("show")]
        public Show Show { get; set; }

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

