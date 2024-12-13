namespace Fleet.Core.Extensions;

public static class DateTimeExtensions
{
    public static DateTime Truncate(this DateTime dateTime, TimeSpan timeSpan)
    {
        if (timeSpan == TimeSpan.Zero)
        {
            return dateTime;
        }

        if (dateTime == DateTime.MinValue || dateTime == DateTime.MaxValue)
        {
            return dateTime;
        }

        return dateTime.AddTicks(-(dateTime.Ticks % timeSpan.Ticks));
    }

    public static int ConvertToUnixTimestamp(this DateTime date)
    {
        var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        var diff = date.ToUniversalTime() - origin;
        return (int)Math.Floor(diff.TotalSeconds);
    }
}