namespace Utilities.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// The system display timezone (UTC+3 — Arabia Standard Time / Cairo).
        /// All UTC values stored in the database are converted to this zone before display.
        /// </summary>
        private static readonly TimeZoneInfo DisplayTimeZone =
            TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");

        // ──────────────────────────────────────────────────────────────
        // Timezone conversion helpers
        // ──────────────────────────────────────────────────────────────

        /// <summary>
        /// Converts a UTC DateTime to the local display timezone (UTC+3).
        /// If the DateTime kind is Unspecified it is treated as UTC.
        /// </summary>
        public static DateTime ToLocalDisplayTime(this DateTime utcDateTime)
        {
            var utc = utcDateTime.Kind == DateTimeKind.Local
                ? utcDateTime.ToUniversalTime()
                : DateTime.SpecifyKind(utcDateTime, DateTimeKind.Utc);
            return TimeZoneInfo.ConvertTimeFromUtc(utc, DisplayTimeZone);
        }

        /// <summary>
        /// Converts a UTC TimeSpan (time-of-day) to a local TimeSpan for the display timezone.
        /// </summary>
        private static TimeSpan ToLocalTimeSpan(this TimeSpan utcTimeOfDay)
        {
            var utcToday = DateTime.UtcNow.Date.Add(utcTimeOfDay);
            var local = TimeZoneInfo.ConvertTimeFromUtc(utcToday, DisplayTimeZone);
            return local.TimeOfDay;
        }

        // ──────────────────────────────────────────────────────────────
        // Date formatting — converts UTC → local before formatting
        // ──────────────────────────────────────────────────────────────

        public static string ToShortDateTimeString(this DateTime value)
        {
            return value.ToLocalDisplayTime().ToString("dd/MM/yyyy hh:mm tt");
        }

        public static string ToDateString(this DateTime value)
        {
            return value.ToLocalDisplayTime().ToString("dd/MM/yyyy");
        }

        public static string FullDateString(this DateTime value)
        {
            return value.ToLocalDisplayTime().ToString("dd MMMM yyyy");
        }

        public static string ToLongDateString(this DateTime value)
        {
            return value.ToLocalDisplayTime().ToString("dddd, dd MMMM yyyy");
        }

        public static string ToLongDateTimeString(this DateTime value)
        {
            return value.ToLocalDisplayTime().ToString("dddd, dd MMMM yyyy hh:mm tt");
        }

        public static string ToInputDateString(this DateTime value)
        {
            return value.ToLocalDisplayTime().ToString("yyyy-MM-dd");
        }

        // ──────────────────────────────────────────────────────────────
        // Time formatting — converts UTC TimeSpan → local before formatting
        // ──────────────────────────────────────────────────────────────

        /// <summary>Formats a UTC TimeSpan as a 12-hour local time string (e.g. "10:00 AM").</summary>
        public static string ToTimeString(this TimeSpan value)
        {
            var local = value.ToLocalTimeSpan();
            return DateTime.Today.Add(local).ToString("hh:mm tt");
        }

        public static string To24HourTimeString(this TimeSpan value)
        {
            var local = value.ToLocalTimeSpan();
            return local.ToString(@"hh\:mm\:ss");
        }

        /// <summary>Formats a UTC TimeSpan as a 12-hour local time string (e.g. "10:00 AM").</summary>
        public static string To12HourTimeString(this TimeSpan value)
        {
            var local = value.ToLocalTimeSpan();
            return DateTime.Today.Add(local).ToString("hh:mm tt");
        }
    }
}
