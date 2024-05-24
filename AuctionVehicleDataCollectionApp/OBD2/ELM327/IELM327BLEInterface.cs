namespace AuctionVehicleDataCollectionApp.OBD2.ELM327
{
    public interface IELM327BLEInterface : IDisposable 
    {
        bool IsConnected { get; }
        Task<ELM327Response> SendMessageAsync(string message, CancellationToken token);

        Task<ELM327Response> SendMessageAsync(string message) 
        {
            return SendMessageAsync(message, CancellationToken.None);
        }  
    }
}
