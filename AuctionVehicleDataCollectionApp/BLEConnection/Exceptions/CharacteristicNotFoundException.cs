namespace AuctionVehicleDataCollectionApp.BLEConnection.Exceptions
{
    public class CharacteristicNotFoundException : BLENotFoundException
    {
        public CharacteristicNotFoundException(string? message) : base(message) { }
        public CharacteristicNotFoundException(string? message, Exception inner) : base(message, inner) { }
    }
}
