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
        public PageScheduleShow ScheduleToday { get; set; } = new PageScheduleShow() { TitleDate = "On TV today" };
        public PageScheduleShow ScheduleTomorrow { get; set; } = new PageScheduleShow() { TitleDate = "On TV tomorrow" };
        public PageScheduleShow ScheduleAfterTomorrow { get; set; } = new PageScheduleShow() { TitleDate = "On TV after tomorrow" };

        public ObservableCollection<PageScheduleShow> SchedulesShows { get; set; } = new ObservableCollection<PageScheduleShow>();
        public List<Show> Favorites { get; set; }


        public void Init(){
            ScheduleToday.Clear();
            ScheduleTomorrow.Clear();
            ScheduleAfterTomorrow.Clear();
            SchedulesShows.Clear();
            Task.Run(() =>
            Device.BeginInvokeOnMainThread(async () =>
            {
                await ExecuteUpCommingCommand();
            }));
            Task.WaitAll();
        }

        public HomeUpcommingViewModel() : base()
        {
            Title = "Up Coming Shows";
        }

        public async Task ExecuteUpCommingCommand()
        {
            DateTime DateTime = DateTime.Now;
            string dateTimeToday = DateTime.ToString("yyyy-MM-dd");
            string dateTimeTomorrow = DateTime.AddDays(1).ToString("yyyy-MM-dd");
            string dateTimeAfterTomorrow = DateTime.AddDays(2).ToString("yyyy-MM-dd");
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

