namespace AuctionVehicleDataCollectionApp.OBD2.Entity.Response
{
    /// <summary>
    /// ネガティブレスポンスの種類
    /// </summary>
    public enum NegativeResponseCode : int
    {
        GeneralReject = 0x10,

        ServiceNotSupported = 0x11,

        SubFunctionNotSupported = 0x12,

        InvalidFormat = 0x13,

        Busy_RepeatRequest = 0x21,

        ConditionsNotCorrectOrRequestSequenseError = 0x22,

        RequestSequenceError = 0x24,

        RequestOutOfRange = 0x31,

        SecurityAccessDenied = 0x33,

        InvalidKey = 0x35,

        ExceedNumberOfAttempts = 0x36,

        RequiredTimeDelayNotExpired = 0x37,

        ResponsePending = 0x78,

        NonvolatileMemoryReadOrWriteError = 0x81,

        Others = 0x00
    }
}