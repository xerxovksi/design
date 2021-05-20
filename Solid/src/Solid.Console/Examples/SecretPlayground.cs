namespace Solid.Console.Examples
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Solid.Core.Contracts;
    using Solid.Core.Infrastructure.LocalVault;
    using Solid.Core.Infrastructure.KeyVault;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Azure.Identity;

    using AzureSecretClient = Azure.Security.KeyVault.Secrets.SecretClient;

    public class SecretPlayground
    {
        private readonly Action<string> log = null;

        public SecretPlayground(Action<string> log) =>
            this.log = log;

        public async Task LocalPlayground()
        {
            var services = new ServiceCollection();
            this.RegisterLocalDependencies(services);

            var provider = services.BuildServiceProvider();
            var secretManager = provider.GetService<ISecretManager>();

            this.log(await secretManager.GetSecretAsync("vaultA", "Username"));
            this.log(await secretManager.GetSecretAsync("vaultA", "Password"));
            await secretManager.SetSecretAsync("vaultA", "Password", "JusticeLeague");
            this.log(await secretManager.GetSecretAsync("vaultA", "Password"));

            this.log(await secretManager.GetSecretAsync("vaultB", "Username"));
            this.log(await secretManager.GetSecretAsync("vaultB", "Password"));
            await secretManager.SetSecretAsync("vaultB", "Password", "Avengers");
            this.log(await secretManager.GetSecretAsync("vaultB", "Password"));
        }

        public async Task AzurePlayground()
        {
            var services = new ServiceCollection();
            this.RegisterAzureDependencies(services);

            var provider = services.BuildServiceProvider();
            var configuration = provider.GetService<IConfiguration>();
            var secretManager = provider.GetService<ISecretManager>();

            this.log(
                await secretManager.GetSecretAsync(configuration["vaultA"], "Username"));
            this.log(
                await secretManager.GetSecretAsync(configuration["vaultB"], "Password"));
        }

        private void RegisterLocalDependencies(IServiceCollection services)
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

        private void RegisterAzureDependencies(IServiceCollection services)
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
