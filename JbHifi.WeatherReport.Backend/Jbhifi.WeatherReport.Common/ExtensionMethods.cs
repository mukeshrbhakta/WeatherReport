namespace JbHifi.WeatherReport.Common;

public static class ExtensionMethods
{
    /// <summary>
    /// Convert list to json 
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public static string ToJsonArray(this IEnumerable<string> list)
    {
        var join = string.Join(",", list.Select(x => x).ToArray());
        return $"[{join}]";
    }
}