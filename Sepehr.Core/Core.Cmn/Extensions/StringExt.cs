using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Core.Cmn.FarsiUtils;
using System.Globalization;
using Core.Cmn.Extensions;
using System.Text.RegularExpressions;

namespace Core.Cmn.Extensions
{
    public static class StringExt
    {

        private static ILogService _logService = AppBase.LogService;

        public static string ExtractNumbers(this string input)
        {
            return new String(input.ToCharArray().Where(c => Char.IsDigit(c)).ToArray());
        }
        /// <summary>
        /// in method character haye numebre farsi mesle ٠١٢٣٤٥٦٧٨٩ ro tabdil mikone ke 0123456789
        /// http://stackoverflow.com/questions/5879437/how-to-convert-arabic-number-to-int
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToEnglishNumber(this string input)
        {
            string englishNumbers = string.Empty;

            if (!string.IsNullOrEmpty(input))
            {
                for (int i = 0; i < input.Length; i++)
                {
                    if (Char.IsDigit(input[i]))
                    {
                        englishNumbers += char.GetNumericValue(input, i);

                    }
                    else
                    {
                        englishNumbers += input[i].ToString();
                    }
                }
            }

            return englishNumbers;
        }

        public static DateTime ToDateTime(this string persianDateStr)
        {
            if (persianDateStr.Length == 6)
            {
                var strDate = string.Format("13{0}/{1}/{2}", persianDateStr.Substring(0, 2), persianDateStr.Substring(2, 2), persianDateStr.Substring(4, 2));
                DateTime dateTime = PersianDateConverter.ToGregorianDateTime(strDate);
                return dateTime;
            }
            else
            {
                var strDate = string.Format("{0}/{1}/{2}", persianDateStr.Substring(0, 4), persianDateStr.Substring(4, 2), persianDateStr.Substring(6, 2));
                DateTime dateTime = PersianDateConverter.ToGregorianDateTime(strDate);
                return dateTime;
            }
        }
        public static int ToPersianDate_8Num(this PersianDate date)
        {
            var persianDate = new PersianDate(date).ToString("d");
            int persianDate_8Num = int.Parse(persianDate.Replace("/", string.Empty));
            return persianDate_8Num;
        }

        private static string[] yekan = new string[10] { "صفر", "یک", "دو", "سه", "چهار", "پنج", "شش", "هفت", "هشت", "نه" };
        private static string[] dahgan = new string[10] { "", "", "بیست", "سی", "چهل", "پنجاه", "شصت", "هفتاد", "هشتاد", "نود" };
        private static string[] dahyek = new string[10] { "ده", "یازده", "دوازده", "سیزده", "چهارده", "پانزده", "شانزده", "هفده", "هجده", "نوزده" };
        private static string[] sadgan = new string[10] { "", "یکصد", "دویست", "سیصد", "چهارصد", "پانصد", "ششصد", "هفتصد", "هشتصد", "نهصد" };
        private static string[] basex = new string[5] { "", "هزار", "میلیون", "میلیارد", "تریلیون" };
        /// <summary>
        /// in method dar vaghe k arabi ra be k farsi va ye farsi ra be ye arabi tabdil mikonad, dalile an serfan baraye ravieiist k dar sepehr anjam shode va be dalile hajme ziade data va coste ziad felan ghabele taghir nist...
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string CorrectPersianChars(this string str)
        {
            return str.Replace("ك", "ک").Replace("ی", "ي");//.Replace("ه", "ة");
        }

        public static string FormatWith(this string instance, params object[] args)
        {
            return string.Format(CultureInfo.CurrentCulture, instance, args);
        }

        public static bool HasValue(this string value)
        {
            return !string.IsNullOrEmpty(value);
        }


        public static string ConvertNumberToChar(this string snum)
        {
            string stotal = "";
            if (snum == "0")
            {
                return yekan[0];
            }
            else
            {
                snum = snum.PadLeft(((snum.Length - 1) / 3 + 1) * 3, '0');
                int L = snum.Length / 3 - 1;
                for (int i = 0; i <= L; i++)
                {
                    int b = int.Parse(snum.Substring(i * 3, 3));
                    if (b != 0)
                        stotal = stotal + GetNumberName(b) + " " + basex[L - i] + " و ";
                }
                stotal = stotal.Substring(0, stotal.Length - 3);
            }
            return stotal;
        }

        private static string GetNumberName(int num)
        {
            string s = "";
            int d3, d12;
            d12 = num % 100;
            d3 = num / 100;
            if (d3 != 0)
                s = sadgan[d3] + " و ";
            if ((d12 >= 10) && (d12 <= 19))
            {
                s = s + dahyek[d12 - 10];
            }
            else
            {
                int d2 = d12 / 10;
                if (d2 != 0)
                    s = s + dahgan[d2] + " و ";
                int d1 = d12 % 10;
                if (d1 != 0)
                    s = s + yekan[d1] + " و ";
                s = s.Substring(0, s.Length - 3);
            };
            return s;
        }

