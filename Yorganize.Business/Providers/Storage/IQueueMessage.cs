namespace Yorganize.Business.Providers.Storage
{
    public interface IQueueMessage
    {
        string MessageId { get; set; }
        string PopReceipt { get; set; }
        int DequeueCount { get; set; }
    }
}
