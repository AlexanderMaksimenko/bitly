using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bitly.Model
{
    public class User
    {
        public Guid Id { get; set; }
        public List<Link> Links { get; set; }
    }
}