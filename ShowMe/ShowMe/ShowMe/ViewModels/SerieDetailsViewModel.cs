using System;
using System.Collections.Generic;
using System.Text;
using ShowMe.Models;

namespace ShowMe.ViewModels
{
    public class ShowDetailsViewModel : BaseViewModel
    {
        public Show Show { get; set; }
        public ShowDetailsViewModel(Show show = null)
        {
            Title = show?.Title;
            Show = show;
        }
    }
}
