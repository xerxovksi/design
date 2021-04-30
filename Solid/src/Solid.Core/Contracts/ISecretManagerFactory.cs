namespace Solid.Core.Contracts
{
    public interface ISecretManagerFactory<T>
    {
        T GetClient(string storeName);
    }
}
