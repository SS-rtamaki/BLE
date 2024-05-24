namespace AuctionVehicleDataCollectionApp.OBD2.Entity.Response
{
    public class DTCStatus
    {
        private byte[] dtc { get; init; }

        /// <summary>
        /// ステータスレコード
        /// </summary>
        public string Status { get; init; }

        /// <summary>
        /// 接頭辞変換済みのDTC
        /// </summary>
        public string DispCode
        {
            get
            {
                // 上位2ビットを取得
                var dtcCategoryNumber = (dtc[0] & 0xC0);
                var dtcCategory = "";
                switch (dtcCategoryNumber)
                {
                    case 0x00:
                        dtcCategory = "P";
                        break;
                    case 0x40:
                        dtcCategory = "C";
                        break;
                    case 0x80:
                        dtcCategory = "B";
                        break;
                    case 0xC0:
                        dtcCategory = "U";
                        break;
                }

                // 接頭語にあたる上位2ビットを除いた1バイト目のDTC
                var firstByteDTC = (dtc[0] & 0x3F).ToString("X2");

                return dtcCategory + firstByteDTC + dtc[1].ToString("X2") + dtc[2].ToString("X2");
            }

        }

        /// <summary>
        /// 接頭辞変換なしのDTC
        /// </summary>
        public string RawCode
        {
            get
            {
                return dtc[0].ToString("X2") + dtc[1].ToString("X2") + dtc[2].ToString("X2");
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dtc">DTC</param>
        /// <param name="status">ステータスレコード</param>
        public DTCStatus(byte[] dtc, string status)
        {
            this.dtc = dtc;
            Status = status;
        }
    }
}