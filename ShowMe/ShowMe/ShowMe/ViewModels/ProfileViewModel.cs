using System;
using System.Collections.Generic;
using System.Text;
using ShowMe.Models;

namespace ShowMe.ViewModels
{
    public class ProfileViewModel : BaseViewModel
    {
        int minutesWatched { get; set; } = 0;
        int hoursWatched { get; set; } = 0;
        int daysWatched { get; set; } = 0;
        public ProfileViewModel()
        {
            
        }


        public void DisplayStatistics(User user)
        {
            daysWatched = user.TotalMinutesWatched / 1400;
            hoursWatched = (user.TotalMinutesWatched % 1400) / 60;
            minutesWatched = (user.TotalMinutesWatched % 1400) % 60;
        }
    }
}
