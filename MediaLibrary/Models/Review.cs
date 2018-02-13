using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MediaLibrary.Models
{
    public class Review
    {
        public int ID { get; set; }
        public decimal Rating { get; set; }

        [Required]
        public string Summary { get; set; }

        [Required]
        public virtual Reviewer Reviewer { get; set; }

        [Required]
        public virtual Recording Recording { get; set; }
    }
}