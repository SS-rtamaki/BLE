namespace AuctionVehicleDataCollectionApp.OBD2.Entity.Response
{
    public enum ErrorType
    {
        /// <summary>
        /// エラーなし
        /// </summary>
        NoErrors,
        
        /// <summary>
        /// データなし
        /// </summary>
        NoData,
        
        /// <summary>
        /// CANエラー
        /// </summary>
        CanError
    }


}
