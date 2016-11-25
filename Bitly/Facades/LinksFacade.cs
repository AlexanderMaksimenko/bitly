using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security;
using Bitly.Model;
using System.Net;

namespace Bitly
{

    public interface ILinksFacade
    {
        LinkDto GetLink(string shortLink);
        List<LinkDto> GetUserLinks(Guid userId);
        LinkDto SaveLink(LinkDto link);
    }
    public class LinksFacade : ILinksFacade
    {
        private readonly DataContext dataContext;
        private readonly IShortLinksGenerator shortLinkGenerator;

        public LinksFacade(DataContext db, IShortLinksGenerator shortLinkGenerator)
        {
            dataContext = db;
            this.shortLinkGenerator = shortLinkGenerator;
        }

        public LinkDto GetLink(string shortLink)
        {
            if (string.IsNullOrEmpty(shortLink))
            {
                throw new ArgumentNullException(nameof(shortLink));
            }
            var link = dataContext.Links.Where(e => e.ShortLink == shortLink.Trim().ToUpperInvariant()).SingleOrDefault();
            if (link == null)
            {
                return null;
            }
            link.JumpsCount++;
            dataContext.Links.Update(link);
            dataContext.SaveChanges();
            return link.MapToLinkDto();
        }

        public List<LinkDto> GetUserLinks(Guid userId)
        {
            const int MaxItems = 100;
            if(userId == Guid.Empty)
            {
                throw new ArgumentException("UserId could not be empty");
            }
            return dataContext.Links.Where(e => e.User.Id == userId).Take(MaxItems).ToList().Select(l => l.MapToLinkDto()).ToList();
        }

        public LinkDto SaveLink(LinkDto link) {
            if(link == null)
            {
                throw new ArgumentNullException(nameof(link));
            }
            var sourceLink = WebUtility.UrlEncode(LinksFormatter.FormatAndValidateSourceLink(link.SourceLink));
            User existingUser = null;
            if (link.User != null) {
                existingUser = dataContext.Users.Where(u => u.Id == link.User.Id).SingleOrDefault();
            }
            if (existingUser == null)
            {
                existingUser = dataContext.Users.Add(new User()).Entity;
                dataContext.SaveChanges();
            }
            else
            {
                var existingResult = dataContext.Links.Where(l => l.User.Id == existingUser.Id && l.SourceLink == sourceLink).FirstOrDefault();
                if (existingResult != null)
                {
                    return existingResult.MapToLinkDto();
                }
            }
            var result = dataContext.Links.Add(new Link
            {
                CreationDate = DateTime.UtcNow,
                SourceLink = sourceLink,
                JumpsCount = 0,
                ShortLink = shortLinkGenerator.Generate(),
                User = existingUser
            });
            dataContext.SaveChanges();
            return result.Entity.MapToLinkDto();
        }
    }
}