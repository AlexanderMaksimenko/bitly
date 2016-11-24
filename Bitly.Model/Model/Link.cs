using System;
using System.ComponentModel.DataAnnotations;

namespace Bitly.Model
{
    public class Link
    {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public string SourceLink { get; set; }
        public string ShortLink { get; set; }
        public int JumpsCount { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}