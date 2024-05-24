using System.Diagnostics;
using AuctionVehicleDataCollectionApp.BLEConnection;

namespace MauiBleTest.ble
{
    public class OBD2ConnectionManager
    {
        public static OBD2ConnectionManager Instance { get; } = new();

        private OBD2ConnectionManager()
        {

        }

        public async Task<List<string>> Test() 
        {
            var device = await Veepeak.CreateConnectionAsync();
            List<string> res = new();

            Debug.WriteLine("Connected");

            // ATコマンドを送信
            var resultATZ = await device.SendMessageAsync("ATZ");
            var resultATE0 = await device.SendMessageAsync("ATE0");
            var resultATSP6 = await device.SendMessageAsync("ATSP6");
            var resultATH1 = await device.SendMessageAsync("ATH1");
            var resultATR1 = await device.SendMessageAsync("ATR1");
            var resultATCAF0 = await device.SendMessageAsync("ATCAF0");

            res.Add(resultATZ.ElmResponse);
            res.Add(resultATE0.ElmResponse);
            res.Add(resultATSP6.ElmResponse);
            res.Add(resultATH1.ElmResponse);
            res.Add(resultATR1.ElmResponse);
            res.Add(resultATCAF0.ElmResponse);

            Console.WriteLine("ATZ: " + resultATZ);
            Console.WriteLine("ATE0: " + resultATE0);
            Console.WriteLine("ATSP6: " + resultATSP6);
            Console.WriteLine("ATH1: " + resultATH1);
            Console.WriteLine("ATR1: " + resultATR1);
            Console.WriteLine("ATCAF0: " + resultATCAF0);

            return res;
        }
    }
}