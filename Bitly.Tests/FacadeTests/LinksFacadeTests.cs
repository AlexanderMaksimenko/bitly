using Bitly.Model;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Bitly.Tests
{
    [TestFixture]
    public class LinksFacadeTests
    {
        ILinksFacade facade;
        DataContext context;
        Guid userId1 = Guid.NewGuid();
        Guid userId2 = Guid.NewGuid();
        const string ExistingShortLink = "asd34fsd";
        const string ExistingSourceLink = "http://test1.ru/";
        int currentJupmsCount = 4;

        void FillTestData(DataContext context)
        {
            context.Add(new Link
            {
                Id = 1,
                SourceLink = ExistingSourceLink,
                CreationDate = new DateTime(2015, 01, 02),
                JumpsCount = currentJupmsCount,
                ShortLink = ExistingShortLink.ToUpperInvariant(),
                User = new User { Id = userId1 }
            });
            context.SaveChanges();
        }

        [OneTimeSetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
              .UseInMemoryDatabase(databaseName: "FakeDatabase")
              .Options;
            context = new DataContext(options);
            FillTestData(context);
            facade = new LinksFacade(context, new ShortLinksGenerator());
        }

        DataContext CreateContextInternal()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                  .UseInMemoryDatabase(databaseName: "FakeDatabase" + Guid.NewGuid())
                  .Options;
            return new DataContext(options);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            context.Dispose();
        }

        [TestCase("asd8qws", ExpectedResult = null)]
        [TestCase(ExistingShortLink, ExpectedResult = ExistingSourceLink)]
        [TestCase(" " + ExistingShortLink + "  ", ExpectedResult = ExistingSourceLink)]
        public string GetLink(string shortLink)
        {
            var result = facade.GetLink(shortLink);
            if (result != null)
            {
                Assert.That(result.JumpsCount, Is.EqualTo(++currentJupmsCount));
            }
            return result?.SourceLink;
        }

        [Test]
        public void GetLinkExceptions()
        {
            Assert.That(() => facade.GetLink(null), Throws.Exception.TypeOf<ArgumentNullException>()
               .With.Property("ParamName").EqualTo("shortLink"));
        }

        [Test]
        public void GetUserLinks()
        {
            var link1 = new Link
            {
                Id = 2,
                SourceLink = "http://test3.ru/",
                CreationDate = new DateTime(2015, 01, 01),
                JumpsCount = 0,
                ShortLink = "sdqw321sd".ToUpperInvariant(),
                User = new User { Id = userId2 }
            };
            var link2 = new Link
            {
                Id = 3,
                SourceLink = "http://test4.ru/",
                CreationDate = new DateTime(2015, 01, 02),
                JumpsCount = 7,
                ShortLink = "dfgf53dsf".ToUpperInvariant(),
                User = new User { Id = userId2 }
            };
            context.Add(link1);
            context.Add(link2);
            context.SaveChanges();

            var result = facade.GetUserLinks(Guid.NewGuid());
            Assert.That(result.Any(), Is.False);
            result = facade.GetUserLinks(userId2);

            Assert.That(result.Count, Is.EqualTo(2));
            var first = result.Single(l => l.Id == link1.Id);
            Assert.That(first.SourceLink, Is.EqualTo(link1.SourceLink));
            Assert.That(first.CreationDate, Is.EqualTo(link1.CreationDate));
            Assert.That(first.JumpsCount, Is.EqualTo(link1.JumpsCount));
            Assert.That(first.ShortLink, Is.EqualTo(link1.ShortLink));
            Assert.That(first.User.Id, Is.EqualTo(link1.User.Id));
            var second = result.Single(l => l.Id == link2.Id);
            Assert.That(second.SourceLink, Is.EqualTo(link2.SourceLink));
            Assert.That(second.CreationDate, Is.EqualTo(link2.CreationDate));
            Assert.That(second.JumpsCount, Is.EqualTo(link2.JumpsCount));
            Assert.That(second.ShortLink, Is.EqualTo(link2.ShortLink));
            Assert.That(second.User.Id, Is.EqualTo(link2.User.Id));
        }

        [Test]
        public void GetUserLinksExceptions()
        {
            Assert.That(() => facade.GetUserLinks(Guid.Empty), Throws.Exception.TypeOf<ArgumentException>()
              .With.Message.EqualTo("UserId could not be empty"));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void SaveLink(bool isUserExists)
        {
            const string SourceLink = "http://test7.ru";
            var userId = Guid.NewGuid();
            const string ExpectedShortLink = "ad342sadsa";

            var shortLinkGenerator = Substitute.For<IShortLinksGenerator>();
            shortLinkGenerator.Generate().Returns(ExpectedShortLink);

            using (var savingContext = CreateContextInternal())
            {
                var facade = new LinksFacade(savingContext, shortLinkGenerator);
                if (isUserExists)
                {
                    savingContext.Users.Add(new User { Id = userId });
                    savingContext.SaveChanges();
                }

                var usersCount = savingContext.Users.Count();
                var result = facade.SaveLink(new LinkDto { SourceLink = SourceLink, User = isUserExists ? new UserDto { Id = userId } : null });


                Assert.That(savingContext.Users.Count(), Is.EqualTo(isUserExists ? usersCount : usersCount + 1));

                var link = savingContext.Links.Where(l => l.SourceLink == WebUtility.UrlEncode(SourceLink)).Single();
                Assert.That(link.JumpsCount, Is.EqualTo(0));
                Assert.That(link.ShortLink, Is.EqualTo(ExpectedShortLink));
                if (isUserExists)
                {
                    Assert.That(link.User.Id, Is.EqualTo(userId));
                }
            }
        }       
    }
}