using ShowMe.Models;
using ShowMe.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;

namespace ShowMe.ViewModels
{
    public class HomeUpcommingViewModel : BaseViewModel
    {
        TvMazeService service = new TvMazeService();
        public ObservableCollection<ScheduleShow> ScheduleToday { get; set; } = new ObservableCollection<ScheduleShow>();
        public List<Show> Favorites { get; set; }



        public HomeUpcommingViewModel() : base()
        {
            Title = "Up Coming Shows";
            Task.Run(() => ExecuteUpCommingCommand());
            Task.WaitAll();
        }

        public async Task ExecuteUpCommingCommand()
        {
            DateTime dateTimeToday = DateTime.Now;
            RegionInfo region = new RegionInfo("FR");
            string regionISO = region.TwoLetterISORegionName;
            Favorites = await FavoriteList();
            /*List<MyShow> myShowsUpcomming = new List<MyShow>();
            foreach(MyShow myshow in MyShows)
            {
                myShowsUpcomming.Add(myshow);
            }*/

            List<ScheduleShow> sToday = await service.GetUpCommingEpisode(Favorites, dateTimeToday, regionISO);
            foreach (ScheduleShow schedule in sToday)
            {
                ScheduleToday.Add(schedule);
            }
            
        }

        public async Task<List<Show>> FavoriteList()

        {
            List<Show> Favorites = new List<Show>();

            List<int> favoritesid = new List<int> { 38681, 38991, 26950, 38680, 34409};

            foreach (int i in favoritesid)
            {
                Show s = await service.GetShowAsync(i);
                if (s != null)
                {
                    Favorites.Add(s);
                }
            }
            return (Favorites);

        }
    }
}
