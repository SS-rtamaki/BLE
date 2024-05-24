using AuctionVehicleDataCollectionApp.Common;
using AuctionVehicleDataCollectionApp.OBD2.Exceptions;

namespace AuctionVehicleDataCollectionApp.OBD2.Entity.Response
{
    /// <summary>
    /// CAN通信の応答値クラス
    /// </summary>
    public class OBD2Response
    {
        /// <summary>
        /// ペイロード(SID以降のデータのみ)
        /// </summary>
        public byte[] Payload
        {
            get
            {
                if (ResponseDatas.Count == 0)
                {
                    return Array.Empty<byte>();
                }

                // 各行の1バイト目を除く
                var removedFirstByteMessageBytesTwoDimArray = ResponseDatas.Select(responseData => responseData.MessageBytes[1..]).ToArray();

                // １フレームごとのbyte配列の配列結合
                var removedFirstByteMessageBytesArray = removedFirstByteMessageBytesTwoDimArray.SelectMany(array => array).ToArray();

                byte[]? removedFirstNonPayloadDatasArray = null;

                if (ResponseDatas[0].MessageType == MessageType.FirstFrame)
                {
                    // データ長削除
                    removedFirstNonPayloadDatasArray = removedFirstByteMessageBytesArray[1..];
                }
                else
                {
                    removedFirstNonPayloadDatasArray = removedFirstByteMessageBytesArray;
                }

                // データ部後ろ0埋め部分削除
                var dataArray = removedFirstNonPayloadDatasArray.SkipLast(removedFirstNonPayloadDatasArray.Length - DataLength);

                return dataArray.ToArray();

            }
        }

        /// <summary>
        /// データ長
        /// </summary>
        public int DataLength
        {
            get
            {
                if (ResponseDatas.Count == 0)
                {
                    return 0;
                }

                switch (ResponseDatas[0].MessageType)
                {
                    case MessageType.SingleFrame:
                        {
                            var messageBytes = ResponseDatas[0].MessageBytes;

                            // データ長取得
                            int dataLength = messageBytes[0];

                            // データ長以降のデータ取得
                            return dataLength;
                        }


                    case MessageType.FirstFrame:
                        {
                            //  データ長取得
                            var messageBytes = ResponseDatas[0].MessageBytes;

                            // 1X XX SID DID 
                            int upper = messageBytes[0] & 0x0F;

                            int lower = messageBytes[1];

                            //  データ長
                            int dataLength = (upper << 8) + lower;

                            return dataLength;
                        }


                    default:
                        return 0;
                }
            }
        }

        /// <summary>
        /// データ長だけデータが入っているか
        /// </summary>
        public bool IsValid
        {
            get
            {
                if (ResponseDatas.Count == 0)
                {
                    return false;
                }
                switch (ResponseDatas[0].MessageType)
                {
                    case MessageType.SingleFrame:
                        {
                            var messageBytes = ResponseDatas[0].MessageBytes;

                            // データ長取得
                            int dataLength = DataLength;

                            // データ長以降のデータ取得
                            return (messageBytes.Length - 1) >= dataLength;
                        }

                    case MessageType.FirstFrame:
                        {
                            //  データ長
                            int dataLength = DataLength;

                            // index0: 3バイト目以降 messageBytes.Length - 2
                            // index1以降: 2バイト目以降 messageBytes.Length - 1
                            int actualDataLength = ResponseDatas.Select(data => data.MessageBytes.Length - 1).Sum() - 1;

                            return (actualDataLength >= dataLength);
                        }

                    default:
                        {
                            return false;
                        }
                }
            }
        }

        /// <summary>
        /// ネガティブレスポンスか
        /// </summary>
        public bool IsNegative
        {
            get
            {
                if (ResponseDatas.Count == 0)
                {
                    return false;
                }
                return ResponseDatas[0].IsNegativeResponse;
            }
        }

        /// <summary>
        /// CANID
        /// </summary>
        public string CanId { get; init; }

