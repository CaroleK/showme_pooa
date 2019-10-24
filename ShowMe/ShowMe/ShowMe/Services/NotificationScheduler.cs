using Plugin.LocalNotifications;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShowMe.Services
{
    public class NotificationScheduler
    {
        static public void ScheduleNotification()
        {
            //TODO
            CrossLocalNotifications.Current.Show("title", App.User.GivenName, 99, DateTime.Now);
            CrossLocalNotifications.Current.Show("title", "body", 101, DateTime.Now.AddSeconds(10));
        }
    }
}
