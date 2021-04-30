namespace Solid.Core.Contracts
{
    using System.Threading.Tasks;

    public interface ISecretManager
    {
        Task<string> GetSecretAsync(string storeName, string secretName);

        Task SetSecretAsync(string storeName, string secretName, string secretValue);
    }
}
