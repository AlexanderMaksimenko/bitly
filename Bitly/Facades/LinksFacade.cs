using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security;
using Bitly.Model;

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

        //make as extention
        LinkDto MapToLinkDto(Link link)
        {
            return new LinkDto
            {
                Id = link.Id,
                CreationDate = link.CreationDate,
                JumpsCount = link.JumpsCount,
                ShortLink = link.ShortLink,
                SourceLink = link.SourceLink,
                User = link.User == null ? null : new UserDto { Id = link.User.Id }
            };
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
            return MapToLinkDto(link);
        }

        public List<LinkDto> GetUserLinks(Guid userId)
        {
            const int MaxItems = 100;
            if(userId == Guid.Empty)
            {
                throw new ArgumentException("UserId could not be empty");
            }
            return dataContext.Links.Where(e => e.User.Id == userId).Take(MaxItems).ToList().Select(l => MapToLinkDto(l)).ToList();
        }

        public LinkDto SaveLink(LinkDto link) {
            if(link == null)
            {
                throw new ArgumentNullException(nameof(link));
            }
            User existingUser = null;
            if (link.User != null) {
                existingUser = dataContext.Users.Where(u => u.Id == link.User.Id).SingleOrDefault();
            }
            if (existingUser == null)
            {
                existingUser = dataContext.Users.Add(new User()).Entity;
                dataContext.SaveChanges();
            }
            var result = dataContext.Links.Add(new Link
            {
                CreationDate = DateTime.UtcNow,
                SourceLink = link.SourceLink,
                JumpsCount = 0,
                ShortLink = shortLinkGenerator.Generate(),
                User = existingUser
            });
            dataContext.SaveChanges();
            return MapToLinkDto(result.Entity);
        }
    }
}