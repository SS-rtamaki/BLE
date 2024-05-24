using AuctionVehicleDataCollectionApp.OBD2.Exceptions;

namespace AuctionVehicleDataCollectionApp.OBD2.Entity.Request
{
    /// <summary>
    /// SID19Optionクラス
    /// </summary>
    public abstract class SID19Option
    {
        /// <summary>
        /// Option03
        /// </summary>
        public static SID19Option Opt03
        {
            get
            {
                return new Opt03();
            }
        }

        /// <summary>
        /// 各Optionに対応する要求文字列(19XX...)
        /// </summary>
        public abstract string OptionString { get; }

        /// <summary>
        /// 各Optionに対応するデータ長
        /// </summary>
        public abstract string OptionDataLength { get; }

        /// <summary>
        /// Option02
        /// </summary>
        /// <param name="mask">マスク値</param>
        /// <returns></returns>
        public static SID19Option Opt02(string mask)
        {
            return new Opt02(mask);
        }

        /// <summary>
        /// Option04
        /// </summary>
        /// <param name="dtc">DTC</param>
        /// <param name="id">Id</param>
        /// <returns></returns>
        public static SID19Option Opt04(string dtc, string id)
        {
            return new Opt04(dtc, id);
        }
    }

    /// <summary>
    /// Option02クラス
    /// </summary>
    public class Opt02 : SID19Option
    {
        /// <summary>
        /// マスク値
        /// </summary>
        public string Mask { get; init; }

        /// <summary>
        /// 要求文字列
        /// </summary>
        public override string OptionString => $"1902{Mask}";

        /// <summary>
        /// データ長
        /// </summary>
        public override string OptionDataLength => "03";

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="mask">マスク値</param>
        /// <exception cref="OBD2Exception">OBD2Exception</exception>
        public Opt02(string mask)
        {
            // maskは2文字(1バイト)
            if (mask?.Length != 2)
            {
                throw new OBD2Exception("Opt02MaskIsInvalid");
            }

            Mask = mask;
        }
    }

    /// <summary>
    /// Option03クラス
    /// </summary>
    public class Opt03 : SID19Option
    {
        /// <summary>
        /// 要求文字列
        /// </summary>
        public override string OptionString => "1903";

        /// <summary>
        /// データ長
        /// </summary>
        public override string OptionDataLength => "02";
    }


    /// <summary>
    /// Option04クラス
    /// </summary>
    public class Opt04 : SID19Option
    {
        /// <summary>
        /// DTC
        /// </summary>
        public string Dtc { get; init; }

        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; init; }

        /// <summary>
        /// 要求文字列
        /// </summary>
        public override string OptionString => $"1904{Dtc}{Id}";

        /// <summary>
        /// データ長
        /// </summary>
        public override string OptionDataLength => "06";

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dtc">DTC</param>
        /// <param name="id">Id</param>
        /// <exception cref="OBD2Exception">OBD2Exception</exception>
        public Opt04(string dtc, string id)
        {
            // dtcは6文字(3バイト)
            if (dtc?.Length != 6)
            {
                throw new OBD2Exception("Opt04DTCIsInvalid");
            }
            Dtc = dtc;

            // idは2文字(1バイト)
            if (id?.Length != 2)
            {
                throw new OBD2Exception("Opt04IdIsInvalid");
            }
            Id = id;
        }
    }
}
