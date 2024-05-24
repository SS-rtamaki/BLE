namespace AuctionVehicleDataCollectionApp.BLEConnection.Exceptions
{
    public class AdvertiseNotFoundException : Exception
    {
        public AdvertiseNotFoundException(string? message) : base(message) { }
        public AdvertiseNotFoundException(string? message, Exception inner) : base(message, inner) { }
    }
}
