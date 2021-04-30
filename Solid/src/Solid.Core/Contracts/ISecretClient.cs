namespace Solid.Core.Contracts
{
    using System.Threading.Tasks;

    public interface ISecretClient
    {
        string StoreName { get; }

        Task<string> GetSecretAsync(string secretName);

        Task SetSecretAsync(string secretName, string secretValue);
    }
}
