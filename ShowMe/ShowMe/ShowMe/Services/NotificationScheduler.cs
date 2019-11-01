using Plugin.LocalNotifications;
using ShowMe.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace ShowMe.Services
{
    public class NotificationScheduler
    {
        /// <summary>
        /// Schedules notifications 24 hours before a show present in the user's list airs on US TV
        /// </summary>
        /// <param name="userId">The user id</param>
        static public void ScheduleNotification(string userId)
        {
            DateTime DateTime = DateTime.Now;

            ObservableCollection<MyShow> myShows = new ObservableCollection<MyShow>(Task.Run(() => FireBaseHelper.GetUserShowList(userId)).Result);
            List<ScheduleShow> scheduledShows = RetrieveScheduledShows(myShows); 

            foreach (ScheduleShow schedule in scheduledShows)
            {
                if (GetByIdFromMyShows(myShows, schedule.Show.Id).MustNotify)
                {
                    string notificationIdString = schedule.Show.Id + "" + DateTime.Day + "" + DateTime.Month;
                    int notificationId = int.Parse(notificationIdString);

                    string notificationBody = schedule.TitleEpisode + " -  on " + schedule.Show.Network.NetworkName + " at " + schedule.Airtime;
                    string notificationTitle = "Don't miss it! " + schedule.Show.Title + "is on TV tomorrow";

                    string[] airtime = schedule.Airtime.Split(':');
                    DateTime notificationTime = new DateTime(DateTime.Year, DateTime.Month, DateTime.Day + 1, int.Parse(airtime[0]), int.Parse(airtime[1]), 0);
                    
                    CrossLocalNotifications.Current.Show(notificationTitle, notificationBody, notificationId, notificationTime);                    
                }
            }            
        }

        /// <summary>
        /// Retrieves the schedule of relevant shows that are airing tomorrow and the day after in the US, to be able to pln notifications
        /// </summary>
        /// <param name="myShows">The user's shows list</param>
        /// <returns>A list of ScheduleShow with airing details</returns>
        static public List<ScheduleShow> RetrieveScheduledShows(ObservableCollection<MyShow> myShows)
        {
            TvMazeService service = new TvMazeService();
            DateTime DateTime = DateTime.Now;

            CrossLocalNotifications.Current.Show("debug", "Woke up at " + DateTime.ToShortTimeString(), 0, DateTime);

            // We check what's on TV tomorrow and in two days, to be able to schedule notifications 24h before
            string dateTimeTomorrow = DateTime.AddDays(1).ToString("yyyy-MM-dd");
            string dateTimeAfterTomorrow = DateTime.AddDays(2).ToString("yyyy-MM-dd");

            // Choice made to focus on US region because of the API's data 
            string regionISO = "US";

            List<ScheduleShow> sTomorrow = Task.Run(() => service.GetUpCommingEpisode(myShows, dateTimeAfterTomorrow, regionISO)).Result;
            List<ScheduleShow> sAfterTomorrow = Task.Run(() => service.GetUpCommingEpisode(myShows, dateTimeAfterTomorrow, regionISO)).Result;
            sTomorrow.AddRange(sAfterTomorrow);

            return sTomorrow; 
        }


        /// <summary>
        /// Retrieves a show MyShow from the Instance of the user's MyShow list by its ID
        /// </summary>
        /// <param name="id">The ID of the MyShow to retrieve</param>
        /// <returns>The matching MyShow, or null if no show matches the ID</returns>
        static protected MyShow GetByIdFromMyShows(ObservableCollection<MyShow> myShows, int id)
        {
            foreach (MyShow myShow in myShows)
            {
                if (myShow.Id == id)
                {
                    return myShow;
                }
            }

            // if no matching show was found, return null
            return null;
        }
    }
}
