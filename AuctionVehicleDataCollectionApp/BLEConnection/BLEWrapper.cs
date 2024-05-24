using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Maui.Controls;
using Plugin.BLE;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using Plugin.BLE.Abstractions.Exceptions;
using AuctionVehicleDataCollectionApp.BLEConnection.Exceptions;
using AuctionVehicleDataCollectionApp.Common;


namespace AuctionVehicleDataCollectionApp.BLEConnection
{
    internal static class BLEWrapper
    {
        private static MyLogger logger = new MyLogger(typeof(BLEWrapper));

        private static readonly IBluetoothLE ble = CrossBluetoothLE.Current;
        private static readonly IAdapter adapter = CrossBluetoothLE.Current.Adapter;

        public static async Task<IDevice> ScanForDeviceAsync(string deviceName, int timeout = 20000, CancellationToken token = default)
        {
            //検索タイムアウト
            adapter.ScanTimeout = timeout;                            
            var tcs = new TaskCompletionSource<IDevice>();

            /// <summary>
            /// 検索発見イベント
            /// </summary>
            /// <param name="sender">送信元</param>
            /// <param name="e">イベント内容</param>
            void BLE_DeviceDiscoverd(object? sender, Plugin.BLE.Abstractions.EventArgs.DeviceEventArgs e)
            {
                IDevice discoveredDevice = e.Device;

                //指定したデバイス名ならば
                if (discoveredDevice.Name == deviceName)
                {
                    Debug.WriteLine("指定したデバイスを発見");
                                           
                    adapter.StopScanningForDevicesAsync();
                    //接続
                    adapter.ConnectToDeviceAsync(discoveredDevice);    
                    tcs.SetResult(discoveredDevice);
                }
                //throw new NotImplementedException();
            }
            adapter.DeviceDiscovered += BLE_DeviceDiscoverd;       //検索時の発見イベント
            // adapter.ScanTimeoutElapsed += BLE_ScanTimeoutElapsed;  //検索タイムアウトイベント
            // adapter.DeviceConnectionLost += _adapter_DeviceConnectionLost; //デバイスロスト
            // adapter.DeviceConnected += _adapter_DeviceConnected;           //接続イベント
            // adapter.DeviceDisconnected += _adapter_DeviceDisconnected;     //切断イベント

            await Scan();   //検索開始
            await Task.WhenAny(tcs.Task, Task.Delay(timeout, token)); 
            return tcs.Task.Result;
        }

        /// <summary>
        /// Peripheralを検索開始
        /// </summary>
        private static async Task Scan()
        {
            if (ble.State == BluetoothState.Off)
            {
                return;
            }
            if (adapter.IsScanning)
            {
                return;
            }

            try
            {
                adapter.ScanMode = Plugin.BLE.Abstractions.Contracts.ScanMode.Balanced;
                await adapter.StartScanningForDevicesAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }





        /// <summary>
        /// 指定GUIDのサービスを取得する。
        /// </summary>
        /// <param name="device">BLEデバイス</param>
        /// <param name="guid">GUID</param>
        /// <returns>サービスインスタンス</returns>
        /// <exception cref="BLENotFoundException"></exception>
        public static async Task<IService> GetServiceAsync(IDevice device, string serviceId, CancellationToken token = default)
        {
            logger.Debug($"Start GetServiceAsync device: {device.Name}");
            var retryCount = 6;
            for (var i = 0; i < retryCount; i++)
            {
                logger.Trace($"retryCount {i}");

                // tokenを合成
                var cts = CancellationTokenSource.CreateLinkedTokenSource(token);
                cts.CancelAfter(10000);

                try
                {
                    var service = await device.GetServiceAsync(new Guid(serviceId)).WaitAsync(cts.Token);

                    if (service is IService acquiredService)
                    {
                        logger.Debug($"End   GetServiceAsync device: {device.Name}");
                        return acquiredService;
                    }
                    throw new ServiceNotFoundException("service not found.");
                }
                catch (OperationCanceledException ex) when (ex.CancellationToken == cts.Token)
                {
                    if (!token.IsCancellationRequested)
                    {
                        // タイムアウト
                        logger.Debug($"{i} timeout");
                    }
                }

                token.ThrowIfCancellationRequested();
                await Task.Delay(100, token);
            }
            logger.Warn($"service not found");
            throw new ServiceNotFoundException("service not found.");
        }

        /// <summary>
        /// 指定GUIDのキャラクタリスティックを取得する。
        /// </summary>
        /// <param name="service">サービス</param>
        /// <returns>キャラクタリスティックインスタンス</returns>
        /// <exception cref="BLENotFoundException"></exception>
        public static async Task<ICharacteristic> GetReadCharacteristicAsync(IService service, CancellationToken token = default)
        {
            logger.Debug($"Start GetCharacteristicAsync serviceId: {service.Id}");
            var retryCount = 6;
            for (var i = 0; i < retryCount; i++)
            {
                logger.Trace($"retryCount {i}");

                // tokenを合成
                var cts = CancellationTokenSource.CreateLinkedTokenSource(token);
                cts.CancelAfter(10000);

                try
                {
                    var characteristics = await service.GetCharacteristicsAsync().WaitAsync(cts.Token);
                    if (characteristics is null)
                    {
                        throw new CharacteristicNotFoundException("characteristic not found.");
                    }
                    foreach (var characteristic in characteristics)
                    {
                        if (characteristic.CanUpdate)
                        {
                            logger.Debug($"Start GetReadCharacteristicAsync serviceId: {service.Id}");

                            return characteristic;
                        }
                    }
                }
                catch (OperationCanceledException ex) when (ex.CancellationToken == cts.Token)
                {
                    if (!token.IsCancellationRequested)
                    {
                        // タイムアウト
                        logger.Debug($"{i} timeout");
                    }
                }
            }
            throw new CharacteristicNotFoundException("characteristic not found.");
        }

        /// <summary>
        /// 指定GUIDのキャラクタリスティックを取得する。
        /// </summary>
        /// <param name="service">サービス</param>
        /// <returns>キャラクタリスティックインスタンス</returns>
        /// <exception cref="BLENotFoundException"></exception>
        public static async Task<ICharacteristic> GetWriteCharacteristicAsync(IService service, CancellationToken token = default)
        {
            logger.Debug($"Start GetWriteCharacteristicAsync serviceId: {service.Id}");
            var retryCount = 6;
            for (var i = 0; i < retryCount; i++)
            {
                logger.Trace($"retryCount {i}");

                // tokenを合成
                var cts = CancellationTokenSource.CreateLinkedTokenSource(token);
                cts.CancelAfter(10000);

                try
                {
                    var characteristics = await service.GetCharacteristicsAsync().WaitAsync(cts.Token);
                    if (characteristics is null)
                    {
                        throw new CharacteristicNotFoundException("characteristic not found.");
                    }
                    foreach (var characteristic in characteristics)
                    {
                        if (characteristic.CanUpdate)
                        {
                            logger.Debug($"Start GetWriteCharacteristicAsync serviceId: {service.Id}");

                            return characteristic;
                        }
                    }
                }
                catch (OperationCanceledException ex) when (ex.CancellationToken == cts.Token)
                {
                    if (!token.IsCancellationRequested)
                    {
                        // タイムアウト
                        logger.Debug($"{i} timeout");
                    }
                }
            }
            throw new CharacteristicNotFoundException("characteristic not found.");
        }
    }
}


