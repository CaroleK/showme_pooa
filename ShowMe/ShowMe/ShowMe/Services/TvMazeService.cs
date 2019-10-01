using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ShowMe.Models;

namespace ShowMe.Services
{
    class TvMazeService
    {
        HttpClient client;

        public TvMazeService()
        {
            client = new HttpClient();
        }

        public async Task<Show> GetSeriesAsync(string uri)
        {
            Show serie = null;
            try
            {
                HttpResponseMessage response = await client.GetAsync(new Uri(uri));
                if (response.IsSuccessStatusCode)
                {
                    string jsonString = await response.Content.ReadAsStringAsync();
                    serie = JsonConvert.DeserializeObject<Show>(jsonString);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\tERROR {0}", ex.Message); 
            }

            return serie;
        }

        public async Task<List<Show>> SearchSeriesAsync(string search)
        {
            List<Show> series = null;
            try
            {
                HttpResponseMessage response = await client.GetAsync(new Uri("https://api.tvmaze.com/search/shows?q="+search));
                if (response.IsSuccessStatusCode)
                {
                    string jsonString = await response.Content.ReadAsStringAsync();
                    List<SearchResult>  result = JsonConvert.DeserializeObject<List<SearchResult>>(jsonString);
                    series = new List<Show>();
                    for (int i = 0; i < Math.Min(result.Count, 10); i++)
                    {
                        series.Add(result[i].Serie);
                    }
                    Show s = series[0]; 
                }
            }
            catch (Exception ex)
            {
                return series;
            }

            return series;
        }
    }

    public class SearchResult
    {
        [JsonProperty("score")]
        public string Score { get; set; }

        [JsonProperty("show")]
        public Show Serie { get; set; }
    }
}
