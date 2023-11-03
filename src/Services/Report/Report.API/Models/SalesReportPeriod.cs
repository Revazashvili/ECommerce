using System.Text.Json.Serialization;

namespace Report.API.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SalesReportPeriod
{
    Day,
    Week,
    Month
}

public static class SalesReportPeriodExtensions
{
    public static DateTime CalculateFromDate(this SalesReportPeriod period)
    {
        var daysToSubtract = period switch
        {
            SalesReportPeriod.Day => -1,
            SalesReportPeriod.Week => -7,
            SalesReportPeriod.Month => -30,
            _ => throw new ArgumentOutOfRangeException(nameof(period), period, null)
        };

        return DateTime.Now.AddDays(daysToSubtract);
    }
}