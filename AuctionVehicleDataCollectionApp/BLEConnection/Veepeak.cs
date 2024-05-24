using AuctionVehicleDataCollectionApp.BLEConnection.Exceptions;
using Plugin.BLE;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using Plugin.BLE.Abstractions.Exceptions;
using System.Text;
using AuctionVehicleDataCollectionApp.Common;
using static AuctionVehicleDataCollectionApp.BLEConnection.BLEWrapper;
using AuctionVehicleDataCollectionApp.OBD2.ELM327;


namespace AuctionVehicleDataCollectionApp.BLEConnection
{
    /// <summary>
    /// VEEPEAKクラス
    /// </summary>
    public partial class Veepeak : IBLEConnection, IDisposable
    {
        private static MyLogger logger = new MyLogger(typeof(Veepeak));
        private static readonly string beanName = "VEEPEAK";
        private static readonly string serviceId = "0000fff0-0000-1000-8000-00805f9b34fb";
        //private static readonly string readCharacteristicId = "0000fff1-0000-1000-8000-00805f9b34fb";
        //private static readonly string writeCharacteristicId = "0000fff2-0000-1000-8000-00805f9b34fb";

        private readonly IDevice device;
        private readonly IService service;
        private readonly ICharacteristic readCharacteristic;
        private readonly ICharacteristic writeCharacteristic;

        /// <summary>排他ロック用オブジェクト</summary>
        private SemaphoreSlim lockObj = new SemaphoreSlim(1, 1);

        private bool disposed;

        /// <summary>
        /// デバイスID
        /// </summary>
        public string Name
        {
            get
            {
                return device.Name;
            }
        }

        /// <summary>
        /// 接続状態
        /// </summary>
        public bool IsConnected
        {
            get
            {
                return device.State == DeviceState.Connected;
            }
        }

        private Veepeak(IDevice device, IService service, ICharacteristic readCharacteristic, ICharacteristic writeCharacteristic)
        {
            this.device = device;
            this.service = service;
            this.readCharacteristic = readCharacteristic;
            this.writeCharacteristic = writeCharacteristic;
        }


        ~Veepeak()
        {
            Dispose();
        }

        static public void DisConnect(IELM327BLEInterface veepeak)
        {
            veepeak.Dispose();
        }

        /// <summary>
        /// Veepeakに接続する。
        /// </summary>
        /// <param name="addr">デバイスアドレス</param>
        /// <param name="token">キャンセルトークン</param>
        /// <returns>Veepeakオブジェクト</returns>
        /// <exception cref="BLENotFoundException"></exception>
        static public async Task<IELM327BLEInterface> CreateConnectionAsync(CancellationToken token = default)
        {
            logger.Debug($"Start CreateConnectionAsync");
            try
            {

                var veepeak = await CreateConnectionAsync_inner(token);
                return veepeak;
            }
            catch (BLENotFoundException ex)
            {
                logger.Warn($"failed to connect to Veepeak");
                logger.Warn($"{ex.Message} {ex.StackTrace}");
                if (ex is AdvertiseNotFoundException)
                {
                    throw;
                }

                try
                {
                    var veepeak = await CreateConnectionAsync_inner(token);
                    return veepeak;
                }
                catch (Exception e) when (e is not OperationCanceledException)
                {
                    logger.Warn($"failed to connect to Veepeak");
                    logger.Warn($"{e.Message} {e.StackTrace}");
                    throw;
                }
            }
            finally
            {
                logger.Debug($"End   CreateConnectionAsync");
            }
        }

        /// <summary>
        /// アドバタイズ通信からキャラクタリスティック取得までの処理
        /// </summary>
        /// <param name="token">キャンセルトークン</param>
        /// <returns>Veepeakオブジェクト</returns>

        static private async Task<Veepeak> CreateConnectionAsync_inner(CancellationToken token = default)
        {
            logger.Debug($"Start CreateConnectionAsync_inner");
            IDevice? device = null;
            try
            {
                device = await ScanForDeviceAsync(beanName, token: token);
                var service = await GetServiceAsync(device, serviceId, token);
                var readCharacteristic = await GetReadCharacteristicAsync(service, token);
                var writeCharacteristic = await GetWriteCharacteristicAsync(service, token);
                // await readCharacteristic.WriteClientCharacteristicConfigurationDescriptorAsync(GattClientCharacteristicConfigurationDescriptorValue.Notify);
                token.ThrowIfCancellationRequested();
                return new Veepeak(device, service, readCharacteristic, writeCharacteristic);
            }
            catch
            {
                device?.Dispose();
                throw;
            }
            finally
            {
                logger.Debug($"End   CreateConnectionAsync_inner");
            }
        }

