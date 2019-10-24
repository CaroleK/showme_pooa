using Plugin.LocalNotifications;
using ShowMe.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShowMe.Services
{
    public class NotificationScheduler
    {
        static public void ScheduleNotification(string name)
        {
            //TODO
            CrossLocalNotifications.Current.Show("title", name, 99, DateTime.Now);
            CrossLocalNotifications.Current.Show("title", "body", 101, DateTime.Now.AddSeconds(10));
        }
    }
}
