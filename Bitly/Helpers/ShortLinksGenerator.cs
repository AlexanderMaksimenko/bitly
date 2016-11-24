using System;

namespace Bitly
{
    public interface IShortLinksGenerator
    {
        string Generate();
    }
    public class ShortLinksGenerator : IShortLinksGenerator
    {
        const int LinkHashSize = 8;
        public string Generate()
        {
            return Guid.NewGuid().ToString("N").Substring(0, LinkHashSize).ToUpperInvariant();
        }
    }
}
