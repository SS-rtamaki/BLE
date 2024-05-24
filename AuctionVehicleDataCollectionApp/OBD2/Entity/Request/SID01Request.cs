using AuctionVehicleDataCollectionApp.OBD2.Exceptions;

namespace AuctionVehicleDataCollectionApp.OBD2.Entity.Request
{
    /// <summary>
    /// SID01通信リクエストクラス
    /// </summary>
    public class SID01Request : OBD2Request
    {
        /// <summary>
        /// PID
        public string Pid { get; private set; }
        /// </summary>

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="canId">CANID</param>
        /// <param name="pid">PID</param>
        public SID01Request(string canId, string pid) : base(canId, "0322" + pid)
        {
            // pidは2文字(1バイト)
            if (pid.Length != 2)
            {
                throw new OBD2Exception("SID01RequestPIDIsInvalid");
            }
            Pid = pid;
        }
    }



}