        #region From Old Core



        public static bool IsValidDateTime(this string str, string format)
        {
            int day, year, month;
            DateTime dateTime;
            if (string.IsNullOrWhiteSpace(str))
                return false;
            try
            {

                if (str.Split('/').Count() != 3)
                    return false;
                if (System.Threading.Thread.CurrentThread.CurrentUICulture.Name == "fa-IR")
                {
                    var miladiDateInString = str.ShamsiToMiladi().ToString(format);
                    var miladiDate = DateTime.ParseExact(miladiDateInString, format, null);
                    var persianDate = new Core.Cmn.FarsiUtils.PersianDate(miladiDate);
                    day = persianDate.Day;
                    month = persianDate.Month;
                    year = persianDate.Year;
                }
                else
                {
                    var date = DateTime.ParseExact(str, format, null);
                    year = date.Year;
                    month = date.Month;
                    day = date.Day;
                }

                if (month <= 0 || month > 12)
                    return false;

                if (day <= 0 || day > 31)
                    return false;

                if (Thread.CurrentThread.CurrentUICulture.Name == "fa-IR")
                {

                    if (year > 1410 || year < 1300)
                        return false;

                    if (month > 6 && day > 30)
                        return false;

                    dateTime = new DateTime(year, month, day, new System.Globalization.PersianCalendar());

                    if (!(dateTime > DateTime.Parse("1/1/1753 12:00:00 AM") && dateTime < DateTime.Parse("1/1/2054 11:59:59 PM")))
                    {

                        //var eLog = _logService.GetEventLogObj();

                        //eLog.CustomMessage = String.Format("IsValidDateTime : {0} is invalid Date for {1}", str, Thread.CurrentThread.CurrentUICulture.Name);
                        //_logService.Handle(eLog);
                        _logService.Write(String.Format("IsValidDateTime : {0} is invalid Date for {1}", str, Thread.CurrentThread.CurrentUICulture.Name));


                        return false;
                    }
                    return true;
                }
                else if (DateTime.TryParseExact(str, format, null, DateTimeStyles.None, out dateTime))
                {
                    if (!(dateTime > DateTime.Parse("1/1/1753 12:00:00 AM") && dateTime < DateTime.Parse("1/1/2054 11:59:59 PM")))
                    {
                        //var eLog = _logService.GetEventLogObj();
                        //eLog.CustomMessage = String.Format("IsValidDateTime : {0} is invalid Date for {1}", str, Thread.CurrentThread.CurrentUICulture.Name);
                        //_logService.Handle(eLog);
                        _logService.Write(String.Format("IsValidDateTime : {0} is invalid Date for {1}", str, Thread.CurrentThread.CurrentUICulture.Name));

                        return false;
                    }
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                //System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(ex, true);
                //System.Diagnostics.StackFrame[] frames = st.GetFrames();
                //string x = "";
                //// Iterate over the frames extracting the information you need
                //foreach (System.Diagnostics.StackFrame frame in frames)
                //{
                //    //   x = ""+ frame.GetFileName()+"";
                //    x += "    ** FileName:" + frame.GetFileName() + "** MethodName:" + frame.GetMethod().Name + "** LineNumber:" + frame.GetFileLineNumber() + "** ColumnNumber:" + frame.GetFileColumnNumber();
                //}
                //var eLog = _logService.GetEventLogObj();
                //eLog.OccuredException = ex;
                //eLog.CustomMessage = String.Format("IsValidDateTime : {0} is invalid Date for {1},{2}", str, Thread.CurrentThread.CurrentUICulture.Name, x);
                //_logService.Handle(eLog);
                _logService.Handle(ex, String.Format("IsValidDateTime : {0} is invalid Date for {1}", str, Thread.CurrentThread.CurrentUICulture.Name));

                return false;
            }


        }



        //public static bool IsValidDateTime(this string str)
        //{
        //    System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("ar-SA");
        //    if (string.IsNullOrWhiteSpace(str))
        //        return false;
        //    DateTime dateTime;
        //    try
        //    {
        //        if (DateTime.TryParse(str, out dateTime))
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            dateTime = new DateTime(int.Parse(str.Split('/')[0]), int.Parse(str.Split('/')[1]), int.Parse(str.Split('/')[2]), new System.Globalization.PersianCalendar());
        //            return true;
        //        }
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}
        public static DateTime ToMiladiDate(this string str)
        {
            if (!string.IsNullOrWhiteSpace(str))
            {
                return new DateTime(int.Parse(str.Split('/')[0]), int.Parse(str.Split('/')[1]), int.Parse(str.Split('/')[2]));
            }
            else
            {
                return DateTime.Now;
            }

        }
        public static DateTime ShamsiToMiladi(this string persianDate)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(persianDate))
                {
                    return new DateTime(int.Parse(persianDate.Split('/')[0]), int.Parse(persianDate.Split('/')[1]), int.Parse(persianDate.Split('/')[2]), new System.Globalization.PersianCalendar());
                }
                else
                {
                    return DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                //System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(ex, true);
                //System.Diagnostics.StackFrame[] frames = st.GetFrames();
                //string x = "";
                //// Iterate over the frames extracting the information you need
                //foreach (System.Diagnostics.StackFrame frame in frames)
                //{
                //    //   x = ""+ frame.GetFileName()+"";
                //    x += "    ** FileName:" + frame.GetFileName() + "** MethodName:" + frame.GetMethod().Name + "** LineNumber:" + frame.GetFileLineNumber() + "** ColumnNumber:" + frame.GetFileColumnNumber();
                //}
                //var eLog = _logService.GetEventLogObj();
                //eLog.OccuredException = ex;
                //eLog.UserId = "DateExt";
                //eLog.CustomMessage = "parse " + persianDate + "ShamsiToMiladi" + x;
                //_logService.Handle(eLog);
                _logService.Handle(ex, customMessage:"parse " + persianDate + "ShamsiToMiladi", source:"DateExt");

            }
            return DateTime.Now;
        }

        public static int ConvertTimeToNumber(this string time)
        {
            var dateTime = Convert.ToDateTime(time);
            string timeString24Hour = dateTime.ToString("HH:mm", CultureInfo.CurrentCulture);
            string[] timeSplit = timeString24Hour.Split(':');

            return (int.Parse(timeSplit[0]) * 60) + int.Parse(timeSplit[1]);
        }

        public static string GetPersianMonth(this string persianDate, bool includeYear = true)
        {
            string[] persianDateParts = persianDate.Split('/');
            string year = persianDateParts[0];
            int month = int.Parse(persianDateParts[1]);
            string day = persianDateParts[2];
            string[] monthNames = "فروردین,اردیبهشت,خرداد,تیر,مرداد,شهریور,مهر,آبان,آذر,دی,بهمن,اسفند".Split(',');
            string monthName = String.Empty;

            switch (month)
            {
                case 1:
                    monthName = monthNames[0];
                    break;
                case 2:
                    monthName = monthNames[1];
                    break;
                case 3:
                    monthName = monthNames[2];
                    break;
                case 4:
                    monthName = monthNames[3];
                    break;
                case 5:
                    monthName = monthNames[4];
                    break;
                case 6:
                    monthName = monthNames[5];
                    break;
                case 7:
                    monthName = monthNames[6];
                    break;
                case 8:
                    monthName = monthNames[7];
                    break;
                case 9:
                    monthName = monthNames[8];
                    break;
                case 10:
                    monthName = monthNames[9];
                    break;
                case 11:
                    monthName = monthNames[10];
                    break;
                case 12:
                    monthName = monthNames[11];
                    break;
            }

            if (includeYear)
            {
                return String.Format("{0} {1}", monthName, year);
            }
            return monthName;
        }

        public static List<string> GetPersianMonthes(this string persianDate, bool includeYear = true)
        {
            List<string> result = new List<string>();
            string[] persianDateParts = persianDate.Split('/');
            string year = persianDateParts[0];
            int month = int.Parse(persianDateParts[1]);
            string day = persianDateParts[2];

            for (int i = month; i < 13; i++)
            {
                result.Add(GetPersianMonth(String.Format("{0}/{1}/{2}", year, i, day), includeYear));
            }

            return result;
        }

        public static int ToPersianYear(this string persianDate)
        {
            return int.Parse(persianDate.Split('/')[0]);
        }

        public static int ToPersianMonth(this string persianDate)
        {
            return int.Parse(persianDate.Split('/')[1]);
        }

        public static int ToPersianDay(this string persianDate)
        {
            return int.Parse(persianDate.Split('/')[2]);
        }

        #endregion

        public static string ParseNumber(this string num, bool returnOnlyNum = false)
        {
            StringBuilder sb = new StringBuilder();
            var array = num.ToArray();
            foreach (char item in array)
            {
                switch (item)
                {
                    case '۰':
                    case '0':
                        sb.Append(0);
                        break;
                    case '۱':
                    case '1':
                        sb.Append(1);
                        break;
                    case '۲':
                    case '2':
                        sb.Append(2);
                        break;
                    case '۳':
                    case '3':
                        sb.Append(3);
                        break;
                    case '۴':
                    case '4':
                        sb.Append(4);
                        break;
                    case '۵':
                    case '5':
                        sb.Append(5);
                        break;
                    case '۶':
                    case '6':
                        sb.Append(6);
                        break;
                    case '۷':
                    case '7':
                        sb.Append(7);
                        break;
                    case '۸':
                    case '8':
                        sb.Append(8);
                        break;
                    case '۹':
                    case '9':
                        sb.Append(9);
                        break;
                    default:
                        if (!returnOnlyNum) { sb.Append(item); }
                        break;

                }
            }

            return sb.ToString();
        }

        public static string WithoutSpace(this string value)
        {
            return System.Text.RegularExpressions.Regex.Replace(value, @"^[\s,]+|[\s,]+$", "");
        }
    }
}
