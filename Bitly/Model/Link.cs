using System;
using System.ComponentModel.DataAnnotations;

namespace Bitly
{
    public class Link
    {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public string SourceLink { get; set; }
        public string ShortLink { get; set; }
        public int JumpsCount { get; set; }
        public User User { get; set; }
    }
}