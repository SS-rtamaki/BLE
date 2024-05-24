namespace AuctionVehicleDataCollectionApp.Common
{
    /// <summary>
    /// String 拡張メソッドクラス
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// 文字列をN文字ずつ区切ったstring配列にする
        /// </summary>
        /// <param name="rangeNumber">区切る文字数</param>
        /// <param name="characters">対象文字列</param>
        /// <returns></returns>
        public static string[] SeparateCharacters(this string @this, int rangeNumber)
        {
            IEnumerable<string> SeparateCharacters_inner(string str, int maxLength)
            {
                for (int index = 0; index < str.Length; index += maxLength)
                {
                    yield return str.Substring(index, Math.Min(maxLength, str.Length - index));
                }
            }

            return SeparateCharacters_inner(@this, rangeNumber).ToArray();
        }
    }

    /// <summary>
    /// DateTime 拡張メソッドクラス
    /// </summary>
    public static class DateTimeExtensions
    {
        public static bool
        IsBetween(this DateTime value, DateTime from, DateTime to)
        {
            return from <= value && to >= value;
        }
    }

}