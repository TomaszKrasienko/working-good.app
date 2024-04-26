namespace wg.tests.shared.Helpers;

internal static class ShortDateTimeProvider
{
    internal static DateTime Get(DateTime dateTime)
        => new DateTime(dateTime.Year, dateTime.Month, dateTime.Day,
            dateTime.Hour, dateTime.Minute, 0);
}