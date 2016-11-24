using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bitly
{
    public class LinkDto
    {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public string SourceLink { get; set; }
        public string ShortLink { get; set; }
        public int JumpsCount { get; set; }
        public UserDto User { get; set; }
    }
}
