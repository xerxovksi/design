namespace Solid.Core.Infrastructure
{
    using Solid.Core.Contracts;
    using System;

    public class LocalVaultManagerFactory : ISecretManagerFactory<ISecretClient>
    {
        private readonly ISecretClient clientA = null;
        private readonly ISecretClient clientB = null;

        public LocalVaultManagerFactory(ISecretClient clientA, ISecretClient clientB)
        {
            this.clientA = clientA;
            this.clientB = clientB;
        }

        public ISecretClient GetClient(string storeName) =>
            storeName switch
            {
                "vaultA" => this.clientA,
                "vaultB" => this.clientB,
                _ => throw new InvalidOperationException($"Invalid store: {storeName}.")
            };
    }
}
