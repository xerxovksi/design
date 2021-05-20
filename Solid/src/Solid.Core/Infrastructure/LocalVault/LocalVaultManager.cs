namespace Solid.Core.Infrastructure.LocalVault
{
    using Solid.Core.Contracts;
    using System.Threading.Tasks;

    public class LocalVaultManager : ISecretManager
    {
        private readonly ISecretManagerFactory<ISecretClient> secretManagerFactory = null;

        public LocalVaultManager(ISecretManagerFactory<ISecretClient> secretManagerFactory)
        {
            this.secretManagerFactory = secretManagerFactory;
        }

        public async Task<string> GetSecretAsync(string storeName, string secretName)
        {
            var client = this.secretManagerFactory.GetClient(storeName);
            return await client.GetSecretAsync(secretName);
        }

        public async Task SetSecretAsync(string storeName, string secretName, string secretValue)
        {
            var client = this.secretManagerFactory.GetClient(storeName);
            await client.SetSecretAsync(secretName, secretValue);
        }
    }
}
