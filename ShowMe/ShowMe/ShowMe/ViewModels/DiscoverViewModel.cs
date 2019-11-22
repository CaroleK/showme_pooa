using ShowMe.Models;
using ShowMe.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace ShowMe.ViewModels
{
    /// <summary>
    /// View Model associated with the Discover Page
    /// </summary>
    class DiscoverViewModel : BaseViewModel
    {
        // The list of shows to display on the page
        public ObservableCollection<Show> Shows { get; set; }

        // TV Maze service
        TvMazeService service = new TvMazeService();

        //Instantiate a Singleton of the Semaphore with a value of 1. This means that only 1 thread can be granted access at a time.
        static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

        // Counter used to increment the ids of shows to display
        public int counter = 1;

        public DiscoverViewModel()
        {
            Title = "Browse";
            Shows = new ObservableCollection<Show>();
            // Shows must be shown when the user arrives on the discover page
            Task.Run(() => ExecuteLoadShowCommand());
        }

        /// <summary>
        /// Retrieves 10 new shows from API and adds them to shows to display
        /// </summary>
        /// <returns>Void task</returns>
        public async Task ExecuteLoadShowCommand()
        {
            // Lock the access to show list and counter because shows are retrieved asynchronously
            // (If user scrolls down too fast, the same shows would possibily be added multiple times otherwise) 
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

        /// <summary>
        /// Searches for shows matching with user input. Clears the show list and adds results instead
        /// </summary>
        /// <param name="search">The user's input</param>
        /// <returns>Void task</returns>
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
            
        }
    }
}

