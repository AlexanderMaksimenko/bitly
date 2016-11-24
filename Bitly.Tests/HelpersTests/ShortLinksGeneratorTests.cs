using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bitly.Tests
{
    [TestFixture]
    public class ShortLinksGeneratorTests
    {
        [Test]
        public void UniquenessTest() {
            var generator = new ShortLinksGenerator();
            var ids = new HashSet<string>();
            for (int i = 0; i < 1000000; i++) {
                ids.Add(generator.Generate());
            }
        }
    }
}
