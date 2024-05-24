using AuctionVehicleDataCollectionApp.OBD2.Exceptions;

namespace AuctionVehicleDataCollectionApp.OBD2.Entity.Request
{
    /// <summary>
    /// SID22通信要求クラス
    /// </summary>
    public class SID22Request : OBD2Request
    {
        /// <summary>
        /// DID
        public string Did { get; private set; }
        /// </summary>

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="canId">CANID</param>
        /// <param name="did">DID</param>
        public SID22Request(string canId, string did) : base(canId, "0322" + did)
        {
            // didは4文字(2バイト)
            if (did.Length != 4)
            {
                throw new OBD2Exception("SID22RequestDIDIsInvalid");
            }
            Did = did;
        }
    }



}
