namespace AuctionVehicleDataCollectionApp.OBD2.Exceptions
{
    public class OBD2Exception : Exception
    {
        public OBD2Exception() { }
        public OBD2Exception(string message) : base(message) { }
        public OBD2Exception(string? message, Exception innerException) : base(message, innerException) { }
    }
}