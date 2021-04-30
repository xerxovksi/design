namespace Solid.Core.Infrastructure
{
    using Azure.Security.KeyVault.Secrets;
    using Solid.Core.Contracts;
    using System;
    using System.Threading.Tasks;

    public class KeyVaultManager : ISecretManager
    {
        private readonly ISecretManagerFactory<SecretClient> secretManagerFactory = null;

        public KeyVaultManager(ISecretManagerFactory<SecretClient> secretManagerFactory)
        {
            this.secretManagerFactory = secretManagerFactory;
        }

        public async Task<string> GetSecretAsync(string storeName, string secretName)
        {
            var client = this.secretManagerFactory.GetClient(storeName);

            try
            {
                var secret = await client.GetSecretAsync(secretName);
                return secret?.Value?.Value;
            }
            catch
            {
                return null;
            }
        }

        public Task SetSecretAsync(string storeName, string secretName, string secretValue)
        {
            throw new NotImplementedException();
        }
    }
}
