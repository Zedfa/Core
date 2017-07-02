﻿namespace Core.Cmn.Resources
{
    /// <summary>
    /// Farsi Localizer
    /// </summary>
    public class FALocalizer : BaseLocalizer
    {
        public override string GetLocalizedString(StringID id)
        {
            switch (id)
            {
                case StringID.Empty: return string.Empty;
                case StringID.Numbers_0: return "۰";
                case StringID.Numbers_1: return "۱";
                case StringID.Numbers_2: return "۲";
                case StringID.Numbers_3: return "۳";
                case StringID.Numbers_4: return "۴";
                case StringID.Numbers_5: return "۵";
                case StringID.Numbers_6: return "۶";
                case StringID.Numbers_7: return "۷";
                case StringID.Numbers_8: return "۸";
                case StringID.Numbers_9: return "۹";

                case StringID.FADateTextBox_Required: return "فيلد اجباری میباشد";
                case StringID.FAMonthView_None: return "خالی";
                case StringID.FAMonthView_Today: return "امروز";

                case StringID.PersianDate_InvalidDateFormat: return "ساختار تاریخ مجاز نمیباشد.";
                case StringID.PersianDate_InvalidDateTime: return "مقدار زمان/ساعت صحیح نمیباشد.";
                case StringID.PersianDate_InvalidDateTimeLength: return "متن وارد شده برای زمان/ساعت صحیح نمیباشد.";
                case StringID.PersianDate_InvalidDay: return "مقدار روز صحیح نمیباشد.";
                case StringID.PersianDate_InvalidEra: return "محدوده وارد شده صحیح نمیباشد.";
                case StringID.PersianDate_InvalidFourDigitYear: return "مقدار وارد شده را نمیتوان به سال تبدیل کرد.";
                case StringID.PersianDate_InvalidHour: return "مقدار ساعت صحیح نمیباشد.";
                case StringID.PersianDate_InvalidLeapYear: return "این سال ، سال کبیسه نیست. مقدار روز صحیح نمیباشد.";
                case StringID.PersianDate_InvalidMinute: return "مقدار دقیقه صحیح نمیباشد.";
                case StringID.PersianDate_InvalidMonth: return "مقدار ماه صحیح نمیباشد.";
                case StringID.PersianDate_InvalidMonthDay: return "مقدار ماه/روز صحیح نمیباشد.";
                case StringID.PersianDate_InvalidSecond: return "مقدار ثانیه صحیح نمیباشد.";
                case StringID.PersianDate_InvalidTimeFormat: return "ساختار زمان صحیح نمیباشد.";
                case StringID.PersianDate_InvalidYear: return "مقدار سال صحیح نمیباشد.";

                case StringID.Validation_Cancel: return "مقدار انتخاب شده مجاز نمیباشد.";
                case StringID.Validation_NotValid: return "مقدار انتخاب شده در محدوده مجاز نمیباشد.";
                case StringID.Validation_Required: return "انتخاب اجباری. لطفا مقداری برای این فیلد وارد کنید.";
                case StringID.Validation_NullText: return "[هیج مقداری انتخاب نشده]";

                case StringID.MessageBox_Ok: return "قبول";
                case StringID.MessageBox_Cancel: return "لغو";
                case StringID.MessageBox_Abort: return "لغو";
                case StringID.MessageBox_Ignore: return "ادامه عملیات";
                case StringID.MessageBox_Retry: return "سعی مجدد";
                case StringID.MessageBox_No: return "خیر";                    
                case StringID.MessageBox_Yes: return "بله";
            }

            return "";
        }
    }
}
