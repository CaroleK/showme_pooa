using Newtonsoft.Json;
using ShowMe.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace ShowMe.Services
{
    public class TvMazeService
    {
        HttpClient client;

        public TvMazeService()
        {
            client = new HttpClient();
        }

        /// <summary>
        /// Retrieves json string response from API client
        /// </summary>
        /// <param name="url">The url of the API</param>
        /// <returns>Json data as string if response is successfull, null otherwise</returns>
        public async Task<string> FetchResponseFromAPI(string url)
        {
            string jsonString = null; 
            try
            {
                HttpResponseMessage responseShow = await client.GetAsync(new Uri(url));
                if (responseShow.IsSuccessStatusCode)
                {
                    jsonString = await responseShow.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\tERROR {0}", ex.Message);
            }
            return jsonString;
        }

        /// <summary>
        /// This function retrieves the show from TVMazeAPI with the given id, with all available details
        /// </summary>
        /// <param name="id">The id of the show</param>
        /// <returns>A task that returns a show with all available details in the API</returns>
        public async Task<Show> GetShowAsync(int id)
        {
            Show show = null;

            string jsonString = await FetchResponseFromAPI("https://api.tvmaze.com/shows/" + id);
            if (jsonString != null )
            {
                show = JsonConvert.DeserializeObject<Show>(jsonString);
            }
            
            return show;
        }

        
        /// <summary>
        /// This function retrieves the list of episodes from TVMazeAPI for the show with the given id
        /// </summary>
        /// <param name="ShowId">The id of the show</param>
        /// <returns>A task that returns a list of episodes with all available details in the API</returns>
        public async Task<List<Episode>> GetEpisodesListAsync(int ShowId)
        {
            List<Episode> EpisodesList = null;

            string jsonString = await FetchResponseFromAPI("https://api.tvmaze.com/shows/" + ShowId + "/episodes");
            if (jsonString != null)
            {
                EpisodesList = JsonConvert.DeserializeObject<List<Episode>>(jsonString);
            }
            
            return EpisodesList;
        }

        /// <summary>
        /// This function retrieves the list of seasons from TVMazeAPI for the show with the given id
        /// </summary>
        /// <param name="ShowId">The id of the show</param>
        /// <returns>A task that returns a list of seasons with all available details in the API</returns>
        public async Task<List<Season>> GetSeasonsListAsync(int ShowId)
        {
            List<Season> SeasonsList = null;

            string jsonString = await FetchResponseFromAPI("https://api.tvmaze.com/shows/" + ShowId + "/seasons");
            if (jsonString != null)
            {
                SeasonsList = JsonConvert.DeserializeObject<List<Season>>(jsonString);
            }
            
            return SeasonsList;
        }

        /// <summary>
        /// This function retrieves the list of shows from TVMazeAPI matching a string search for the title
        /// </summary>
        /// <param name="search">The string search</param>
        /// <returns>A task that returns a list of shows that match the search, by order of relevance</returns>
        public async Task<List<Show>> SearchShowAsync(string search)
        {
            List<Show> shows = null;

            string jsonString = await FetchResponseFromAPI("https://api.tvmaze.com/search/shows?q=" + search);
            if(jsonString != null)
            {
                List<SearchResult> result = JsonConvert.DeserializeObject<List<SearchResult>>(jsonString);
                shows = new List<Show>();
                foreach (SearchResult searchresult in result)
                {
                    shows.Add(searchresult.Serie);
                }
            }
                    
            return shows;            
        }

        /// <summary>
        /// This function retrieves the list of actors from TVMazeAPI for the show with the given id
        /// </summary>
        /// <param name="ShowId">The id of the show</param>
        /// <returns>A task that returns a list of actors with all available details in the API</returns>
        public async Task<List<Actor>> GetCastAsync(int ShowId)
        {
            List<Actor> actors = null;

            string jsonString = await FetchResponseFromAPI("https://api.tvmaze.com/shows/" + ShowId + "/cast");
            if (jsonString != null)
            {
                List<SearchActor> result = JsonConvert.DeserializeObject<List<SearchActor>>(jsonString);
                actors = new List<Actor>();
                for (int i = 0; i < result.Count; i++)
                {
                    actors.Add(result[i].Actor);
                }
            }

            return actors;
        }

        /// <summary>
        /// This function retrieves the list of episodes from TVMazeAPI airing of the given date in the given country for shows in a given list.
        /// </summary>
        /// <param name="MyShows">The collection of shows from which we want to retrieve airing episodes</param>
        /// <param name="dateTime">he airing date</param>
        /// <param name="regionISO">The airing country</param>
        /// <returns></returns>
        public async Task<List<ScheduleShow>> GetUpCommingEpisode(ObservableCollection<MyShow> MyShows, string dateTime, string regionISO)
        {
            List<ScheduleShow> scheduleShows = new List<ScheduleShow>();

            string jsonString = await FetchResponseFromAPI("https://api.tvmaze.com/schedule?country=" + regionISO + "&date=" + dateTime);
            if (jsonString != null)
            {
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

            return scheduleShows;
        }
    }

    /// <summary>
    /// A class used only to deserialize results from TVMazeAPI search by title queries.  
    /// </summary>
    public class SearchResult
    {
        [JsonProperty("score")]
        public string Score { get; set; }

        [JsonProperty("show")]
        public Show Serie { get; set; }
    }

    /// <summary>
    /// A class used only to deserialize results from TVMazeAPI search actor queries.  
    /// </summary>
    public class SearchActor
    {
        [JsonProperty("person")]
        public Actor Actor { get; set; }
    }

    /// <summary>
    /// A class used only to deserialize results from TVMazeAPI search schedule queries.  
    /// </summary>

    public class SearchNetwork
    {

        [JsonProperty("name")]
        public string NetworkName { get; set; }
    }
}
