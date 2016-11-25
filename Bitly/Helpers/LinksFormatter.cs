using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bitly
{
    public static class LinksFormatter
    {
        public static string FormatAndValidateSourceLink(string link)
        {
            const string HttpPrefix = "http://";
            if (!link.StartsWith(HttpPrefix) && !link.StartsWith("https://"))
            {
                link = HttpPrefix + link;
            }
            Uri uriResult;
            if (!Uri.TryCreate(link, UriKind.Absolute, out uriResult) || !uriResult.Host.Contains("."))
            {
                throw new ArgumentException("The source link is invalid");
            }
            return link;
        }
    }
}
