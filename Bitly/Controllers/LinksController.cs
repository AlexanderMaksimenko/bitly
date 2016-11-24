using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Bitly
{
    [Route("api/[controller]")]
    public class LinksController : Controller
    {
        ILinksFacade facade;

        public LinksController(ILinksFacade facade)
        {
            this.facade = facade;
        }
        
        [HttpGet("byUser")]
        public JsonResult Get(Guid id)
        {            
            return new JsonResult(facade.GetUserLinks(id));
        }

        [HttpGet("{link}")]        
        public JsonResult Get(string link)
        {            
            return new JsonResult(facade.GetLink(link));
        }

        [HttpPost]
        public JsonResult Post([FromBody] LinkDto value)
        {            
            return new JsonResult(facade.SaveLink(value));
        }
    }
}
