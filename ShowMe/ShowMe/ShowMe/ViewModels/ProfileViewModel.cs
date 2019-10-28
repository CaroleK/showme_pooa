using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using ShowMe.Models;

namespace ShowMe.ViewModels
{
    public class ProfileViewModel : BaseViewModel
    {
        ObservableCollection<int> minutesWatched { get; set; } = new ObservableCollection<int>();
        ObservableCollection<int> hoursWatched { get; set; } = new ObservableCollection<int>();
        ObservableCollection<int> daysWatched { get; set; } = new ObservableCollection<int>();
        ObservableCollection<int> episodesWatched { get; set; } = new ObservableCollection<int>();

        
        public ProfileViewModel()
        {
            
        }

        public void DisplayStatistics(User user)
        {
            daysWatched.Add(user.TotalMinutesWatched / 1400);
            hoursWatched.Add((user.TotalMinutesWatched % 1400) / 60);
            minutesWatched.Add((user.TotalMinutesWatched % 1400) % 60);
            episodesWatched.Add((user.TotalNbrEpisodesWatched));
        }
    }
}
