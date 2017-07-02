using Core.Cmn.Resources;
using System;
using System.Globalization;
namespace Core.Cmn.FarsiUtils
{
    /// <summary>
    /// Helper class to convert numbers to it's farsi equivalent. Use this class' methods to overcome a problem in displaying farsi numeric values.
    /// </summary>
    public sealed class toFarsi
    {

        /// <summary>
        /// Converts a number in string format e.g. 14500 to its localized version, if <c>Localized</c> value is set to <c>true</c>.
        /// </summary>
        /// <param name="EnglishNumber"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static string Convert(string EnglishNumber, CultureInfo culture)
        {
            string numEnglish = "";
            string numTemp = "";

            for (int i = 0; i < EnglishNumber.Length; i++)
            {
                numTemp = EnglishNumber.Substring(i, 1);
                switch (numTemp)
                {
                    case "0":
                        numEnglish = numEnglish + FALocalizeManager.GetLocalizerByCulture(culture).GetLocalizedString(StringID.Numbers_0);
                        break;
                    case "1":
                        numEnglish = numEnglish + FALocalizeManager.GetLocalizerByCulture(culture).GetLocalizedString(StringID.Numbers_1);
                        break;
                    case "2":
                        numEnglish = numEnglish + FALocalizeManager.GetLocalizerByCulture(culture).GetLocalizedString(StringID.Numbers_2);
                        break;
                    case "3":
                        numEnglish = numEnglish + FALocalizeManager.GetLocalizerByCulture(culture).GetLocalizedString(StringID.Numbers_3);
                        break;
                    case "4":
                        numEnglish = numEnglish + FALocalizeManager.GetLocalizerByCulture(culture).GetLocalizedString(StringID.Numbers_4);
                        break;
                    case "5":
                        numEnglish = numEnglish + FALocalizeManager.GetLocalizerByCulture(culture).GetLocalizedString(StringID.Numbers_5);
                        break;
                    case "6":
                        numEnglish = numEnglish + FALocalizeManager.GetLocalizerByCulture(culture).GetLocalizedString(StringID.Numbers_6);
                        break;
                    case "7":
                        numEnglish = numEnglish + FALocalizeManager.GetLocalizerByCulture(culture).GetLocalizedString(StringID.Numbers_7);
                        break;
                    case "8":
                        numEnglish = numEnglish + FALocalizeManager.GetLocalizerByCulture(culture).GetLocalizedString(StringID.Numbers_8);
                        break;
                    case "9":
                        numEnglish = numEnglish + FALocalizeManager.GetLocalizerByCulture(culture).GetLocalizedString(StringID.Numbers_9);
                        break;
                    default:
                        numEnglish = numEnglish + numTemp;
                        break;
                }
            }

            return (numEnglish);
        }

