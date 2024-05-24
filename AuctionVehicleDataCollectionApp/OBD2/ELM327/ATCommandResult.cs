namespace AuctionVehicleDataCollectionApp.OBD2.ELM327
{
    /// <summary>
    /// ATCommandの応答値クラス
    /// </summary>
    public class ATCommandResult
    {
        /// <summary>
        /// 応答メッセージ
        /// </summary>
        public string Message { get; init; }

        /// <summary>
        /// 正常な応答か
        /// </summary>
        public bool Success
        {
            get
            {
                return Message.Contains("OK") || Message.Contains("ELM327 v2.2");
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message">応答メッセージ</param>
        public ATCommandResult(string message)
        {
            Message = message;
        }
    }
}