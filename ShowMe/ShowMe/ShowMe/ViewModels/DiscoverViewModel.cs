using ShowMe.Models;
using ShowMe.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace ShowMe.ViewModels
{
    class DiscoverViewModel : BaseViewModel
    {
        public ObservableCollection<Show> Shows { get; set; }

        TvMazeService service = new TvMazeService();

        public int counter = 1;

        public DiscoverViewModel()
        {
            Title = "Browse";
            Shows = new ObservableCollection<Show>();
            // Example must be shown as the user first arrived on the discover page
            Task.Run(() => ExecuteLoadShowCommand());
            //Show s = Shows[0];
        }

        public async Task ExecuteLoadShowCommand()
        {
            for (int i = counter * 10 - 9; i < counter * 10; i++)
            {
                Show s = await service.GetShowAsync("https://api.tvmaze.com/shows/" + i);
                if (s != null)
                {
                    Shows.Add(s);
                }

            }
            counter++;
        }

        public async Task ExecuteSearchShowCommand(string search)
        {
            List<Show> s = await service.SearchShowAsync(search);
            Shows.Clear();
            if ((s != null) && (s.Count > 0))
            {
                foreach (Show show in s)
                {
                    Shows.Add(show);
                }
            }
            counter = 1;
        }
    }
}

