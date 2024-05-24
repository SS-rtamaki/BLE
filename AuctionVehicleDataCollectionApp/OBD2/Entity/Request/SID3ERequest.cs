namespace AuctionVehicleDataCollectionApp.OBD2.Entity.Request
{
    /// <summary>
    /// 3E通信リクエストクラス
    /// </summary>
    public class SID3ERequest : OBD2Request
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="canId">CANID</param>
        public SID3ERequest(string canId) : base(canId, "023E00") { }

    }
}
