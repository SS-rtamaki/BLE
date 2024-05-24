using AuctionVehicleDataCollectionApp.OBD2.Entity.Response;
using AuctionVehicleDataCollectionApp.OBD2.Entity.Request;

namespace AuctionVehicleDataCollectionApp.OBD2
{
    public interface IOBD2ConnectionInterface : IDisposable 
    {
        Task<OBD2Response> SendMessageAsync(OBD2Request request) 
        {
            return SendMessageAsync(request, CancellationToken.None);
        }  

        Task<OBD2Response> SendMessageAsync(OBD2Request request, CancellationToken token);
    }
}
