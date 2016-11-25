using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bitly.Tests.HelpersTests
{
    [TestFixture]
    public class LinksFormatterTest
    {

        [TestCase("test.ru", ExpectedResult = true)]
        [TestCase("http://test.ru", ExpectedResult = true)]
        [TestCase("https://test.ru", ExpectedResult = true)]
        [TestCase("test", ExpectedResult = false)]
        [TestCase("test.ru/eefs/deddd?if=asdsds", ExpectedResult = true)]
        [TestCase("http:/test.ru", ExpectedResult = false)]
        public bool LinkValidation(string link)
        {
            var success = true;
            try
            {
                LinksFormatter.FormatAndValidateSourceLink(link);
            }
            catch (ArgumentException ex)
            {
                success = false;
                Assert.That(ex.Message, Is.EqualTo("The source link is invalid"));
            }
            return success;
        }
    }
}
