namespace Solid.Console
{
    using Azure.Identity;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Solid.Core.Contracts;
    using Solid.Core.Infrastructure;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using AzureSecretClient = Azure.Security.KeyVault.Secrets.SecretClient;

    public class Program
    {
        static async Task Main()
        {
            await LocalPlayground();
            await AzurePlayground();
        }

        private static async Task LocalPlayground()
        {
            var services = new ServiceCollection();
            RegisterLocalDependencies(services);

            var provider = services.BuildServiceProvider();
            var secretManager = provider.GetService<ISecretManager>();

            Console.WriteLine(await secretManager.GetSecretAsync("vaultA", "Username"));
            Console.WriteLine(await secretManager.GetSecretAsync("vaultA", "Password"));
            await secretManager.SetSecretAsync("vaultA", "Password", "JusticeLeague");
            Console.WriteLine(await secretManager.GetSecretAsync("vaultA", "Password"));

            Console.WriteLine(await secretManager.GetSecretAsync("vaultB", "Username"));
            Console.WriteLine(await secretManager.GetSecretAsync("vaultB", "Password"));
            await secretManager.SetSecretAsync("vaultB", "Password", "Avengers");
            Console.WriteLine(await secretManager.GetSecretAsync("vaultB", "Password"));
        }

        private static async Task AzurePlayground()
        {
            var services = new ServiceCollection();
            RegisterAzureDependencies(services);

            var provider = services.BuildServiceProvider();
            var configuration = provider.GetService<IConfiguration>();
            var secretManager = provider.GetService<ISecretManager>();

            Console.WriteLine(
                await secretManager.GetSecretAsync(configuration["vaultA"], "Username"));
            Console.WriteLine(
                await secretManager.GetSecretAsync(configuration["vaultB"], "Password"));
        }

        private static void RegisterLocalDependencies(ServiceCollection services)
        {
            services.AddSingleton<ISecretClient>(_ => new LocalVaultClient("vaultA"));
            services.AddSingleton<ISecretClient>(_ => new LocalVaultClient("vaultB"));

            services.AddSingleton<ISecretManagerFactory<ISecretClient>>(
                provider => new LocalVaultManagerFactory(
                    provider.GetServices<ISecretClient>()
                        .First(client => client.StoreName.Equals("vaultA")),
                    provider.GetServices<ISecretClient>()
                        .First(client => client.StoreName.Equals("vaultB"))));

            services.AddScoped<ISecretManager, LocalVaultManager>();
        }

        private static void RegisterAzureDependencies(ServiceCollection services)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile(
                    path: "appsettings.json",
                    optional: true,
                    reloadOnChange: true)
                .Build();

            services.AddSingleton(_ => configuration);

            services.AddSingleton(_ => new AzureSecretClient(
                new Uri(configuration["vaultA"]),
                new DefaultAzureCredential()));

            services.AddSingleton(_ => new AzureSecretClient(
                new Uri(configuration["vaultB"]),
                new DefaultAzureCredential()));

            services.AddSingleton<ISecretManagerFactory<AzureSecretClient>>(
                provider => new KeyVaultManagerFactory(
                    provider.GetServices<AzureSecretClient>()
                        .First(client => client.VaultUri.Equals(configuration["vaultA"])),
                    provider.GetServices<AzureSecretClient>()
                        .First(client => client.VaultUri.Equals(configuration["vaultB"])),
                    configuration["vaultA"],
                    configuration["vaultB"]));

            services.AddScoped<ISecretManager, KeyVaultManager>();
        }
    }
}
