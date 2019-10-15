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
    public class TvMazeService
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
                HttpResponseMessage responseShow = await client.GetAsync(new Uri("https://api.tvmaze.com/shows/" + i));
                if (responseShow.IsSuccessStatusCode)
                {
                    string jsonString = await responseShow.Content.ReadAsStringAsync();
                    show = JsonConvert.DeserializeObject<Show>(jsonString);

                    //retrieve last episode
                    //In order to make apprearance of Discovery page faster, put these calls in separate function @Laura ?
                    HttpResponseMessage responseSeason = await client.GetAsync(new Uri("https://api.tvmaze.com/shows/" + i + "/seasons"));
                    if (responseSeason.IsSuccessStatusCode)
                    {
                        string jsonStringSeason = await responseSeason.Content.ReadAsStringAsync();
                        List<Season> seasons = JsonConvert.DeserializeObject<List<Season>>(jsonStringSeason);
                        Season maxSeason = null;
                        foreach (Season season in seasons)
                        {
                            if ((maxSeason == null) || (season.Number > maxSeason.Number))
                            {
                                maxSeason = season;
                            }
                        }

                        show.LastEpisode = new Dictionary<string, int>{
                            { "episode", maxSeason.NumberOfEpisodes },
                            { "season", maxSeason.Number }
                        };
                    }
                }               

            }
            catch (Exception ex)
            {
                Debug.WriteLine("\tERROR {0}", ex.Message);
            }
            return show;
        }

        public async Task<List<Episode>> GetEpisodesListAsync(int ShowId)
        {
            List<Episode> EpisodesList = null;
            try
            {
                HttpResponseMessage responseShow = await client.GetAsync(new Uri("https://api.tvmaze.com/shows/" + ShowId + "/episodes"));
                if (responseShow.IsSuccessStatusCode)
                {
                    string jsonString = await responseShow.Content.ReadAsStringAsync();
                    EpisodesList = JsonConvert.DeserializeObject<List<Episode>>(jsonString);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\tERROR {0}", ex.Message);
            }
            return EpisodesList;
        }

        public async Task<List<Season>> GetSeasonsListAsync(int ShowId)
        {
            List<Season> SeasonsList = null;
            try
            {
                HttpResponseMessage responseShow = await client.GetAsync(new Uri("https://api.tvmaze.com/shows/" + ShowId + "/seasons"));
                if (responseShow.IsSuccessStatusCode)
                {
                    string jsonString = await responseShow.Content.ReadAsStringAsync();
                    SeasonsList = JsonConvert.DeserializeObject<List<Season>>(jsonString);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\tERROR {0}", ex.Message);
            }
            return SeasonsList;
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

        public async Task<List<Actor>> GetCastAsync(int ShowId)
        {
            List<Actor> actors = null;
            try
            {
                HttpResponseMessage response = await client.GetAsync(new Uri("https://api.tvmaze.com/shows/" + ShowId + "/cast"));
                if (response.IsSuccessStatusCode)
                {
                    string jsonString = await response.Content.ReadAsStringAsync();
                    List<SearchActor> result = JsonConvert.DeserializeObject<List<SearchActor>>(jsonString);
                    actors = new List<Actor>();
                    for (int i = 0; i < result.Count; i++)
                    {
                        actors.Add(result[i].Actor);
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO
            }
            return actors;
        }

        // Not clean method : get the schedule regardless of the favorites then select schedule only if they are part of favorite
        public async Task<List<ScheduleShow>> GetUpCommingEpisode(ObservableCollection<MyShow> MyShows, DateTime dateTime, string regionISO) 
        {
            List<ScheduleShow> scheduleShows = new List<ScheduleShow>();
                                 
            try
            {
                string Datetime = dateTime.ToString("yyy-MM-dd");
                Uri Url = new Uri("https://api.tvmaze.com/schedule?country=" + regionISO + "&date=" + Datetime);
                HttpResponseMessage response = await client.GetAsync(Url);
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonString = await response.Content.ReadAsStringAsync();
                        List<ScheduleShow> results = JsonConvert.DeserializeObject<List<ScheduleShow>>(jsonString);
                        
                        foreach (MyShow show in MyShows)
                        {
                            int id = show.Id; 
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


    public class SearchActor
    {
        [JsonProperty("person")]
        public Actor Actor { get; set; }
    }


    public class SearchSchedule
    {
        [JsonProperty("time")]
        public string Time { get; set; }

        [JsonProperty("days")]
        public List<string> Days { get; set; }
    }

    public class SearchNetwork
    {
        [JsonProperty("id")]
        public int NetworkId { get; set; }

        [JsonProperty("name")]
        public string NetworkName { get; set; }
    }
}