        /// <summary>
        /// 拡張CANアドレス
        /// </summary>
        public string? Nta { get; init; }

        /// <summary>
        /// エラー情報
        /// </summary>
        public ErrorType ErrorType { get; init; }

        /// <summary>
        /// 8バイトごとの応答メッセージ配列
        /// </summary>
        public IReadOnlyList<OBD2ResponseLine> ResponseDatas { get; init; }

        /// <summary>
        /// 受信データオブジェクト
        /// </summary>
        /// <param name="canId">CANID</param>
        /// <param name="data">受信データ</param>
        public OBD2Response(string canId, string data) : this(canId, null, ErrorType.NoErrors, new[] { new OBD2ResponseLine(data) }) { }

        /// <summary>
        /// 受信データオブジェクト
        /// </summary>
        /// <param name="canId">CANID</param>
        /// <param name="nta">NTA</param>
        /// <param name="data">受信データ</param>
        public OBD2Response(string canId, string? nta, string data) : this(canId, nta, ErrorType.NoErrors, new[] { new OBD2ResponseLine(data) }) { }

        /// <summary>
        /// 受信データオブジェクト
        /// </summary>
        /// <param name="canId">CANID</param>
        /// <param name="datas">受信データ</param>
        public OBD2Response(string canId, IEnumerable<string> datas) : this(canId, null, ErrorType.NoErrors, datas.Select(data => new OBD2ResponseLine(data))) { }

        /// <summary>
        /// 受信データオブジェクト
        /// </summary>
        /// <param name="canId">CANID</param>
        /// <param name="nta">NTA</param>
        /// <param name="datas">受信データ</param>
        public OBD2Response(string canId, string? nta, IEnumerable<string> datas) : this(canId, nta, ErrorType.NoErrors, datas.Select(data => new OBD2ResponseLine(data))) { }

        /// <summary>
        /// 受信データオブジェクト
        /// </summary>
        /// <param name="canId">CANID</param>
        /// <param name="datas">受信データ</param>
        public OBD2Response(string canId, IEnumerable<OBD2ResponseLine> datas) : this(canId, null, ErrorType.NoErrors, datas) { }

        /// <summary>
        /// 受信データオブジェクト
        /// </summary>
        /// <param name="canId">CANID</param>
        /// <param name="nta">NTA</param>
        /// <param name="datas">受信データ</param>
        public OBD2Response(string canId, string? nta, IEnumerable<OBD2ResponseLine> datas) : this(canId, nta, ErrorType.NoErrors, datas) { }

        /// <summary>
        /// 受信データオブジェクト
        /// </summary>
        /// <param name="canId">CANID</param>
        /// <param name="errorType">エラータイプ</param>
        /// <param name="datas">受信データ</param>
        public OBD2Response(string canId, ErrorType errorType, IEnumerable<OBD2ResponseLine> datas) : this(canId, null, errorType, datas) { }

        /// <summary>
        /// 受信データオブジェクト
        /// </summary>
        /// <param name="canId">CANID</param>
        /// <param name="nta">NTA</param>
        /// <param name="errorrType">エラータイプ</param>
        /// <param name="datas">受信データ</param>
        public OBD2Response(string canId, string? nta, ErrorType errorrType, IEnumerable<OBD2ResponseLine> datas)
        {
            CanId = canId;
            Nta = nta;
            ErrorType = errorrType;
            ResponseDatas = datas.ToArray();
        }

