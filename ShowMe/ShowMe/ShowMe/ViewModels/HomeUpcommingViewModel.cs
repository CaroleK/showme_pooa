using ShowMe.Models;
using ShowMe.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace ShowMe.ViewModels
{
    public class HomeUpcommingViewModel : BaseViewModel
    {
        TvMazeService service = new TvMazeService();
        public PageScheduleShow ScheduleToday { get; set; } = new PageScheduleShow() {TitleDate="Aujourd'hui"};
        public PageScheduleShow ScheduleTomorrow { get; set; } = new PageScheduleShow() { TitleDate = "Demain" };
        public PageScheduleShow ScheduleAfterTomorrow { get; set; } = new PageScheduleShow() { TitleDate = "Après-demain" };

        public ObservableCollection<PageScheduleShow> SchedulesShows { get; set; } = new ObservableCollection<PageScheduleShow>();
        public List<Show> Favorites { get; set; }




        public HomeUpcommingViewModel() : base()
        {
            Title = "Up Coming Shows";
            Task.Run(() =>
            Device.BeginInvokeOnMainThread(() =>
            {
            ExecuteUpCommingCommand();
            }));
            Task.WaitAll();
        }

        public async Task ExecuteUpCommingCommand()
        {
            DateTime dateTimeToday = DateTime.Now;
            DateTime dateTimeTomorrow = dateTimeToday.AddDays(1);
            DateTime dateTimeAfterTomorrow = dateTimeToday.AddDays(2);
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
            SchedulesShows.Add(ScheduleToday);

            List<ScheduleShow> sTomorrow = await service.GetUpCommingEpisode(Favorites, dateTimeTomorrow, regionISO);
            foreach (ScheduleShow schedule in sToday)
            {
                ScheduleTomorrow.Add(schedule);
            }
            SchedulesShows.Add(ScheduleTomorrow);

            List<ScheduleShow> sAfterTomorrow = await service.GetUpCommingEpisode(Favorites, dateTimeAfterTomorrow, regionISO);
            foreach (ScheduleShow schedule in sToday)
            {
                ScheduleAfterTomorrow.Add(schedule);
            }
            SchedulesShows.Add(ScheduleAfterTomorrow);

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
