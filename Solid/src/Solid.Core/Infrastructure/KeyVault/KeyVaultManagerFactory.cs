namespace Solid.Core.Infrastructure.KeyVault
{
    using Azure.Security.KeyVault.Secrets;
    using Solid.Core.Contracts;
    using System;

    public class KeyVaultManagerFactory : ISecretManagerFactory<SecretClient>
    {
        private readonly SecretClient clientA = null;
        private readonly SecretClient clientB = null;

        private readonly string vaultA = null;
        private readonly string vaultB = null;

        public KeyVaultManagerFactory(
            SecretClient clientA,
            SecretClient clientB,
            string vaultA,
            string vaultB)
        {
            this.clientA = clientA;
            this.clientB = clientB;
            
            this.vaultA = vaultA;
            this.vaultB = vaultB;
        }

        public SecretClient GetClient(string storeName)
        {
            if (storeName.Equals(this.vaultA))
            {
                return this.clientA;
            }

            if (storeName.Equals(this.vaultB))
            {
                return this.clientB;
            }

            throw new InvalidOperationException($"Invalid store: {storeName}.");
        }
    }
}
