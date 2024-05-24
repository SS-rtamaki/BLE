namespace AuctionVehicleDataCollectionApp.OBD2.Entity.Response
{
    /// <summary>
    /// 受信メッセージの種類
    /// </summary>
    public enum MessageType
    {
        SingleFrame,
        FirstFrame,
        FlowControl,
        ConsectiveFrame
    }
}