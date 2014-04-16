namespace YoutrackBoard
{
    using System;

    public static class TimeSpanUtilities
    {
        public static DateTime ToDateTime(this long ms)
        {
            return new DateTime(1970, 1, 1).AddMilliseconds(ms);
        }

        public static bool LessThenDays(this DateTime dateTime, int days)
        {
            return (DateTime.UtcNow - dateTime).TotalDays < days;
        }

        public static string ToReadableAgeString(this TimeSpan span)
        {
            return string.Format("{0:0}", span.Days / 365.25);
        }

        public static string ToReadableString(this TimeSpan span)
        {
            span = new TimeSpan(span.Days * 24 / 8 + span.Hours / 8, span.Hours % 8, span.Minutes, 0);
            string formatted = string.Format("{0}{1}{2}",
                (span.Duration().Hours / 8) > 0 ? string.Format("{0:0}d", span.Hours/8) : string.Empty,
                span.Duration().Hours > 0 ? string.Format("{0:0}h", span.Hours) : string.Empty,
                span.Duration().Minutes > 0 ? string.Format("{0:0}m", span.Minutes) : string.Empty);

            if (string.IsNullOrEmpty(formatted)) return "-";
            return formatted;
        }

        public static string ToRelativeString(this DateTime dt)
        {
            var ts = new TimeSpan(DateTime.UtcNow.Ticks - dt.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            if (delta < 60)
            {
                return ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago";
            }
            if (delta < 120)
            {
                return "a minute ago";
            }
            if (delta < 2700) // 45 * 60
            {
                return ts.Minutes + " minutes ago";
            }
            if (delta < 5400) // 90 * 60
            {
                return "an hour ago";
            }
            if (delta < 86400) // 24 * 60 * 60
            {
                return ts.Hours + " hours ago";
            }
            if (delta < 172800) // 48 * 60 * 60
            {
                return "yesterday";
            }
            if (delta < 2592000) // 30 * 24 * 60 * 60
            {
                return ts.Days + " days ago";
            }
            if (delta < 31104000) // 12 * 30 * 24 * 60 * 60
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? "one month ago" : months + " months ago";
            }
            int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
            return years <= 1 ? "one year ago" : years + " years ago";
        }
    }
}