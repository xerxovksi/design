namespace Solid.Core.Infrastructure.LocalVault
{
    using Solid.Core.Contracts;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class LocalVaultClient : ISecretClient
    {
        private readonly Dictionary<string, string> secrets = new Dictionary<string, string>();

        public string StoreName { get; }

        public LocalVaultClient(string storeName)
        {
            this.StoreName = storeName;
            this.secrets.Add("Username", $"{storeName}-Abhishek");
            this.secrets.Add("Password", $"{storeName}-HelloWorld");
        }

        public async Task<string> GetSecretAsync(string secretName) =>
            this.secrets.TryGetValue(secretName, out var secretValue)
            ? await Task.FromResult(secretValue)
            : string.Empty;

        public async Task SetSecretAsync(string secretName, string secretValue) =>
            this.secrets[secretName] = await Task.FromResult($"{this.StoreName}-{secretValue}");
    }
}