        /// <summary>
        /// dispose
        /// </summary>
        public void Dispose()
        {
            if (!disposed)
            {
                service.Dispose();
                device.Dispose();
                disposed = true;
            }
        }

        /// <summary>
        /// BLEに要求を送信する。
        /// 応答文字列に終端文字">"は含まれない。
        /// </summary>
        /// <param name="request">要求文字列</param>
        /// <returns>応答文字列</returns>
        /// <exception cref="BLECommunicateException"></exception>
        public async Task<string> WriteWithResponseAsync(string message, CancellationToken token = default)
        {
            logger.Debug($"Start WriteWithResponseAsync message: {message}");
            logger.Trace($"VEEPEAK: send    {message}");
            if (!message.EndsWith('\r'))
            {
                message += "\r";
            }

            var tcs = new TaskCompletionSource<string>();
            var responseBuffer = new StringBuilder();

            void notifyEventHandler(object sender, Plugin.BLE.Abstractions.EventArgs.CharacteristicUpdatedEventArgs args)
            {
                var str = args.Characteristic.StringValue;

                // using var streamReader = new StreamReader(notifyStream);

                // var str = streamReader.ReadToEnd();

                responseBuffer.Append(str);

                // 文末に終端文字が入っている場合は、メッセージ受信完了

                if (str.EndsWith(">"))

                {
                    readCharacteristic.ValueUpdated -= notifyEventHandler;
                    var response = responseBuffer.ToString().Replace("\r>", "");
                    tcs.SetResult(response);
                }

            }

            token.ThrowIfCancellationRequested();

            // キャンセル要求があった場合も、VEEPEAKとのメッセージプロトコルは崩さない

            // 読み込み通知購読登録
            readCharacteristic.ValueUpdated += notifyEventHandler;

            var sendData = Encoding.ASCII.GetBytes(message);
            var writeResult = await Write(sendData);

            if (!writeResult)
            {
                readCharacteristic.ValueUpdated -= notifyEventHandler;
                logger.Warn($"BLE write Error");
                token.ThrowIfCancellationRequested();
                throw new BLECommunicateException("BLE write Error");
            }

            var cts = new CancellationTokenSource(10000);
            cts.Token.Register(() => { tcs.TrySetCanceled(); });

            string readResult;
            try
            {
                readResult = await tcs.Task;
                logger.Trace($"VEEPEAK: recieve {readResult}");
            }
            catch (OperationCanceledException ex)
            {
                logger.Warn("message canceled!");
                token.ThrowIfCancellationRequested();
                throw new BLECommunicateException("send timout");
            }

            token.ThrowIfCancellationRequested();
            logger.Debug($"End   WriteWithResponseAsync message: {message}");
            return readResult;
        }

        /// <summary>
        /// データ送信
        /// </summary>
        /// <param name="bdata">接続先にデータを送信する</param>
        async public Task<bool> Write(byte[] bdata)
        {
            try
            {
                await lockObj.WaitAsync();  //排他ロック開始

                if (!IsConnected)
                {
                    //切断状態だったら無視
                    logger.Debug("切断中の送信");
                    return false;
                }
                if (service is null)   //サービスが確保されていなかったら
                {
                    logger.Debug("サービス接続前");
                    return false;             //終了
                }

                if (writeCharacteristic.CanWrite == true)
                {
                    int rslt = await writeCharacteristic.WriteAsync(bdata);  // ★ver 2.13から変わった	戻り値がbool -> int(0=成功)
                    if (rslt != 0)
                    {
                        DisConnect(this);//送信失敗したら切断
                        return false;
                    }
                }
                else 
                {
                    return false;
                }

                return true;
            }
            catch (Exception e)
            {
                //送信失敗したら切断
                DisConnect(this);
                return false;
                //throw new Exception(e.Message);
            }
            finally
            {
                lockObj.Release();          //排他ロック終了
            }
        }
    }
}
