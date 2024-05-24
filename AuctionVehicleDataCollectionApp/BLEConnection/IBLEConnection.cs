using AuctionVehicleDataCollectionApp.OBD2.ELM327;

namespace AuctionVehicleDataCollectionApp.BLEConnection
{    interface IBLEConnection
    {
        string Name { get; }
        bool IsConnected { get; }
        static abstract Task<IELM327BLEInterface> CreateConnectionAsync(CancellationToken token);
        Task<string> WriteWithResponseAsync(string message, CancellationToken token);
        static abstract void DisConnect(IELM327BLEInterface connection);
        void Dispose();
    }
}
