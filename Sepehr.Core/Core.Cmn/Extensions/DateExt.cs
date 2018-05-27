using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Cmn.Extensions
{
    public static class DateExt
    {
        public static IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }

        public static int ConvertToNumber(this DateTime dateTime)
        {
            string timeString24Hour = dateTime.ToString("HH:mm", CultureInfo.CurrentCulture);
            string[] timeSplit = timeString24Hour.Split(':');

            return (int.Parse(timeSplit[0]) * 60) + int.Parse(timeSplit[1]);
        }
        public static string ToStringInEnCulture(this DateTime dateTime)
        {
            var currentCultureName = System.Threading.Thread.CurrentThread.CurrentCulture.Name;
            System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
            var date = dateTime.ToString();
            System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(currentCultureName);
            return date;

        }
        public static DateTime ShamsiToMiladi(this DateTime dateTime)
        {

            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, new System.Globalization.PersianCalendar());
        }

        private static ConcurrentDictionary<DateTime, string> _persianDates = new ConcurrentDictionary<DateTime, string>();
        public static string MiladiToShamsi(this DateTime miladiDate)
        {
            string result;
            if (_persianDates.TryGetValue(miladiDate.Date, out result) == false)
            {
                PersianCalendar persianCalendar = new PersianCalendar();
                result = $"{persianCalendar.GetYear(miladiDate)}/{persianCalendar.GetMonth(miladiDate).FixNumber(2)}/{persianCalendar.GetDayOfMonth(miladiDate).FixNumber(2)}";
                _persianDates[miladiDate.Date] = result;
            }

            return result;
        }

        public static DateTime LastDayOfShamsiMonth(this string persianDate)
        {
            int year = int.Parse(persianDate.Split('/')[0]);
            int month = int.Parse(persianDate.Split('/')[1]);
            int day = int.Parse(persianDate.Split('/')[2]);
            PersianCalendar persianCalendar = new PersianCalendar();
            DateTime miladiDate = new DateTime(year, month, day, persianCalendar);
            int lastDayOfMonth = persianCalendar.GetDaysInMonth(year, month);

            for (int i = day; i < lastDayOfMonth; i++)
            {
                miladiDate = miladiDate.AddDays(1);
            }

            return miladiDate;
        }

        public static string ToYearMonth(this DateTime dateTime)
        {
            return dateTime.ToString("MMMMMMMMMM, yyyy");
        }

        public static DateTime ToLocalKind(this DateTime dateTime)
        {
            return DateTime.SpecifyKind(dateTime, DateTimeKind.Local);
        }
    }
}
