namespace Solid.Console.Examples
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Solid.Core.Contracts;
    using Solid.Core.Infrastructure.InMemoryCache;
    using Solid.Core.Infrastructure.RedisCache;
    using System;
    using System.Threading.Tasks;

    public class CachePlayground
    {
        private readonly Action<string> log = null;

        public CachePlayground(Action<string> log) =>
            this.log = log;

        public async Task InMemoryPlayground()
        {
            var services = new ServiceCollection();
            this.RegisterInMemoryDependencies(services);

            var provider = services.BuildServiceProvider();
            var cache = provider.GetService<ICache<string>>();

            await cache.SetAsync(
                key: "Username",
                timeToLive: TimeSpan.FromMinutes(2),
                converter: () => "Abhishek");

            await cache.SetAsync(
                key: "Password",
                timeToLive: TimeSpan.FromMinutes(2),
                converter: () => "HelloWorld!");

            var username = await cache.GetAsync(
                key: "Username",
                converter: result => result);

            var password = await cache.GetAsync(
                key: "Password",
                converter: result => result);

            log($"Username from InMemory cache: {username}");
            log($"Password from InMemory cache: {password}");
        }

        public async Task RedisPlayground()
        {
            var services = new ServiceCollection();
            this.RegisterRedisDependencies(services);

            var provider = services.BuildServiceProvider();
            var cache = provider.GetService<ICache<string>>();

            await cache.SetAsync(
                key: "Alias",
                timeToLive: TimeSpan.FromMinutes(2),
                converter: () => "Xerxovksi");

            var alias = await cache.GetAsync(
                key: "Alias",
                converter: result => result);

            log($"Alias from Redis cache: {alias}");
        }

        private void RegisterInMemoryDependencies(IServiceCollection services)
        {
            services.AddSingleton(_ => new InMemoryCacheConfiguration(
                sizeLimit: 1024,
                expirationScanFrequency: TimeSpan.FromMinutes(1),
                defaultItemSize: 1));

            services.AddSingleton(
                typeof(ICache<>),
                typeof(InMemoryCache<>));
        }

        private void RegisterRedisDependencies(IServiceCollection services)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile(
                    path: "appsettings.json",
                    optional: true,
                    reloadOnChange: true)
                .Build();

            services.AddSingleton(_ => configuration);

            services.AddSingleton<IRedisConnection>(
                provider => new RedisConnection(
                    provider.GetRequiredService<IConfiguration>()["redisConnectionString"]));

            services.AddScoped(typeof(ICache<>), typeof(RedisCache<>));
        }
    }
}
