using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ShowMe.Models;
using System.Globalization;

namespace ShowMe.Services
{
    class TvMazeService
    {
        HttpClient client;

        public TvMazeService()
        {
            client = new HttpClient();
        }

        public async Task<Show> GetShowAsync(int i)
        {
            Show show = null;
            try
            {
                HttpResponseMessage response = await client.GetAsync(new Uri("https://api.tvmaze.com/shows/" + i));
                if (response.IsSuccessStatusCode)
                {
                    string jsonString = await response.Content.ReadAsStringAsync();
                    show = JsonConvert.DeserializeObject<Show>(jsonString);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\tERROR {0}", ex.Message);
            }

            return show;
        }

        public async Task<List<Show>> SearchShowAsync(string search)
        {
            List<Show> shows = null;
            try
            {
                HttpResponseMessage response = await client.GetAsync(new Uri("https://api.tvmaze.com/search/shows?q=" + search));
                if (response.IsSuccessStatusCode)
                {
                    string jsonString = await response.Content.ReadAsStringAsync();
                    List<SearchResult> result = JsonConvert.DeserializeObject<List<SearchResult>>(jsonString);
                    shows = new List<Show>();
                    for (int i = 0; i < Math.Min(result.Count, 10); i++)
                    {
                        shows.Add(result[i].Serie);
                    }
                    Show s = shows[0];
                }
            }
            catch (Exception ex)
            {
                //TODO
            }

            return shows;
        }

        // Not clean method : get the schedule regardless of the favorites then select schedule only if they are part of favorite
        public async Task<List<ScheduleShow>> GetUpCommingEpisode(ObservableCollection<MyShow> myShows, DateTime dateTime, string regionISO) 
        {
            List<ScheduleShow> scheduleShows = new List<ScheduleShow>();
                                 
            try
            {
                HttpResponseMessage response = await client.GetAsync(new Uri("https://api.tvmaze.com/schedule?country="+regionISO+"&date="+dateTime.ToString("yyy-MM-dd")));
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonString = await response.Content.ReadAsStringAsync();
                        List<ScheduleShow> results = JsonConvert.DeserializeObject<List<ScheduleShow>>(jsonString);
                        
                        foreach (MyShow myshow in myShows)
                        {
                            int id = myshow.Id; 
                            foreach (ScheduleShow r in results)
                            {
                                if (id == r.Show.Id)
                                {
                                    scheduleShows.Add(r);
                                }
                            }
                        
                        }
                    }
                    return (scheduleShows);
            }
            catch (Exception ex)
            {
                return(scheduleShows);
            }

           
        }
    }

    public class SearchResult
    {
        [JsonProperty("score")]
        public string Score { get; set; }

        [JsonProperty("show")]
        public Show Serie { get; set; }
    }

    public class SearchSchedule
    {
        [JsonProperty("time")]
        public string Time { get; set; }

        [JsonProperty("days")]
        public List<string> Days { get; set; }
    }
}
