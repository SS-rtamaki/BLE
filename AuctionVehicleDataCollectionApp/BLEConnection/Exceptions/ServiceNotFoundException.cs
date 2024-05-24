namespace AuctionVehicleDataCollectionApp.BLEConnection.Exceptions
{
    public class ServiceNotFoundException : BLENotFoundException
    {
        public ServiceNotFoundException(string? message) : base(message) { }
        public ServiceNotFoundException(string? message, Exception inner) : base(message, inner) { }
    }
}
