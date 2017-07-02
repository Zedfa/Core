using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Cmn.Extensions
{
    public static class DateExt
    {
        private static ILogService _logService = AppBase.LogService;
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

        public static string MiladiToShamsi(this DateTime miladiDate)
        {
            PersianCalendar persianCalendar = new PersianCalendar();
            return persianCalendar.GetYear(miladiDate) + "/" + FixNumber(persianCalendar.GetMonth(miladiDate).ToString(), 2) + "/" + FixNumber(persianCalendar.GetDayOfMonth(miladiDate).ToString(), 2);
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

        private static string FixNumber(string number, int desiredLength)
        {
            if (number.Length < desiredLength)
            {
                while (number.Length < desiredLength)
                {
                    number = "0" + number;
                }
            }

            return number;
        }

        public static string ToYearMonth(this DateTime dateTime)
        {
            return dateTime.ToString("MMMMMMMMMM, yyyy");
        }

        public static string ToArabYearMonth(this DateTime dateTime)
        {
            var currentCulture = System.Threading.Thread.CurrentThread.CurrentCulture;

            try
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("ar-SA");
                string result = dateTime.ToString("MMMMMMMMMM, yyyy");
                System.Threading.Thread.CurrentThread.CurrentCulture = currentCulture;
                return result;
            }
            catch (Exception ex)
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = currentCulture;

                System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(ex, true);
                System.Diagnostics.StackFrame[] frames = st.GetFrames();
                string x = "";
                // Iterate over the frames extracting the information you need
                foreach (System.Diagnostics.StackFrame frame in frames)
                {
                    //   x = ""+ frame.GetFileName()+"";
                    x += "    ** FileName:" + frame.GetFileName() + "** MethodName:" + frame.GetMethod().Name + "** LineNumber:" + frame.GetFileLineNumber() + "** ColumnNumber:" + frame.GetFileColumnNumber();
                }
                var eLog = _logService.GetEventLogObj();
                eLog.OccuredException = ex;
                eLog.UserId = "---";
                eLog.CustomMessage = String.Format("Incomming parameter: {0} in ToArabYearMonth,{1}", dateTime, x);
                _logService.Handle(eLog);

                return String.Empty;
            }
        }

        public static DateTime ToLocalKind(this DateTime dateTime)
        {
            return DateTime.SpecifyKind(dateTime, DateTimeKind.Local);
        }
    }
}
