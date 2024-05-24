namespace AuctionVehicleDataCollectionApp.OBD2.ELM327
{
    /// <summary>
    /// ATCommandクラス
    /// </summary>
    public abstract class ATCommand
    {
        /// <summary>
        /// reset all
        /// </summary>
        static public ATCommand ATZ
        {
            get
            {
                return new ATZ();
            }
        }
        /// <summary>
        /// echo off
        /// </summary>
        static public ATCommand ATE0
        {
            get
            {
                return new ATE0();
            }
        }
        /// <summary>
        /// set protocol to CAN
        /// </summary>
        static public ATCommand ATSP6
        {
            get
            {
                return new ATSP6();
            }
        }
        /// <summary>
        /// header on
        /// </summary>
        static public ATCommand ATH1
        {
            get
            {
                return new ATH1();
            }
        }
        /// <summary>
        /// response on
        /// </summary>
        static public ATCommand ATR1
        {
            get
            {
                return new ATR1();
            }
        }
        /// <summary>
        /// can auto format off
        /// </summary>
        static public ATCommand ATCAF0
        {
            get
            {
                return new ATCAF0();
            }
        }
        /// <summary>
        /// send flow control off
        /// </summary>
        static public ATCommand ATCFC0
        {
            get
            {
                return new ATCFC0();
            }
        }
        /// <summary>
        /// コマンド文字列
        /// </summary>
        public abstract string CommandString { get; }
        /// <summary>
        /// set header
        /// </summary>
        /// <param name="canId">CANID</param>
        /// <returns></returns>
        public static ATCommand ATSH(string canId)
        {
            return new ATSH(canId);
        }

    }
    /// <summary>
    /// ATZクラス
    /// </summary>
    public class ATZ : ATCommand
    {
        public override string CommandString => "ATZ";
    }
    /// <summary>
    /// ATE0クラス
    /// </summary>
    public class ATE0 : ATCommand
    {
        public override string CommandString => "ATE0";
    }
    /// <summary>
    /// ATSP6クラス
    /// </summary>
    public class ATSP6 : ATCommand
    {
        public override string CommandString => "ATSP6";
    }
    /// <summary>
    /// ATH1クラス
    /// </summary>
    public class ATH1 : ATCommand
    {
        public override string CommandString => "ATH1";
    }
    /// <summary>
    /// ATR1クラス
    /// </summary>
    public class ATR1 : ATCommand
    {
        public override string CommandString => "ATR1";
    }
    /// <summary>
    /// ATCAF0クラス
    /// </summary>
    public class ATCAF0 : ATCommand
    {
        public override string CommandString => "ATCAF0";
    }
    /// <summary>
    /// ATSHクラス
    /// </summary>
    public class ATSH : ATCommand
    {
        /// <summary>
        /// 指定CanID
        /// </summary>
        public string CanId { get; init; }

        public override string CommandString => $"ATSH {CanId}";

        /// <summary>
        /// ATSHクラス
        /// </summary>
        /// <param name="canId">CANID</param>
        public ATSH(string canId)
        {
            CanId = canId;
        }
    }
    /// <summary>
    /// ATCFC0クラス
    /// </summary>
    public class ATCFC0 : ATCommand
    {
        public override string CommandString => "ATCFC0";
    }
}