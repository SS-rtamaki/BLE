using AuctionVehicleDataCollectionApp.BLEConnection;
using AuctionVehicleDataCollectionApp.BLEConnection.Exceptions;
using AuctionVehicleDataCollectionApp.Common;
// using AuctionVehicleDataCollectionApp.OBD2.Dummy;
using AuctionVehicleDataCollectionApp.OBD2.ELM327;
using AuctionVehicleDataCollectionApp.OBD2.Exceptions;

namespace AuctionVehicleDataCollectionApp.OBD2
{
    public class OBD2ConnectionManager
    {
        private static MyLogger logger = new(typeof(OBD2ConnectionManager));

        private OBD2ConnectionManager()
        {
            
        }
        public static OBD2ConnectionManager Instance { get; } = new();

        public async Task<IOBD2ConnectionInterface> GetConnectionAsync(CancellationToken token = default)
        {
            //return await DummyConnectionInterface.CreateAsyncFromAsync(AppConfig.DummyDataPath);
            logger.Debug($"Start GetConnectionAsync");
            try
            {
                var elmInterface = await CreateVeepeakConnectionAsync(token);
                var connection = new ELM327MessagingProtocol(elmInterface);
                var initialized = await connection.InitializeAsync(token);
                if (initialized)
                {
                    logger.Debug($"connection established.");
                    return connection;
                }
                else
                {
                    logger.Warn($"connection cannot initialize");
                    throw new OBD2Exception("connection cannot initialize.");
                }
            }
            catch (KeyNotFoundException ex)
            {
                logger.Warn($"device is not found");
                logger.Warn($"{ex.Message} {ex.StackTrace}");
                throw new BLENotFoundException($"device is not found.", ex);
            }
            catch (BLENotFoundException ex)
            {
                logger.Warn($"device is not found");
                logger.Warn($"{ex.Message} {ex.StackTrace}");
                throw;
            }
            finally
            {
                logger.Debug($"End   GetConnectionAsync");
            }
        }


        private async Task<IELM327BLEInterface> CreateVeepeakConnectionAsync(CancellationToken token = default)
        {
            logger.Debug($"Start CreateVeepeakConnectionAsync");
            try
            {
                return await Veepeak.CreateConnectionAsync(token);
            }
            finally
            {
                logger.Debug($"End   CreateVeepeakConnectionAsync");
            }
        }
    }
}