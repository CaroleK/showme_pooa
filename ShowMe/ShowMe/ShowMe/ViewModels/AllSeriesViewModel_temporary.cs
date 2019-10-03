using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ShowMe.Models;
using ShowMe.Services;
using Xamarin.Forms;

namespace ShowMe.ViewModels
{
    class AllSeriesViewModel_temporary : BaseViewModel
    {
        public ObservableCollection<Show> Series { get; set; }
        TvMazeService service = new TvMazeService();
        public int counter = 1;

        public AllSeriesViewModel_temporary()
        {
            Title = "Browse";
            Series = new ObservableCollection<Show>();
            Task.Run(() => ExecuteLoadSeriesCommand());
        }

        public async Task ExecuteLoadSeriesCommand()
        {
            for (int i = counter * 10 - 9; i < counter * 10 + 1; i++)
            {
                Show s = await service.GetShowAsync("https://api.tvmaze.com/shows/" + i);
                if (s != null)
                {
                    Series.Add(s);
                }
            }
            counter++;
        }

        public async Task ExecuteSearchSeriesCommand(string search)
        {
            List<Show> s = await service.SearchShowAsync(search);
            Series.Clear();
            if ((s != null) && (s.Count > 0))
            {
                foreach (Show serie in s)
                {
                    Series.Add(serie);
                }
            }
            counter = 1;
        }
    }
}
