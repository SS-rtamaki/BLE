using AuctionVehicleDataCollectionApp.BLEConnection.Exceptions;
using AuctionVehicleDataCollectionApp.Common;
using AuctionVehicleDataCollectionApp.OBD2.Entity.Request;
using AuctionVehicleDataCollectionApp.OBD2.Entity.Response;

namespace AuctionVehicleDataCollectionApp.OBD2.ELM327
{
    /// <summary>
    /// ELM327メッセージプロトコルクラス
    /// </summary>
    public class ELM327MessagingProtocol : IOBD2ConnectionInterface
    {
        private IELM327BLEInterface connection;
        private bool disposed;
        private static MyLogger logger = new(typeof(ELM327MessagingProtocol));

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="connection">BLEコネクション</param>
        public ELM327MessagingProtocol(IELM327BLEInterface connection)
        {
            this.connection = connection;
        }

        ~ELM327MessagingProtocol()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (disposed) return;
            connection.Dispose();
            disposed = true;
        }

        /// <summary>
        /// BLE初期化通信を実施。
        /// </summary>
        /// <returns>true：成功、false：失敗</returns>
        public async Task<bool> InitializeAsync(CancellationToken token = default)
        {
            logger.Debug($"Start InitializeAsync");
            try
            {
                var resultATZ = await SendMessageAsync(ATCommand.ATZ, token);
                if (!resultATZ.Success)
                {
                    logger.Trace($"ATZ failed");
                    return false;
                }
                logger.Trace($"ATZ success");
                var resultATE0 = await SendMessageAsync(ATCommand.ATE0, token);
                if (!resultATE0.Success)
                {
                    logger.Trace($"ATE0 failed");
                    return false;
                }
                logger.Trace($"ATE0 success");
                var resultATSP6 = await SendMessageAsync(ATCommand.ATSP6, token);
                if (!resultATSP6.Success)
                {
                    logger.Trace($"ATSP6 failed");
                    return false;
                }
                logger.Trace($"ATSP6 success");
                var resultATH1 = await SendMessageAsync(ATCommand.ATH1, token);
                if (!resultATH1.Success)
                {
                    logger.Trace($"ATH1 failed");
                    return false;
                }
                logger.Trace($"ATH1 success");
                var resultATR1 = await SendMessageAsync(ATCommand.ATR1, token);
                if (!resultATR1.Success)
                {
                    logger.Trace($"ATR1 failed");
                    return false;
                }
                logger.Trace($"ATR1 success");
                var resultATCAF0 = await SendMessageAsync(ATCommand.ATCAF0, token);
                if (!resultATCAF0.Success)
                {
                    logger.Trace($"ATCAF0 failed");
                    return false;
                }
                logger.Trace($"ATCAF0 success");
                var resultATCFC0 = await SendMessageAsync(ATCommand.ATCFC0, token);
                if (!resultATCFC0.Success)
                {
                    logger.Trace($"ATCFC0 failed");
                    return false;
                }
                logger.Trace($"ATCFC0 success");

                return true;
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                logger.Warn($"InitializeAsync failed");
                logger.Warn($"{ex.Message} {ex.StackTrace}");
                return false;
            }
            finally
            {
                logger.Debug($"End   InitializeAsync");
            }
        }
        /// <summary>
        /// CAN通信を実施。
        /// </summary>
        /// <param name="request">CAN要求(CANID+Message)</param>
        /// <returns>CAN応答</returns>
        public async Task<OBD2Response> SendMessageAsync(OBD2Request request, CancellationToken token = default)
        {
            // TODO:通信順の制御（シリアルキュー、後実装)

            logger.Debug($"Start SendMessageAsync canId: {request.CanId} message: {request.Message}");
            try
            {
                var resultATSH = await SendMessageAsync(ATCommand.ATSH(request.CanId), token);
                if (!resultATSH.Success)
                {
                    return OBD2Response.CreateOBD2ResponseEmpty("", ErrorType.NoErrors);
                }

                var elmResponse = await connection.SendMessageAsync(request.Message + " 1", token);

                // OBD2ResponseFragmentクラスに変換して、データの断片として保持
                var obd2ResponseFragment = elmResponse.ParseAsOBD2ResponseFragment();

                // OBD2ResponseBufferにデータの断片をAddしていく
                var obd2ResponeBuffer = new OBD2ResponseBuffer();

                obd2ResponeBuffer.Append(obd2ResponseFragment);

                // データが異常の場合は、そこまでのデータを返す
                if (!obd2ResponseFragment.Successs)
                {
                    return obd2ResponeBuffer.GetOBD2Response();
                }

                while (!obd2ResponeBuffer.HasEnoughLine)
                {
                    // マルチフレームの場合の処理

                    var nextRequest = CreateFlowControllMessage(request.CanId);

                    var nextElmResponse = await connection.SendMessageAsync(nextRequest.Message, token);

                    var nextObd2ResponseFragment = nextElmResponse.ParseAsOBD2ResponseFragment();
                    obd2ResponeBuffer.Append(nextObd2ResponseFragment);

                    if (!nextObd2ResponseFragment.Successs)
                    {
                        break;
                    }
                }

                return obd2ResponeBuffer.GetOBD2Response();
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                // connection.IsConnectedがfalseなら専用のExceptionをthrow
                if (!connection.IsConnected)
                {
                    logger.Warn($"BLE disconnected");
                    logger.Warn($"{ex.Message} {ex.StackTrace}");
                    throw new BLEDisconnectedException("BLE connection is disconnected");
                }

                logger.Warn($"SendMessageAsync failed");
                logger.Warn($"{ex.Message} {ex.StackTrace}");
                return OBD2Response.CreateOBD2ResponseEmpty("", ErrorType.NoErrors);
            }
            finally
            {
                logger.Debug($"End   SendMessageAsync canId: {request.CanId} message: {request.Message}");
            }

        }
        /// <summary>
        /// 要求(ATコマンド)を文字列で送信。
        /// </summary>
        /// <param name="command">要求(ATコマンド)</param>
        /// <returns>応答値</returns>
        public async Task<ATCommandResult> SendMessageAsync(ATCommand command, CancellationToken token = default)
        {
            logger.Debug($"Start SendMessageAsync ATCommand: {command}");
            try
            {
                var response = await connection.SendMessageAsync(command.CommandString, token);
                return response.ParseAsAtCommandResult();
            }
            finally
            {
                logger.Debug($"End   SendMessageAsync ATCommand: {command}");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="canId">CANID</param>
        /// <param name="request">リクエストメッセージ</param>
        /// <returns>OBD2Requestインスタンス</returns>
        private OBD2Request CreateFlowControllMessage(string canId)
        {
            return new OBD2Request(canId, "3010000000000000");
        }

    }
}