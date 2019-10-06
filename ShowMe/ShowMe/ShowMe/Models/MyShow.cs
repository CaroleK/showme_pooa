using System;
using System.Collections.Generic;
using System.Text;

namespace ShowMe.Models
{
    public class MyShow : Show
    {
        public bool IsFavorite { get; set; }
        public bool MustNotify { get; set; }
        public Episode LastEpisode { get; set; }

        public string LastEpisodeInString => "S" + LastEpisode.Season + "E" + LastEpisode.Number;
        public MyShow (Show show, bool isFavorite, bool mustNotify, Episode lastEpisode)
        {
            Id = show.Id;
            Title = show.Title;
            Language = show.Language;
            Genres = show.Genres;
            Url = show.Url;
            Description = show.Description;
            Image = show.Image;

            IsFavorite = isFavorite;
            MustNotify = mustNotify;
            LastEpisode = lastEpisode; 
        }

        public void IncrementLastEpisode()
        {
            //TODO
        }        
    }
}
