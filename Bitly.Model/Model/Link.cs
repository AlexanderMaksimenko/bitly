using System;
using System.ComponentModel.DataAnnotations;

namespace Bitly.Model
{
    public class Link
    {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        [StringLength(2000)]
        public string SourceLink { get; set; }
        [StringLength(8)]
        public string ShortLink { get; set; }
        public int JumpsCount { get; set; }
        public User User { get; set; }
    }
}