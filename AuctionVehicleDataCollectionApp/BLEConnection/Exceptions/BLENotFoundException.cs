namespace AuctionVehicleDataCollectionApp.BLEConnection.Exceptions
{
    public class BLENotFoundException : Exception
    {
        public BLENotFoundException(string? message) : base(message) { }
        public BLENotFoundException(string? message, Exception inner) : base(message, inner) { }
    }
}
