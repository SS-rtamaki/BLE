using NLog;

namespace AuctionVehicleDataCollectionApp.Common
{
    /// <summary>
    /// NLogラッパークラス
    /// </summary>
    public class MyLogger
    {

        private Logger logger;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="type">呼び出し元のクラスの型</param>
        public MyLogger(Type type)
        {
            logger = NLog.LogManager.GetLogger(type.FullName);
        }

        /// <summary>
        /// トレース用ログ
        /// </summary>
        /// <param name="message">ログメッセージ</param>
        public void Trace(string message)
        {
            logger.Trace(message);
        }

        /// <summary>
        /// インフォ用ログ
        /// </summary>
        /// <param name="message">ログメッセージ</param>
        public void Info(string message)
        {
            logger.Info(message);
        }

        /// <summary>
        /// デバッグ用ログ
        /// </summary>
        /// <param name="message">ログメッセージ</param>
        public void Debug(string message)
        {
            logger.Debug(message);
        }

        /// <summary>
        /// 警告用ログ
        /// </summary>
        /// <param name="message">ログメッセージ</param>
        public void Warn(string message)
        {
            logger.Warn(message);
        }

        /// <summary>
        /// エラー用ログ
        /// </summary>
        /// <param name="message">ログメッセージ</param>
        public void Error(string message)
        {
            logger.Error(message);
        }
    }
}
