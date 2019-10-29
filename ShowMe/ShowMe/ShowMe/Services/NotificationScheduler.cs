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
            DateTime DateTime = DateTime.Now;
            CrossLocalNotifications.Current.Show("debug", "Woke up at " + DateTime.ToShortTimeString(), 0, DateTime);

            ObservableCollection<MyShow> myShows = new ObservableCollection<MyShow>(Task.Run(() => FireBaseHelper.GetUserShowList(userId)).Result);

            // We check what's on TV tomorrow and in two days, to be able to schedule notifications 24h before
            string dateTimeTomorrow = DateTime.Year + "-" + DateTime.Month + "-" + (DateTime.Day + 1);
            string dateTimeAfterTomorrow = DateTime.Year + "-" + DateTime.Month + "-" + (DateTime.Day + 2);
            // Choice made to focus on US region because of the API's data 
            string regionISO = "US";

            List<ScheduleShow> sTomorrow = Task.Run(() => service.GetUpCommingEpisode(myShows, dateTimeAfterTomorrow, regionISO)).Result;
            List<ScheduleShow> sAfterTomorrow = Task.Run(() => service.GetUpCommingEpisode(myShows, dateTimeAfterTomorrow, regionISO)).Result;
            sTomorrow.AddRange(sAfterTomorrow); 

            foreach (ScheduleShow schedule in sTomorrow)
            {
                if (GetByIdFromMyShows(myShows, schedule.Show.Id).MustNotify)
                {
                    string notificationBody = schedule.TitleEpisode + " -  on " + schedule.Show.Network.NetworkName + " at " + schedule.Airtime;
                    string notificationTitle = "Don't miss it! " + schedule.Show.Title + "is on TV tomorrow";
                    string[] airtime = schedule.Airtime.Split(':');
                    DateTime notificationTime = new DateTime(DateTime.Year, DateTime.Month, DateTime.Day + 1, int.Parse(airtime[0]), int.Parse(airtime[1]), 0);
                    string notificationId = schedule.Show.Id + "" + DateTime.Day + "" + DateTime.Month;

                    CrossLocalNotifications.Current.Show(notificationTitle, notificationBody, int.Parse(notificationId), notificationTime);                    
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
