using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace MediaLibrary.Models
{
    public class MediaLibraryContext : DbContext
    {
        public MediaLibraryContext() : base("MediaLibraryContext") { }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Label> Labels { get; set; }
        public DbSet<Recording> Recordings { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Reviewer> Reviewers { get; set; }
        public DbSet<Track> Tracks { get; set; }

    }
}