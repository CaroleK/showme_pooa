using System;
using System.Collections.Generic;
using System.Text;
using TVSeries.Models;

namespace TVSeries.ViewModels
{
    public class SerieDetailViewModel : BaseViewModel
    {
        public Serie Serie { get; set; }
        public SerieDetailViewModel(Serie serie = null)
        {
            Title = serie?.Title;
            Serie = serie;
        }
    }
}
