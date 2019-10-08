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
        public ObservableCollection<ScheduleShow> ScheduleToday { get; set; }
        public ObservableCollection<Show> Favorites { get; set; }



        public HomeUpcommingViewModel() : base()
        {
            Title = "Up Coming Shows";
            Task.Run(() => ExecuteUpCommingCommand());
        }

        public async Task ExecuteUpCommingCommand()
        {
            DateTime dateTimeToday = DateTime.Now;
            RegionInfo region = new RegionInfo("FR");
            string regionISO = region.TwoLetterISORegionName;

            List<ScheduleShow> sToday = await service.GetUpCommingEpisode(MyShows, dateTimeToday, regionISO);
            foreach (ScheduleShow schedule in sToday)
            {
                ScheduleToday.Add(schedule);
            }
        }

        public async Task FavoriteList()

        {
            List<int> favoritesid = new List<int> { 38681, 38991, 26950, 38680, 34409, 16505, 5871, 24793 };

            foreach (int i in favoritesid)
            {
                Show s = await service.GetShowAsync(i);
                if (s != null)
                {
                    Favorites.Add(s);
                }
            }

        }
    }
}
