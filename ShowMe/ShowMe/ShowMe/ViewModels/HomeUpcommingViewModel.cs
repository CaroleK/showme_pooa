using ShowMe.Models;
using ShowMe.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace ShowMe.ViewModels
{
    public class HomeUpcommingViewModel : BaseViewModel
    {
        TvMazeService service = new TvMazeService();
        public PageScheduleShow ScheduleToday { get; set; } = new PageScheduleShow() { TitleDate = "Shows to watch Today :" };
        public PageScheduleShow ScheduleTomorrow { get; set; } = new PageScheduleShow() { TitleDate = "Shows to watch Tomorrow :" };
        public PageScheduleShow ScheduleAfterTomorrow { get; set; } = new PageScheduleShow() { TitleDate = "Shows to watch After Tomorrow : " };

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
            // Choice made to focus on US region because of the API's data 
            string regionISO = "US";

            List<ScheduleShow> sToday = await service.GetUpCommingEpisode(MyShowsCollection.Instance, dateTimeToday, regionISO);
            foreach (ScheduleShow schedule in sToday)
            {
                ScheduleToday.Add(schedule);
            }
            SchedulesShows.Add(ScheduleToday);

            List<ScheduleShow> sTomorrow = await service.GetUpCommingEpisode(MyShowsCollection.Instance, dateTimeTomorrow, regionISO);
            foreach (ScheduleShow schedule in sTomorrow)
            {
                ScheduleTomorrow.Add(schedule);
            }
            SchedulesShows.Add(ScheduleTomorrow);

            List<ScheduleShow> sAfterTomorrow = await service.GetUpCommingEpisode(MyShowsCollection.Instance, dateTimeAfterTomorrow, regionISO);
            foreach (ScheduleShow schedule in sAfterTomorrow)
            {
                ScheduleAfterTomorrow.Add(schedule);
            }
           SchedulesShows.Add(ScheduleAfterTomorrow);

        }



    }
}

