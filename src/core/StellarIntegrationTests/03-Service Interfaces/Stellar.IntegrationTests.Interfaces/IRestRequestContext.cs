namespace Stellar.IntegrationTests.Interfaces
{
    public interface IRestRequestContext<T>
    {
        bool IsRawJson { get; set; }

        T Data { get; set; }
        void MergeJson(string json);
    }
   
}
