using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace MediaLibrary.Models
{
    public class MediaLibrayBasicInitializer : DropCreateDatabaseAlways<MediaLibraryContext>
    {
        protected override void Seed(MediaLibraryContext context)
        {
            base.Seed(context);
            var labels = new List<Label>()
                {
                    new Label()
                    {
                        Name = "Universal Music"
                    },
                    new Label()
                    {
                        Name = "Atlantic Recording"
                    }
                };
            labels.ForEach(m => context.Labels.Add(m));

            var artists = new List<Artist>()
                {
                    new Artist()
                    {
                        Name = "The Beatles"
                    },
                    new Artist()
                    {
                        Name = "Black Sabbath"
                    },
                    new Artist()
                    {
                        Name = "AC/DC"
                    },
                    new Artist()
                    {
                        Name = "King Crimson"
                    }
                };
            artists.ForEach(m => context.Artists.Add(m));

            context.SaveChanges();
        }
     }
}