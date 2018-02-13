﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MediaLibrary.Models
{
    public class Genre
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<Track> Tracks { get; set; }
    }
}