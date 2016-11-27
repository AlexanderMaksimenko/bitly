using Microsoft.AspNetCore.Mvc;

namespace Bitly
{
    [Route("api/[controller]")]
    public class RedirectController : Controller
    {

        [HttpGet("{link}")]
        public RedirectResult Get(string link)
        {
            return Redirect(link.Replace("-", "/"));
        }
    }
}
