using ShowMe.Models;
using ShowMe.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace ShowMe.ViewModels
{
    class DiscoverViewModel : BaseViewModel
    {
        public ObservableCollection<Show> Shows { get; set; }

        TvMazeService service = new TvMazeService();

        //Instantiate a Singleton of the Semaphore with a value of 1. This means that only 1 thread can be granted access at a time.
        static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

        public int counter = 1;

        public DiscoverViewModel()
        {
            Title = "Browse";
            Shows = new ObservableCollection<Show>();
            // Example must be shown as the user first arrived on the discover page
            Task.Run(() => ExecuteLoadShowCommand());
        }

        public async Task ExecuteLoadShowCommand()
        {
            await semaphoreSlim.WaitAsync();
            try
            {
                for (int i = counter * 10 - 9; i < counter * 10; i++)
                {
                    Show s = await service.GetShowAsync(i);
                    if (s != null)
                    {
                        Shows.Add(s);
                    }

                }
                counter++;
                
            }
            finally
            {
                semaphoreSlim.Release();
            }
            
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

