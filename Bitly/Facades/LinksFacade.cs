using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security;

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
        const int LinkHashSize = 8;
        private readonly DataContext dataContext;
        public LinksFacade(DataContext db)
        {
            dataContext = db;
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
                //User = new UserDto { Id = link.User.Id }
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
                throw new ArgumentException("Guid could not be empty");
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
                ShortLink = Guid.NewGuid().ToString("N").Substring(0, LinkHashSize),
                User = existingUser
            });
            dataContext.SaveChanges();
            return MapToLinkDto(result.Entity);
        }
    }
}