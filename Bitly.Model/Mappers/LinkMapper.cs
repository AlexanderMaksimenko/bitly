using System.Net;

namespace Bitly.Model
{
    public static class LinkMapper
    {
        public static LinkDto MapToLinkDto(this Link link)
        {
            if (link == null)
            {
                return null;
            }
            return new LinkDto
            {
                Id = link.Id,
                CreationDate = link.CreationDate,
                JumpsCount = link.JumpsCount,
                ShortLink = link.ShortLink,
                SourceLink = WebUtility.UrlDecode(link.SourceLink),
                User = link.User == null ? null : new UserDto { Id = link.User.Id }
            };
        }
    }
}