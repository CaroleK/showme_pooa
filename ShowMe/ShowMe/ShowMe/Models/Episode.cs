using System;
using System.Collections.Generic;
using System.Text;

namespace ShowMe.Models
{
    public class Episode
    {
        public int Season;
        public int Number;
        public int ShowId;

        public Episode(int season, int number, int showId)
        {
            Season = season;
            Number = number;
            ShowId = showId;
        }
    }
}
