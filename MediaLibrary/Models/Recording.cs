using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MediaLibrary.Models
{
    public class Recording
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public DateTime? ReleaseDate { get; set; }
        [Required]
        public virtual Label Label { get; set; }
        [Required]
        public virtual Artist Artist { get; set; }
        public virtual ICollection<Track> Tracks { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }

    }
}