        /// <summary>
        /// SID19Type2応答値に変換
        /// </summary>
        /// <returns>DTCStatusインスタンス配列</returns>
        public DTCStatus[] ParseAsSID19Type2()
        {
            // 前提条件確認
            if (!IsValid)
            {
                throw new OBD2Exception("Invalid data or message type");
            }

            if (IsNegative)
            {
                throw new OBD2Exception("Negative response");
            }

            if (ResponseDatas[0].MessageType == MessageType.SingleFrame)
            {
                if (!(ResponseDatas[0].Message.Substring(2, 4) == "5902"))
                {
                    throw new OBD2Exception("Invalid data");
                }
            }

            if (ResponseDatas[0].MessageType == MessageType.FirstFrame)
            {
                if (!(ResponseDatas[0].Message.Substring(4, 4) == "5902"))
                {
                    throw new OBD2Exception("Invalid data");
                }
            }

            try
            {
                // データ部前3バイト分(SID、Option、ECU依存ID)削除
                var removedNonDatasArray = Payload[3..];

                var stringArray = removedNonDatasArray.Select(data => data.ToString("x2")).ToArray();

                var dataStrings = string.Join("", stringArray);

                // DTCごとに配列にする
                var dtcStatusList = dataStrings.SeparateCharacters(8);

                DTCStatus[] dtcStatuses = dtcStatusList.Select<string, DTCStatus>(data =>
                {
                    var firstByteDtc = Convert.ToByte(data.Substring(0, 2), 16);
                    var middleByteDtc = Convert.ToByte(data.Substring(2, 2), 16);
                    var LastByteDtc = Convert.ToByte(data.Substring(4, 2), 16);
                    var ssrId = data.Substring(6, 2);
                    return new DTCStatus(new byte[] { firstByteDtc, middleByteDtc, LastByteDtc }, ssrId);
                }).ToArray();

                return dtcStatuses;
            }
            catch (Exception ex)
            {
                throw new OBD2Exception("Failed to get dtcs", ex);
            }
        }

        /// <summary>
        /// SID19Type3応答値に変換
        /// </summary>
        /// <returns>SSRインスタンスの配列</returns>
        public DTC[] ParseAsSID19Type3()
        {
            // 前提条件確認
            if (!IsValid)
            {
                throw new OBD2Exception("Invalid data or message type");
            }

            if (IsNegative)
            {
                throw new OBD2Exception("Negative response");
            }

            if (ResponseDatas[0].MessageType == MessageType.SingleFrame)
            {
                if (!(ResponseDatas[0].Message.Substring(2, 4) == "5903"))
                {
                    throw new OBD2Exception("Invalid data");
                }
            }

            if (ResponseDatas[0].MessageType == MessageType.FirstFrame)
            {
                if (!(ResponseDatas[0].Message.Substring(4, 4) == "5903"))
                {
                    throw new OBD2Exception("Invalid data");
                }
            }

            try
            {
                // データ部前2バイト分(SID、Option)削除
                var removedNonDatasArray = Payload[2..];


                var stringArray = removedNonDatasArray.Select(data => data.ToString("x2")).ToArray();


                var dataStrings = string.Join("", stringArray);



                // DTCごとに配列にする
                var dtcStatusList = dataStrings.SeparateCharacters(8);

                DTC[] dtcs = dtcStatusList.Select<string, DTC>(data =>
                {
                    var firstByteDtc = Convert.ToByte(data.Substring(0, 2), 16);
                    var middleByteDtc = Convert.ToByte(data.Substring(2, 2), 16);
                    var LastByteDtc = Convert.ToByte(data.Substring(4, 2), 16);
                    var ssrId = data.Substring(6, 2);
                    return new DTC(new byte[] { firstByteDtc, middleByteDtc, LastByteDtc }, ssrId);
                }).ToArray();

                return dtcs;
            }
            catch (Exception ex)
            {
                throw new OBD2Exception("Failed to get ssrs", ex);
            }
        }

        /// <summary>
        /// 空のOBD2Rsponseインスタンス生成
        /// </summary>
        /// <param name="canId">CANID</param>
        /// <param name="errorType">エラータイプ</param>
        /// <returns></returns>
        public static OBD2Response CreateOBD2ResponseEmpty(string canId, ErrorType errorType)
        {
            return new OBD2Response(canId, errorType, Array.Empty<OBD2ResponseLine>());
        }
    }
}