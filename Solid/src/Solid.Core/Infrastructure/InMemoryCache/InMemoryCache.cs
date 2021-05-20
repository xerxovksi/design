namespace Solid.Core.Infrastructure.InMemoryCache
{
    using Microsoft.Extensions.Caching.Memory;
    using Solid.Core.Contracts;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class InMemoryCache<T> : ICache<T>
    {
        private readonly MemoryCacheEntryOptions options = null;

        private MemoryCache Cache { get; }

        public InMemoryCache(InMemoryCacheConfiguration configuration)
        {
            this.Cache = new MemoryCache(
                new MemoryCacheOptions
                {
                    SizeLimit = configuration.SizeLimit,
                    ExpirationScanFrequency = configuration.ExpirationScanFrequency
                });

            this.options = new MemoryCacheEntryOptions()
                .SetSize(configuration.DefaultItemSize);
        }

        public async Task<T> GetAsync(string key, Func<string, T> converter)
        {
            if (this.Cache.TryGetValue(key, out T result))
            {
                return await Task.FromResult(result);
            }

            return default;
        }

        public async Task<Dictionary<string, T>> GetAllAsync(
            Func<string, T> converter,
            string pattern = default)
        {
            var keys = this.GetKeys(pattern);
            if (keys == default)
            {
                return default;
            }

            var items = new Dictionary<string, T>();
            foreach (var key in keys)
            {
                items.Add(key, await this.GetAsync(key, converter));
            }

            return items;
        }

        public async Task<bool> SetAsync(
            string key,
            TimeSpan timeToLive,
            Func<string> converter)
        {
            var optionsCopy = this.options.DeepCopy();
            optionsCopy.SetAbsoluteExpiration(timeToLive);
            this.Cache.Set(key, converter(), optionsCopy);

            return await Task.FromResult(true);
        }

        public async Task<bool> RemoveAsync(string key)
        {
            if (this.Cache.TryGetValue(key, out _))
            {
                this.Cache.Remove(key);
            }

            return await Task.FromResult(true);
        }

        public async Task<bool> RemoveAllAsync(string pattern = default)
        {
            if (pattern == default)
            {
                this.Cache.Compact(100);
                return true;
            }

            var keys = this.GetKeys(pattern);
            if (keys == default)
            {
                return true;
            }

            foreach (var key in keys)
            {
                await this.RemoveAsync(key);
            }

            return true;
        }

        private List<string> GetKeys(string pattern = default)
        {
            throw new NotImplementedException();
        }
    }
}
