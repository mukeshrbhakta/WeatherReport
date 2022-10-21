namespace JbHifi.WeatherReport.Common;

public static class ExtensionMethods
{
    public static string ToJsonArray(this IEnumerable<string> list)
    {
        var join = string.Join(",", list.Select(x => x).ToArray());
        return $"[{join}]";
    }
}