        /// <summary>
        /// Converts an English number to it's Farsi value.
        /// </summary>
        /// <remarks>This method only converts the numbers in a string, and does not convert any non-numeric characters.</remarks>
        /// <param name="EnglishNumber"></param>
        /// <returns></returns>
        public static string Convert(string EnglishNumber)
        {
            string numEnglish = "";
            string numTemp = "";

            for (int i = 0; i < EnglishNumber.Length; i++)
            {
                numTemp = EnglishNumber.Substring(i, 1);
                switch (numTemp)
                {
                    case "0":
                        numEnglish = numEnglish + "۰";
                        break;
                    case "1":
                        numEnglish = numEnglish + "۱";
                        break;
                    case "2":
                        numEnglish = numEnglish + "۲";
                        break;
                    case "3":
                        numEnglish = numEnglish + "۳";
                        break;
                    case "4":
                        numEnglish = numEnglish + "۴";
                        break;
                    case "5":
                        numEnglish = numEnglish + "۵";
                        break;
                    case "6":
                        numEnglish = numEnglish + "۶";
                        break;
                    case "7":
                        numEnglish = numEnglish + "۷";
                        break;
                    case "8":
                        numEnglish = numEnglish + "۸";
                        break;
                    case "9":
                        numEnglish = numEnglish + "۹";
                        break;
                    default:
                        numEnglish = numEnglish + numTemp;
                        break;
                }
            }

            return (numEnglish);
        }
        /// <summary>
        /// Converts an english text that type by Farsi keybord to English text or Conversely(English To Farsi).
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ConvertStrToEnglishOrPersianSide(string str)
        {
            string result = string.Empty;
            if (str == null)
            {
                str = string.Empty;
                result = null;
            }
            char[] alphabet = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'w', 'x', 'y', 'z', ' ' };
            for (int i = 0; i < str.Length; i++)
            {


                str.Replace("ریال", "ق");
                switch (System.Convert.ToChar(str.Substring(i, 1)))
                {
                    //Common Characters
                    case ' ':
                        result = result + System.Convert.ToString(' ');
                        break;
                    case ':':
                        result = result + System.Convert.ToString(':');
                        break;
                    case '"':
                        result = result + System.Convert.ToString('"');
                        break;
                    case '?':
                        result = result + System.Convert.ToString('؟');
                        break;
                    case '؟':
                        result = result + System.Convert.ToString('?');
                        break;

                    //Farsi to English
                    case 'ض':
                        result = result + System.Convert.ToString('q');
                        break;
                    case 'ص':
                        result = result + System.Convert.ToString('w');
                        break;
                    case 'ث':
                        result = result + System.Convert.ToString('e');
                        break;
                    case 'ق':
                        result = result + System.Convert.ToString('r');
                        break;
                    case 'ف':
                        result = result + System.Convert.ToString('t');
                        break;
                    case 'غ':
                        result = result + System.Convert.ToString('y');
                        break;
                    case 'ع':
                        result = result + System.Convert.ToString('u');
                        break;
                    case 'ه':
                        result = result + System.Convert.ToString('i');
                        break;
                    case 'خ':
                        result = result + System.Convert.ToString('o');
                        break;
                    case 'ح':
                        result = result + System.Convert.ToString('p');
                        break;
                    case 'ج':
                        result = result + System.Convert.ToString('[');
                        break;
                    case 'چ':
                        result = result + System.Convert.ToString(']');
                        break;
                    case 'ش':
                        result = result + System.Convert.ToString('a');
                        break;
                    case 'س':
                        result = result + System.Convert.ToString('s');
                        break;
                    case 'ی':
                        result = result + System.Convert.ToString('d');
                        break;
                    case 'ب':
                        result = result + System.Convert.ToString('f');
                        break;
                    case 'ل':
                        result = result + System.Convert.ToString('g');
                        break;
                    case 'ا':
                        result = result + System.Convert.ToString('h');
                        break;
                    case 'ت':
                        result = result + System.Convert.ToString('j');
                        break;
                    case 'ن':
                        result = result + System.Convert.ToString('k');
                        break;
                    case 'م':
                        result = result + System.Convert.ToString('l');
                        break;
                    case 'ک':
                        result = result + System.Convert.ToString(';');
                        break;
                    case 'گ':
                        result = result + System.Convert.ToString('\'');
                        break;
                    case 'ظ':
                        result = result + System.Convert.ToString('z');
                        break;
                    case 'ط':
                        result = result + System.Convert.ToString('x');
                        break;
                    case 'ز':
                        result = result + System.Convert.ToString('c');
                        break;
                    case 'ر':
                        result = result + System.Convert.ToString('v');
                        break;
                    case 'ذ':
                        result = result + System.Convert.ToString('b');
                        break;
                    case 'د':
                        result = result + System.Convert.ToString('n');
                        break;
                    case 'ئ':
                        result = result + System.Convert.ToString('m');
                        break;
                    case 'و':
                        result = result + System.Convert.ToString(',');
                        break;
                    case '.':
                        result = result + System.Convert.ToString('.');
                        break;
                    case 'ً':
                        result = result + System.Convert.ToString('Q');
                        break;
                    case 'ٌ':
                        result = result + System.Convert.ToString('W');
                        break;
                    case 'ٍ':
                        result = result + System.Convert.ToString('E');
                        break;
                    case '،':
                        result = result + System.Convert.ToString('T');
                        break;
                    case '؛':
                        result = result + System.Convert.ToString('Y');
                        break;
                    case '٫':
                        result = result + System.Convert.ToString('U');
                        break;
                    case 'ة':
                        result = result + System.Convert.ToString('I');
                        break;
                    case ']':
                        int t1 = 0;
                        foreach (char c in alphabet)
                        {
                            if (i + 1 < str.Length)
                            {
                                if (c == System.Convert.ToChar(str.Substring(i + 1, 1)))
                                {
                                    t1 = 1;
                                    break;
                                }
                            }
                        }
                        if (t1 == 1)
                            result = result + System.Convert.ToString('چ');
                        else
                            result = result + System.Convert.ToString('O');
                        break;
                    case '[':
                        int t2 = 0;
                        foreach (char c in alphabet)
                        {
                            if (i + 1 < str.Length)
                            {
                                if (c == System.Convert.ToChar(str.Substring(i + 1, 1)))
                                {
                                    t2 = 1;
                                    break;
                                }
                            }
                        }
                        if (t2 == 1)
                            result = result + System.Convert.ToString('ج');
                        else
                            result = result + System.Convert.ToString('P');
                        break;
                    case '}':
                        result = result + System.Convert.ToString('{');
                        break;
                    case '{':
                        result = result + System.Convert.ToString('}');
                        break;
                    case 'َ':
                        result = result + System.Convert.ToString('A');
                        break;
                    case 'ُ':
                        result = result + System.Convert.ToString('S');
                        break;
                    case 'ِ':
                        result = result + System.Convert.ToString('D');
                        break;
                    case 'ّ':
                        result = result + System.Convert.ToString('F');
                        break;
                    case 'ۀ':
                        result = result + System.Convert.ToString('G');
                        break;
                    case 'آ':
                        result = result + System.Convert.ToString('H');
                        break;
                    case 'ـ':
                        result = result + System.Convert.ToString('J');
                        break;
                    case '»':
                        result = result + System.Convert.ToString('K');
                        break;
                    case '«':
                        result = result + System.Convert.ToString('L');
                        break;
                    case 'ؤ':
                        result = result + System.Convert.ToString('Z');
                        break;
                    case '‍':
                        result = result + System.Convert.ToString('X');
                        break;
                    case 'ي':
                        result = result + System.Convert.ToString('V');
                        break;
                    case 'إ':
                        result = result + System.Convert.ToString('B');
                        break;
                    case 'أ':
                        result = result + System.Convert.ToString('N');
                        break;
                    case 'ء':
                        result = result + System.Convert.ToString('M');
                        break;
                    case '>':
                        result = result + System.Convert.ToString('<');
                        break;
                    case '<':
                        result = result + System.Convert.ToString('>');
                        break;
                    case 'پ':
                        result = result + System.Convert.ToString('`');
                        break;
                    case 'ژ':
                        result = result + System.Convert.ToString("C");
                        break;
                    //case 'ژ':
                    //    str_new = str_new + System.Convert.ToString("");
                    //    break;

                    //English to Farsi

                    case 'q':
                        result = result + System.Convert.ToString('ض');
                        break;
                    case 'w':
                        result = result + System.Convert.ToString('ص');
                        break;
                    case 'e':
                        result = result + System.Convert.ToString('ث');
                        break;
                    case 'r':
                        result = result + System.Convert.ToString('ق');
                        break;
                    case 't':
                        result = result + System.Convert.ToString('ف');
                        break;
                    case 'y':
                        result = result + System.Convert.ToString('غ');
                        break;
                    case 'u':
                        result = result + System.Convert.ToString('ع');
                        break;
                    case 'i':
                        result = result + System.Convert.ToString('ه');
                        break;
                    case 'o':
                        result = result + System.Convert.ToString('خ');
                        break;
                    case 'p':
                        result = result + System.Convert.ToString('ح');
                        break;
                    case 'a':
                        result = result + System.Convert.ToString('ش');
                        break;
                    case 's':
                        result = result + System.Convert.ToString('س');
                        break;
                    case 'd':
                        result = result + System.Convert.ToString('ی');
                        break;
                    case 'f':
                        result = result + System.Convert.ToString('ب');
                        break;
                    case 'g':
                        result = result + System.Convert.ToString('ل');
                        break;
                    case 'h':
                        result = result + System.Convert.ToString('ا');
                        break;
                    case 'j':
                        result = result + System.Convert.ToString('ت');
                        break;
                    case 'k':
                        result = result + System.Convert.ToString('ن');
                        break;
                    case 'l':
                        result = result + System.Convert.ToString('م');
                        break;
                    case ';':
                        result = result + System.Convert.ToString('ک');
                        break;
                    case '\'':
                        result = result + System.Convert.ToString('گ');
                        break;
                    case 'z':
                        result = result + System.Convert.ToString('ظ');
                        break;
                    case 'x':
                        result = result + System.Convert.ToString('ط');
                        break;
                    case 'c':
                        result = result + System.Convert.ToString('ز');
                        break;
                    case 'v':
                        result = result + System.Convert.ToString('ر');
                        break;
                    case 'b':
                        result = result + System.Convert.ToString('ذ');
                        break;
                    case 'n':
                        result = result + System.Convert.ToString('د');
                        break;
                    case 'm':
                        result = result + System.Convert.ToString('ئ');
                        break;
                    case ',':
                        result = result + System.Convert.ToString('و');
                        break;
                    case 'Q':
                        result = result + System.Convert.ToString('ً');
                        break;
                    case 'W':
                        result = result + System.Convert.ToString('ٌ');
                        break;
                    case 'E':
                        result = result + System.Convert.ToString('ٍ');
                        break;
                    case 'R':
                        result = result + "ريال";
                        break;
                    case 'T':
                        result = result + System.Convert.ToString('،');
                        break;
                    case 'Y':
                        result = result + System.Convert.ToString('؛');
                        break;
                    case 'A':
                        result = result + System.Convert.ToString('َ');
                        break;
                    case 'S':
                        result = result + System.Convert.ToString('ُ');
                        break;
                    case 'D':
                        result = result + System.Convert.ToString('ِ');
                        break;
                    case 'M':
                        result = result + System.Convert.ToString('ء');
                        break;
                    case 'F':
                        result = result + System.Convert.ToString('ّ');
                        break;
                    case 'H':
                        result = result + System.Convert.ToString('آ');
                        break;
                    case 'J':
                        result = result + System.Convert.ToString('ـ');
                        break;
                    case 'K':
                        result = result + System.Convert.ToString('»');
                        break;
                    case 'L':
                        result = result + System.Convert.ToString('«');
                        break;
                    case 'Z':
                        result = result + System.Convert.ToString('ؤ');
                        break;
                    case 'C':
                        result = result + System.Convert.ToString('ژ');
                        break;
                    //case '':
                    //    str_new = str_new + System.Convert.ToString('ژ');
                    //   break;
                    case 'B':
                        result = result + System.Convert.ToString('إ');
                        break;
                    case 'N':
                        result = result + System.Convert.ToString('أ');
                        break;
                    case '`':
                        result = result + System.Convert.ToString('پ');
                        break;


                    default:
                        result = result + System.Convert.ToChar(str.Substring(i, 1));
                        break;
                }

            }

            return result;
        }
    }
}
