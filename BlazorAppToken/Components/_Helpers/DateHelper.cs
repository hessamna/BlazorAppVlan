using System.Globalization;

namespace BlazorApptToken.Components._Helpers
{
    public static class PersianDateHelper
    {
        private static PersianCalendar _persianCalendar = new PersianCalendar();

        public static string ToPersianDate(this DateTime date, bool showTime = false)
        {
            int year = _persianCalendar.GetYear(date);
            int month = _persianCalendar.GetMonth(date);
            int day = _persianCalendar.GetDayOfMonth(date);

            if (!showTime)
            {
                // فقط تاریخ
                return $"{year:0000}/{month:00}/{day:00}";
            }
            else
            {
                // تاریخ + ساعت
                int hour = date.Hour;
                int minute = date.Minute;
                int second = date.Second;
                return $"{year:0000}/{month:00}/{day:00} {hour:00}:{minute:00}:{second:00}";
            }
        }
    }
}
