using AuctionVehicleDataCollectionApp.OBD2.ELM327;

namespace AuctionVehicleDataCollectionApp.BLEConnection
{
    /// <summary>
    /// VEEPEAKクラス
    /// </summary>
    public partial class Veepeak : IELM327BLEInterface
    {
        public async Task<ELM327Response> SendMessageAsync(string message, CancellationToken token)
        {
            try
            {
                var response = await WriteWithResponseAsync(message, token);
                return new ELM327Response(response);
            }
            finally
            {
                
                
            }
        }
    }
}