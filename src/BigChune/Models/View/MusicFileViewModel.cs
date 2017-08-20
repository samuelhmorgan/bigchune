using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BigChune.Models.View
{
    public class MusicFileViewModel
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public int Year { get; set; }
        public string Comment { get; set; }
        public string Genre { get; set; }
        public int Rating { get; set; }
        public string Lyrics { get; set; }
        public string Directory { get; set; }
        public string Filename { get; set; }
        public string Filesize { get; set; }
        public int Duration { get; set; }
        public string Hash { get; set; }
        public string Mime { get; set; }
        public string AlbumArt { get; set; }
    }
}
