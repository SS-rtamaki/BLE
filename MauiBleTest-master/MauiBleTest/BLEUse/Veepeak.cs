using System.Text;
using static MauiBleTest.ble.BLEMain;
using Plugin.BLE.Abstractions.Contracts;

// namespace MauiBleTest.BLEUse
// {
// 	public class Veepeak : BLEControlBase
// 	{

// 		private TBLEConnectInfo _bleInfo;

// 		public Veepeak() : base()
// 		{

// 			_bleInfo.TerminalName = "VEEPEAK";

// 			//_bleInfo.UUService = "4fafc201-1fb5-459e-8fcc-c5c9c331914b";
// 			//_bleInfo.UUCharacteristic = "beb5483e-36e1-4688-b7f5-ea07361b26a8";

// 			_bleInfo.UUService = "0000fff0-0000-1000-8000-00805f9b34fb";
// 			_bleInfo.ReadCharacteristic = "0000fff1-0000-1000-8000-00805f9b34fb";
// 			_bleInfo.WriteCharacteristic = "0000fff2-0000-1000-8000-00805f9b34fb";

// 			//BLECtrlLED = new BLEControlESPLED(_bleInfo);
// 			SetBleConnectInfo(_bleInfo);
// 			StartConnect();
// 		}
// 	}
// }


namespace MauiBleTest.BLEUse

{

    /// <summary>

    /// VEEPEAKクラス

    /// </summary>

    public partial class Veepeak : BLEControlBase

    {

        //private static MyLogger logger = new MyLogger(typeof(Veepeak));

        private static readonly string beanName = "VEEPEAK";

        private static readonly string serviceId = "0000fff0-0000-1000-8000-00805f9b34fb";

        private static readonly string readCharacteristicId = "0000fff1-0000-1000-8000-00805f9b34fb";

        private static readonly string writeCharacteristicId = "0000fff2-0000-1000-8000-00805f9b34fb";

        private readonly TBLEConnectInfo _bleInfo;

        public ConnectStat IsConnect 
        { 
            get
            {
                return _IsConnect;
            }
           
        } 

        private ConnectStat _IsConnect;


        private bool disposed;
 
 

        public Veepeak() : base()

        {
            _bleInfo.TerminalName = beanName;
            _bleInfo.UUService = serviceId;
            _bleInfo.ReadCharacteristic = readCharacteristicId;
            _bleInfo.WriteCharacteristic = writeCharacteristicId;

            SetBleConnectInfo(_bleInfo);

            var res = StartConnect();
            switch(res)
            {
                case BLERESULT.OK:
                    _IsConnect = ConnectStat.CONNECT;
                    break;
                case BLERESULT.NG:
                    _IsConnect = ConnectStat.DISCONNECT;
                    break;
                case BLERESULT.BUSY:
                    _IsConnect = ConnectStat.DISCONNECT;
                    break;
            }

        }
 
 
        ~Veepeak()

        {

            // Dispose();

        }
 
        // / <summary>

        // / Veepeakに接続する。

        // / </summary>

        // / <param name="addr">デバイスアドレス</param>

        // / <param name="token">キャンセルトークン</param>

        // / <returns>Veepeakオブジェクト</returns>

        // / <exception cref="BLENotFoundException"></exception>

        // static public async Task<Veepeak> CreateConnectionAsync(ulong? addr = null, CancellationToken token = default)

        // {

        //     logger.Debug($"Start CreateConnectionAsync addr: {addr}");

        //     var deviceAddr = addr ?? await ScanDeviceAddressAsync(beanName, token: token);

        //     try

        //     {

        //         var veepeak = await CreateConnectionAsync_inner(deviceAddr, token);

        //         return veepeak;

        //     }

        //     catch (BLENotFoundException ex)

        //     {

        //         logger.Warn($"failed to connect to Veepeak");

        //         logger.Warn($"{ex.Message} {ex.StackTrace}");

        //         if (ex is AdvertiseNotFoundException)

        //         {

        //             throw;

        //         }
 
        //         try

        //         {

        //             await ClearCache();

        //             var veepeak = await CreateConnectionAsync_inner(deviceAddr, token);

        //             return veepeak;

        //         }

        //         catch (Exception e) when (e is not OperationCanceledException)

        //         {

        //             logger.Warn($"failed to connect to Veepeak");

        //             logger.Warn($"{e.Message} {e.StackTrace}");

        //             throw;

        //         }

        //     }

        //     finally

        //     {

        //         logger.Debug($"End   CreateConnectionAsync addr: {addr}");

        //     }

        // }
 
        /// <summary>

        /// アドバタイズ通信からキャラクタリスティック取得までの処理

        /// </summary>

        /// <param name="addr">デバイスアドレス</param>

        /// <param name="token">キャンセルトークン</param>

        /// <returns>Veepeakオブジェクト</returns>
 
        // static private async Task<Veepeak> CreateConnectionAsync_inner(ulong addr, CancellationToken token = default)

        // {

        //     logger.Debug($"Start CreateConnectionAsync_inner addr: {addr}");

        //     BluetoothLEDevice? device = null;

        //     try

        //     {

        //         device = await ConnectDeviceAsync(addr, token: token);

        //         var service = await GetServiceAsync(device, serviceId, token);

        //         var readCharacteristic = await GetCharacteristicAsync(service, readCharacteristicId, token);

        //         var writeCharacteristic = await GetCharacteristicAsync(service, writeCharacteristicId, token);

        //         await readCharacteristic.WriteClientCharacteristicConfigurationDescriptorAsync(GattClientCharacteristicConfigurationDescriptorValue.Notify);

        //         token.ThrowIfCancellationRequested();

        //         return new Veepeak(device, service, readCharacteristic, writeCharacteristic);

        //     }

        //     catch

        //     {

        //         device?.Dispose();

        //         throw;

        //     }

        //     finally

        //     {

        //         logger.Debug($"End   CreateConnectionAsync_inner addr: {addr}");

        //     }

        // }
 
        /// <summary>

        /// dispose

        /// </summary>

        // public override void Dispose()

        // {

        //     if (!disposed)

        //     {

        //         service.Dispose();

        //         device.Dispose();

        //         disposed = true;

        //     }

        // }
 
        
    }

}
