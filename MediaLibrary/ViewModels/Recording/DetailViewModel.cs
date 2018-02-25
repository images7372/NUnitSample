using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MediaLibrary.Models;

namespace MediaLibrary.ViewModels.Recording
{
    public class DetailViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string LabelName { get; set; }
        public string ArtistName { get; set; }
        public List<Track> Tracks { get; set; }

        //public virtual ICollection<Review> Reviews { get; set; }
    }
}