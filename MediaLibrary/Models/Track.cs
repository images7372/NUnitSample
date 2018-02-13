using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MediaLibrary.Models
{
    public class Track
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public int Duration { get; set; }
        public virtual Genre Genre { get; set; }
        public virtual Recording Recording { get; set; }
    }
}