using Newtonsoft.Json;
using ShowMe.Services;
using ShowMe.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ShowMe.Models
{
    /// <summary>
    /// A class that describes a user, as found in Google+ API
    /// </summary>
    [JsonObject]
    public class User
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("picture")]
        public string Picture { get; set; }

        /// <summary>
        /// Statistics parameters displays in the profile page
        /// </summary>
        public int TotalNbrEpisodesWatched { get; set; }

        public int TotalMinutesWatched { get; set; }

        public int daysWatched => (TotalMinutesWatched / 1440);
        public int hoursWatchedWhenSubstractingDays => (TotalMinutesWatched % 1440) / 60;
        public int minutesWatchedWhenSubstractingDaysAndHours => ((TotalMinutesWatched % 1440) % 60);

    }
}

       

       
