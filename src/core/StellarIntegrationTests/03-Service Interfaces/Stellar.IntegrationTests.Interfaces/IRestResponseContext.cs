namespace Stellar.IntegrationTests.Interfaces
{
    public interface IRestResponseContext<T>
    {
        T Data { get; set; }
    }
}
