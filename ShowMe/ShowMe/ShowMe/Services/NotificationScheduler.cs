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
        static public void ScheduleNotification(string userId)
        {
            TvMazeService service = new TvMazeService();

            ObservableCollection<MyShow> myShows = new ObservableCollection<MyShow>(Task.Run(() => FireBaseHelper.GetUserShowList(userId)).Result);
            DateTime DateTime = DateTime.Now;
            // We check what's on TV in two days, to be able to schedule notifications for tomorrow
            string dateTimeAfterTomorrow = DateTime.Year + "-" + DateTime.Month + "-" + (DateTime.Day + 2);
            // Choice made to focus on US region because of the API's data 
            string regionISO = "US";
            
            List<ScheduleShow> sAfterTomorrow = Task.Run(() => service.GetUpCommingEpisode(myShows, dateTimeAfterTomorrow, regionISO)).Result;
            foreach (ScheduleShow schedule in sAfterTomorrow)
            {
                if (GetByIdFromMyShows(myShows, schedule.Show.Id).MustNotify)
                {
                    string notificationBody = schedule.Show.Title + " - " + schedule.TitleEpisode + " on " + schedule.Show.Network.NetworkName + " at " + schedule.Airtime;
                    string notificationTitle = "Don't miss it! Tomorrow on TV";
                    string[] airtime = schedule.Airtime.Split(':');
                    DateTime notificationTime = new DateTime(DateTime.Year, DateTime.Month, DateTime.Day + 1, int.Parse(airtime[0]), int.Parse(airtime[1]), 0);
                    CrossLocalNotifications.Current.Show(notificationTitle, notificationBody, schedule.Show.Id, notificationTime);                    
                }
            }            
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

            return null;
        }
    }
}
