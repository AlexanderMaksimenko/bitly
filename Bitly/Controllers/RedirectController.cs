using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bitly.Model;

namespace Bitly
{
    [Route("api/[controller]")]
    public class RedirectController : Controller
    {

        [HttpGet("{link}")]
        public void Get(string link)
        {
            
            HttpContext.Response.Redirect(link.Replace("-", "/"));
        }
    }
}
