using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MediaLibrary.Models
{
    public class Artist
    {
        [Required]
        public string Name { get; set; }
        public int Id { get; set; }

        public virtual ICollection<Recording> Recordings { get; set; }
        public virtual ICollection<Track> Tracks { get; set; }
    }
}