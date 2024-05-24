namespace AuctionVehicleDataCollectionApp.OBD2.ELM327
{
    /// <summary>
    /// ELM327の応答値クラス
    /// </summary>
    public class ELM327Response
    {
        /// <summary>
        /// ELM327の応答値
        /// </summary>
        public string ElmResponse { get; init; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="elmResponse">ELM327の応答値</param>
        public ELM327Response(string elmResponse)
        {
            ElmResponse = elmResponse;
        }

        /// <summary>
        /// ELM327の応答値をATCommandの応答値として変換
        /// </summary>
        /// <returns>ATCommandResultインスタンス</returns>
        public ATCommandResult ParseAsAtCommandResult()
        {
            return new ATCommandResult(ElmResponse);
        }

        /// <summary>
        /// ELM327の応答値をCAN通信の応答値として変換
        /// </summary>
        /// <returns>OBD2ResponseFragmentインスタンス</returns>
        public OBD2ResponseFragment ParseAsOBD2ResponseFragment()
        {
            return new OBD2ResponseFragment(ElmResponse);
        }


    }
}