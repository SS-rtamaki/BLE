using MauiBleTest.BLEUse;

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
            var device = new Veepeak();
            List<string> res = new();
            // ATコマンドを送信
            var resultATZ = await device.WriteWithResponseAsync("ATZ");
            var resultATE0 = await device.WriteWithResponseAsync("ATE0");
            var resultATSP6 = await device.WriteWithResponseAsync("ATSP6");
            var resultATH1 = await device.WriteWithResponseAsync("ATH1");
            var resultATR1 = await device.WriteWithResponseAsync("ATR1");
            var resultATCAF0 = await device.WriteWithResponseAsync("ATCAF0");

            res.Add(resultATZ);
            res.Add(resultATE0);
            res.Add(resultATSP6);
            res.Add(resultATH1);
            res.Add(resultATR1);
            res.Add(resultATCAF0);

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