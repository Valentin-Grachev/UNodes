using System;
using System.Globalization;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;

namespace UNodes.Extensions
{
    public static class SavesExtensions
    {
        public static string ToSaveString(this bool value) 
            => value ? "1" : "0";

        public static bool SaveStringToBool(this string data)
            => data == "0" ? false : true;

        public static string ToSaveString(this DateTime value)
            => value.Ticks.ToString(CultureInfo.InvariantCulture);

        public static DateTime SaveStringToDateTime(this string data)
            => new DateTime(long.Parse(data, CultureInfo.InvariantCulture));




    }
}
