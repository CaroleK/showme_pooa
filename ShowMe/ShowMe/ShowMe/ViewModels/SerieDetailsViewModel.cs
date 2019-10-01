using System;
using System.Collections.Generic;
using System.Text;
using ShowMe.Models;

namespace ShowMe.ViewModels
{
    public class SerieDetailsViewModel : BaseViewModel
    {
        public Show Serie { get; set; }
        public SerieDetailsViewModel(Show serie = null)
        {
            Title = serie?.Title;
            Serie = serie;
        }
    }
}
