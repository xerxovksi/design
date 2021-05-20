namespace Solid.Core.Infrastructure.InMemoryCache
{
    using System;

    public class InMemoryCacheConfiguration
    {
        public int SizeLimit { get; }

        public TimeSpan ExpirationScanFrequency { get; }

        public int DefaultItemSize { get; }

        public InMemoryCacheConfiguration(
            int sizeLimit,
            TimeSpan expirationScanFrequency,
            int defaultItemSize)
        {
            this.SizeLimit = sizeLimit;
            this.ExpirationScanFrequency = expirationScanFrequency;
            this.DefaultItemSize = defaultItemSize;
        }
    }
}
