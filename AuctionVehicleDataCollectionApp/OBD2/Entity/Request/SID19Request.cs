namespace AuctionVehicleDataCollectionApp.OBD2.Entity.Request
{
    /// <summary>
    /// SID19リクエストクラス
    /// </summary>
    public class SID19Request : OBD2Request
    {
        /// <summary>
        /// リクエストメッセージ
        /// </summary>
        public override string Message
        {
            get
            {
                return Option.OptionDataLength + Option.OptionString;
            }
        }

        /// <summary>
        /// SID19オプション情報
        /// </summary>
        public SID19Option Option { get; init; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="canId">CANID</param>
        /// <param name="option">SID19Optionオブジェクト</param>
        public SID19Request(string canId, SID19Option option) : base(canId, "")
        {
            Option = option;
        }
    }


}
