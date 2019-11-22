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
        static public async Task ScheduleNotification(string userId)
        {
            try
            {
                DateTime DateTime = DateTime.Now;

                // Fetch user's show list and shows scheduled in the days to come
                ObservableCollection<MyShow> myShows = new ObservableCollection<MyShow>(await FireBaseHelper.GetUserShowList(userId));
                List<ScheduleShow> scheduledShows = RetrieveScheduledShows(myShows);

                // Schedule notifications
                foreach (ScheduleShow schedule in scheduledShows)
                {
                    if (GetMustNotifyByIdFromMyShows(myShows, schedule.Show.Id))
                    {

                        string notificationBody = schedule.Airtime + " - " + schedule.Show.Network.NetworkName + " || " + schedule.TitleEpisode;
                        string notificationTitle = "Don't miss it! " + schedule.Show.Title + " is on TV tomorrow";

                        string[] airtime = schedule.Airtime.Split(':');
                        string[] airdate = schedule.Airdate.Split('-');
                        DateTime airDate = new DateTime(int.Parse(airdate[0]), int.Parse(airdate[1]), int.Parse(airdate[2]), int.Parse(airtime[0]), int.Parse(airtime[1]), 0);
                        
                        // If the date is already passed, notification is sent right away 
                        // This behaviour would not be ideal in the real applications, 
                        // but for demonstration purposes it allows us to prove that notifications work without having to wait
                        DateTime notificationTime = airDate.AddDays(-1);

                        // If two notifications have the same id, the last one created overrides the first
                        // That way, we never send notifications twice for the same episode
                        string notificationIdString = (schedule.IdEpisode != null) ? schedule.IdEpisode + "" : schedule.Show.Id + "" + notificationTime.Hour + "" + notificationTime.Day;
                        int notificationId = int.Parse(notificationIdString);

                        // We use a plugin to send notifications
                        CrossLocalNotifications.Current.Show(notificationTitle, notificationBody, notificationId, notificationTime);
                    }
                }
            }
            catch (Exception e)
            {
                CrossLocalNotifications.Current.Show("Oops, an exception occurred while scheduling notifications", e.Message, 0, DateTime.Now);
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

            // We check what's on TV tomorrow and in two days, to be able to schedule notifications 24h before
            string dateTimeTomorrow = DateTime.AddDays(1).ToString("yyyy-MM-dd");
            string dateTimeAfterTomorrow = DateTime.AddDays(2).ToString("yyyy-MM-dd");

            // Choice made to focus on US region because of the API's data 
            string regionISO = "US";

            List<ScheduleShow> sTomorrow = Task.Run(() => service.GetUpCommingEpisode(myShows, dateTimeTomorrow, regionISO)).Result;
            List<ScheduleShow> sAfterTomorrow = Task.Run(() => service.GetUpCommingEpisode(myShows, dateTimeAfterTomorrow, regionISO)).Result;
            sTomorrow.AddRange(sAfterTomorrow);

            return sTomorrow; 
        }


        /// <summary>
        /// Retrieves the MustNotify property of a show from the instance of the user's MyShow list by the show ID
        /// </summary>
        /// <param name="id">The ID of the MyShow to retrieve</param>
        /// <returns>The matching MustNotifiy (or false if no show matches the ID)</returns>
        static protected bool GetMustNotifyByIdFromMyShows(ObservableCollection<MyShow> myShows, int id)
        {
            foreach (MyShow myShow in myShows)
            {
                if (myShow.Id == id)
                {
                    return myShow.MustNotify;
                }
            }

            // if no matching show was found, return false
            return false;
        }
    }
}
