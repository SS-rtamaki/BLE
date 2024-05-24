namespace AuctionVehicleDataCollectionApp.OBD2.Entity.Request
{
    /// <summary>
    /// OBD2リクエストクラス
    /// </summary>
    public class OBD2Request
    {

        /// <summary>
        /// CANID
        /// </summary>
        public string CanId { get; init;}

        /// <summary>
        /// リクエストメッセージ
        /// </summary>
        public virtual string Message { get; private init; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message">リクエストメッセージ</param>
        public OBD2Request(string canId, string message)
        {
            CanId = canId;
            Message = message;
        }
    }
}
