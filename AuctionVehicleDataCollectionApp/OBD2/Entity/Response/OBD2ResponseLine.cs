using AuctionVehicleDataCollectionApp.Common;

namespace AuctionVehicleDataCollectionApp.OBD2.Entity.Response
{
    /// <summary>
    /// 8バイト分の応答メッセージクラス
    /// </summary>
    public class OBD2ResponseLine
    {
        /// <summary>
        /// 8バイトのメッセージ
        /// </summary>
        public string Message { get; init; }

        /// <summary>
        /// メッセージの種類
        /// </summary>
        public MessageType MessageType
        {
            get
            {
                // TODO: 複数フレーム対応
                var firstString = Message.Substring(0, 1);
                switch (firstString)
                {

                    case "0":
                        return MessageType.SingleFrame;

                    case "1":
                        return MessageType.FirstFrame;

                    case "2":
                        return MessageType.ConsectiveFrame;

                    default:
                        throw new Exception($"Abnomal frame type. :{firstString}");
                }
            }
        }

        /// <summary>
        /// 保留応答か
        /// </summary>
        public bool IsPendingResponse
        {
            get
            {
                return NegativeResponseCode == Response.NegativeResponseCode.ResponsePending;
            }
        }

        /// <summary>
        /// ネガティブレスポンスか
        /// </summary>
        public bool IsNegativeResponse
        {
            get
            {
                var sid = Message.Substring(2, 2);
                if (sid == "7F")
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// ネガティブレスポンスコード
        /// </summary>
        public NegativeResponseCode? NegativeResponseCode
        {
            get
            {
                if (!IsNegativeResponse)
                {
                    return null;
                }
                var nrc = Message.Substring(6, 2);
                switch (nrc)
                {
                    case "10":
                        return Response.NegativeResponseCode.GeneralReject;

                    case "11":
                        return Response.NegativeResponseCode.ServiceNotSupported;

                    case "12":
                        return Response.NegativeResponseCode.SubFunctionNotSupported;

                    case "13":
                        return Response.NegativeResponseCode.InvalidFormat;

                    case "21":
                        return Response.NegativeResponseCode.Busy_RepeatRequest;

                    case "22":
                        return Response.NegativeResponseCode.ConditionsNotCorrectOrRequestSequenseError;

                    case "24":
                        return Response.NegativeResponseCode.RequestSequenceError;

                    case "31":
                        return Response.NegativeResponseCode.RequestOutOfRange;

                    case "33":
                        return Response.NegativeResponseCode.SecurityAccessDenied;

                    case "35":
                        return Response.NegativeResponseCode.InvalidKey;

                    case "36":
                        return Response.NegativeResponseCode.ExceedNumberOfAttempts;

                    case "37":
                        return Response.NegativeResponseCode.RequiredTimeDelayNotExpired;

                    case "78":
                        return Response.NegativeResponseCode.ResponsePending;

                    case "81":
                        return Response.NegativeResponseCode.NonvolatileMemoryReadOrWriteError;

                    default:
                        return Response.NegativeResponseCode.Others;

                }
            }
        }

        /// <summary>
        /// メッセージのバイト配列
        /// </summary>
        public byte[] MessageBytes
        {
            get
            {
                var separatedMessage = Message.SeparateCharacters(2);
                return separatedMessage.Select(match => byte.Parse(match, System.Globalization.NumberStyles.HexNumber)).ToArray();
            }
        }

        /// <summary>
        /// 受信データ1フレーム分のオブジェクト
        /// </summary>
        /// <param name="message">受信メッセージ</param>
        public OBD2ResponseLine(string message)
        {
            Message = message;
        }
    }
}