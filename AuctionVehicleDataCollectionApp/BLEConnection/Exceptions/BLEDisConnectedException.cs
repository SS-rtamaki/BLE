namespace AuctionVehicleDataCollectionApp.BLEConnection.Exceptions
{
    public class BLEDisconnectedException : Exception
    {
        public BLEDisconnectedException(string message) : base(message) { }

        public BLEDisconnectedException(string? message, Exception innerException) : base(message, innerException) { }
    }
}