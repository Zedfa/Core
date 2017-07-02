var farsi = (function () {
    function farsi() {
    }
    farsi.ConvertStrToEnglishOrPersianSide = function (str) {
        var result = "";
        if (str == null) {
            str = "";
            result = null;
        }
        var alphabet = ['a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'w', 'x', 'y', 'z', ' '];
        for (var i = 0; i < str.length; i++) {
            str.replace(/ریال/g, "ق");
            switch (str.substring(i, i + 1)) {
                case ' ':
                    result = result + ' ';
                    break;
                case ':':
                    result = result + ':';
                    break;
                case '"':
                    result = result + '"';
                    break;
                case '?':
                    result = result + '؟';
                    break;
                case '؟':
                    result = result + '?';
                    break;
                case 'ض':
                    result = result + 'q';
                    break;
                case 'ص':
                    result = result + 'w';
                    break;
                case 'ث':
                    result = result + 'e';
                    break;
                case 'ق':
                    result = result + 'r';
                    break;
                case 'ف':
                    result = result + 't';
                    break;
                case 'غ':
                    result = result + 'y';
                    break;
                case 'ع':
                    result = result + 'u';
                    break;
                case 'ه':
                    result = result + 'i';
                    break;
                case 'خ':
                    result = result + 'o';
                    break;
                case 'ح':
                    result = result + 'p';
                    break;
                case 'ج':
                    result = result + '[';
                    break;
                case 'چ':
                    result = result + ']';
                    break;
                case 'ش':
                    result = result + 'a';
                    break;
                case 'س':
                    result = result + 's';
                    break;
                case 'ی':
                    result = result + 'd';
                    break;
                case 'ب':
                    result = result + 'f';
                    break;
                case 'ل':
                    result = result + 'g';
                    break;
                case 'ا':
                    result = result + 'h';
                    break;
                case 'ت':
                    result = result + 'j';
                    break;
                case 'ن':
                    result = result + 'k';
                    break;
                case 'م':
                    result = result + 'l';
                    break;
                case 'ک':
                    result = result + ';';
                    break;
                case 'گ':
                    result = result + '\'';
                    break;
                case 'ظ':
                    result = result + 'z';
                    break;
                case 'ط':
                    result = result + 'x';
                    break;
                case 'ز':
                    result = result + 'c';
                    break;
                case 'ر':
                    result = result + 'v';
                    break;
                case 'ذ':
                    result = result + 'b';
                    break;
                case 'د':
                    result = result + 'n';
                    break;
                case 'ئ':
                    result = result + 'm';
                    break;
                case 'و':
                    result = result + ',';
                    break;
                case '.':
                    result = result + '.';
                    break;
                case 'ً':
                    result = result + 'Q';
                    break;
                case 'ٌ':
                    result = result + 'W';
                    break;
                case 'ٍ':
                    result = result + 'E';
                    break;
                case '،':
                    result = result + 'T';
                    break;
                case '؛':
                    result = result + 'Y';
                    break;
                case '?':
                    result = result + 'U';
                    break;
                case 'ة':
                    result = result + 'I';
                    break;
                case ']':
                    var t1 = 0;
                    alphabet.forEach(function (c) {
                        if (i + 1 < str.length) {
                            if (c == str.substring(i + 1, i + 2)) {
                                t1 = 1;
                            }
                        }
                    });
                    if (t1 == 1)
                        result = result + 'چ';
                    else
                        result = result + 'O';
                    break;
                case '[':
                    var t2 = 0;
                    alphabet.forEach(function (c) {
                        if (i + 1 < str.length) {
                            if (c == str.substring(i + 1, i + 2)) {
                                t2 = 1;
                            }
                        }
                    });
                    if (t2 == 1)
                        result = result + 'ج';
                    else
                        result = result + 'P';
                    break;
                case '}':
                    result = result + '{';
                    break;
                case '{':
                    result = result + '}';
                    break;
                case 'َ':
                    result = result + 'A';
                    break;
                case 'ُ':
                    result = result + 'S';
                    break;
                case 'ِ':
                    result = result + 'D';
                    break;
                case 'ّ':
                    result = result + 'F';
                    break;
                case '?':
                    result = result + 'G';
                    break;
                case 'آ':
                    result = result + 'H';
                    break;
                case 'ـ':
                    result = result + 'J';
                    break;
                case '»':
                    result = result + 'K';
                    break;
                case '«':
                    result = result + 'L';
                    break;
                case 'ؤ':
                    result = result + 'Z';
                    break;
                case '‍':
                    result = result + 'X';
                    break;
                case 'ی':
                    result = result + 'V';
                    break;
                case 'إ':
                    result = result + 'B';
                    break;
                case 'أ':
                    result = result + 'N';
                    break;
                case 'ء':
                    result = result + 'M';
                    break;
                case '>':
                    result = result + '<';
                    break;
                case '<':
                    result = result + '>';
                    break;
                case 'پ':
                    result = result + '`';
                    break;
                case 'ژ':
                    result = result + 'C';
                    break;
                case 'q':
                    result = result + 'ض';
                    break;
                case 'w':
                    result = result + 'ص';
                    break;
                case 'e':
                    result = result + 'ث';
                    break;
                case 'r':
                    result = result + 'ق';
                    break;
                case 't':
                    result = result + 'ف';
                    break;
                case 'y':
                    result = result + 'غ';
                    break;
                case 'u':
                    result = result + 'ع';
                    break;
                case 'i':
                    result = result + 'ه';
                    break;
                case 'o':
                    result = result + 'خ';
                    break;
                case 'p':
                    result = result + 'ح';
                    break;
                case 'a':
                    result = result + 'ش';
                    break;
                case 's':
                    result = result + 'س';
                    break;
                case 'd':
                    result = result + 'ی';
                    break;
                case 'f':
                    result = result + 'ب';
                    break;
                case 'g':
                    result = result + 'ل';
                    break;
                case 'h':
                    result = result + 'ا';
                    break;
                case 'j':
                    result = result + 'ت';
                    break;
                case 'k':
                    result = result + 'ن';
                    break;
                case 'l':
                    result = result + 'م';
                    break;
                case ';':
                    result = result + 'ک';
                    break;
                case '\'':
                    result = result + 'گ';
                    break;
                case 'z':
                    result = result + 'ظ';
                    break;
                case 'x':
                    result = result + 'ط';
                    break;
                case 'c':
                    result = result + 'ز';
                    break;
                case 'v':
                    result = result + 'ر';
                    break;
                case 'b':
                    result = result + 'ذ';
                    break;
                case 'n':
                    result = result + 'د';
                    break;
                case 'm':
                    result = result + 'ئ';
                    break;
                case ',':
                    result = result + 'و';
                    break;
                case 'Q':
                    result = result + 'ً';
                    break;
                case 'W':
                    result = result + 'ٌ';
                    break;
                case 'E':
                    result = result + 'ٍ';
                    break;
                case 'R':
                    result = result + "ریال";
                    break;
                case 'T':
                    result = result + '،';
                    break;
                case 'Y':
                    result = result + '؛';
                    break;
                case 'A':
                    result = result + 'َ';
                    break;
                case 'S':
                    result = result + 'ُ';
                    break;
                case 'D':
                    result = result + 'ِ';
                    break;
                case 'M':
                    result = result + 'ء';
                    break;
                case 'F':
                    result = result + 'ّ';
                    break;
                case 'H':
                    result = result + 'آ';
                    break;
                case 'J':
                    result = result + 'ـ';
                    break;
                case 'K':
                    result = result + '»';
                    break;
                case 'L':
                    result = result + '«';
                    break;
                case 'Z':
                    result = result + 'ؤ';
                    break;
                case 'C':
                    result = result + 'ژ';
                    break;
                case 'B':
                    result = result + 'إ';
                    break;
                case 'N':
                    result = result + 'أ';
                    break;
                case '`':
                    result = result + 'پ';
                    break;
                default:
                    result = result + str.substring(i, i + 1);
                    break;
            }
        }
        return result;
    };
    return farsi;
}());
