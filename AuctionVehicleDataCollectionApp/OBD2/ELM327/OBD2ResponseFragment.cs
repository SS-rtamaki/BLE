using System.Text.RegularExpressions;

namespace AuctionVehicleDataCollectionApp.OBD2.ELM327
{
    /// <summary>
    /// CAN通信の応答値の断片クラス
    /// </summary>
    public class OBD2ResponseFragment
    {
        /// <summary>
        /// 正常なデータであるか
        /// </summary>
        public bool Successs
        {
            get
            {
                return ResponseFrames.Any() &&
                    ResponseFrames.All((message) => Regex.IsMatch(message, "^[0-9a-fA-F]{3}( [0-9a-fA-F]{2}){3,8}$"));
            }

        }

        /// <summary>
        /// 1フレームごとの応答値の配列
        /// </summary>
        public string[] ResponseFrames { get; init; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="response">CAN通信の応答値</param>
        public OBD2ResponseFragment(string response)
        {
            ResponseFrames = response.Split("\r")
                .Select((frame) => frame.Trim())
                .Where((frame) => !string.IsNullOrEmpty(frame))
                .ToArray();
        }

    }
}