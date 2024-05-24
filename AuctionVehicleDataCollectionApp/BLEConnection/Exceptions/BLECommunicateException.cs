namespace AuctionVehicleDataCollectionApp.BLEConnection.Exceptions
{
    public class BLECommunicateException : Exception
    {
        public BLECommunicateException(string message) : base(message) { }

        public BLECommunicateException(string message, Exception innerException) : base(message, innerException) { }
    }
}