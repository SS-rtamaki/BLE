using AuctionVehicleDataCollectionApp.OBD2.Entity.Response;
using System.Text.RegularExpressions;

namespace AuctionVehicleDataCollectionApp.OBD2.ELM327
{
    public class OBD2ResponseBuffer
    {
        /// <summary>
        /// 全てのデータが格納されているか
        /// </summary>
        public bool HasEnoughLine
        {
            get
            {
                return GetOBD2Response().IsValid;
            }
        }

        /// <summary>
        /// データの断片を格納するバッファ
        /// </summary>
        private List<OBD2ResponseFragment> buffer;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="hasNTA">拡張CANか</param>
        public OBD2ResponseBuffer(bool hasNTA = false)
        {
            buffer = new List<OBD2ResponseFragment>();
        }

        /// <summary>
        /// データの断片を追加する
        /// </summary>
        /// <param name="data">データの断片</param>
        public void Append(OBD2ResponseFragment data)
        {
            buffer.Add(data);
        }

        /// <summary>
        /// データの断片を集めたバッファを使用してOBD2Responseを作成
        /// </summary>
        /// <returns>OBD2Responseインスタンス</returns>
        public OBD2Response GetOBD2Response()
        {
            if (buffer.Count == 0)
            {
                return OBD2Response.CreateOBD2ResponseEmpty("", ErrorType.NoErrors);
            }


            var canErrorFrames = buffer
                .SelectMany((frag) => frag.ResponseFrames)
                .Where((message) => Regex.IsMatch(message, "CAN ERROR"));

            if (canErrorFrames.Any())
            {
                // CAN ERRORの場合
                return OBD2Response.CreateOBD2ResponseEmpty("", ErrorType.CanError);
            }

            var noDataFrames = buffer
                .SelectMany((frag) => frag.ResponseFrames)
                .Where((message) => Regex.IsMatch(message, "NO DATA"));

            if (noDataFrames.Any())
            {
                // NO DATAの場合
                return OBD2Response.CreateOBD2ResponseEmpty("", ErrorType.NoData);
            }

            // ["(CANID) (8バイトのメッセージ)", "(CANID) (8バイトのメッセージ)", "(CANID) (8バイトのメッセージ)"...]
            var responseFrames = buffer
                .SelectMany((frag) => frag.ResponseFrames)
                .Where((message) => Regex.IsMatch(message, "^[0-9a-fA-F]{3}( [0-9a-fA-F]{2}){3,8}$"));

            if (!responseFrames.Any())
            {
                // CAN ERROR、NO DATA以外の不正なメッセージ
                return OBD2Response.CreateOBD2ResponseEmpty("", ErrorType.NoErrors);
            }

            var responseLines = responseFrames
                .Select((bytes) =>
                {
                    var splitMessage = bytes.Split(" ").ToList();
                    splitMessage.RemoveAt(0);
                    var message = string.Join("", splitMessage);
                    return new OBD2ResponseLine(message);
                })
                .ToArray();

            var removedPendingResponseLines = responseLines
                .Where((responseLine) => !(responseLine.IsPendingResponse)).ToArray();

            var firstResponseFrame = responseFrames.FirstOrDefault(" ");
            var canId = firstResponseFrame.Split(" ")[0];
            var response = new OBD2Response(canId, removedPendingResponseLines);

            // 通信順の制御（シリアルキュー、後実装)
            return response;
        }
    }
}