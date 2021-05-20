namespace Solid.Console
{
    using Solid.Console.Examples;
    using System;
    using System.Threading.Tasks;

    public class Program
    {
        static async Task Main()
        {
            await SecretPlayground();
            await CachePlayground();
        }

        private static async Task SecretPlayground()
        {
            var secretPlayground = new SecretPlayground(
                message => Console.WriteLine(message));

            await secretPlayground.LocalPlayground();
            await secretPlayground.AzurePlayground();
        }

        private static async Task CachePlayground()
        {
            var cachePlayground = new CachePlayground(
                message => Console.WriteLine(message));

            await cachePlayground.InMemoryPlayground();
            await cachePlayground.RedisPlayground();
        }
    }
}
