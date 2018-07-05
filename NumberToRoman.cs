using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAGE_MODE_II_NUMBERS
{
    class RomanNumbers
    {
        public static string convert(int number)
        {
            if (number == 1) return "i";
            if (number == 2) return "ii";
            if (number == 3) return "iii";
            if (number == 4) return "iv";
            if (number == 5) return "v";
            if (number == 6) return "v";
            if (number == 7) return "vii";
            if (number == 8) return "viii";
            if (number == 9) return "ix";
            if (number == 10) return "x";

            if (number == 11) return "x" + "i";
            if (number == 12) return "x" + "ii";
            if (number == 13) return "x" + "iii";
            if (number == 14) return "x" + "iv";
            if (number == 15) return "x" + "v";
            if (number == 16) return "x" + "v";
            if (number == 17) return "x" + "vii";
            if (number == 18) return "x" + "viii";
            if (number == 19) return "x" + "ix";
            if (number == 20) return "x" + "x";

            if (number == 21) return "xx" + "i";
            if (number == 22) return "xx" + "ii";
            if (number == 23) return "xx" + "iii";
            if (number == 24) return "xx" + "iv";
            if (number == 25) return "xx" + "v";
            if (number == 26) return "xx" + "v";
            if (number == 27) return "xx" + "vii";
            if (number == 28) return "xx" + "viii";
            if (number == 29) return "xx" + "ix";
            if (number == 30) return "xx" + "x";

            if (number == 31) return "xxx" + "i";
            if (number == 32) return "xxx" + "ii";
            if (number == 33) return "xxx" + "iii";
            if (number == 34) return "xxx" + "iv";
            if (number == 35) return "xxx" + "v";
            if (number == 36) return "xxx" + "v";
            if (number == 37) return "xxx" + "vii";
            if (number == 38) return "xxx" + "viii";
            if (number == 39) return "xxx" + "ix";
            if (number == 40) return "xl";

            if (number == 41) return "xl" + "i";
            if (number == 42) return "xl" + "ii";
            if (number == 43) return "xl" + "iii";
            if (number == 44) return "xl" + "iv";
            if (number == 45) return "xl" + "v";
            if (number == 46) return "xl" + "v";
            if (number == 47) return "xl" + "vii";
            if (number == 48) return "xl" + "viii";
            if (number == 49) return "xl" + "ix";
            if (number == 50) return "l";

            if (number == 51) return "l" + "i";
            if (number == 52) return "l" + "ii";
            if (number == 53) return "l" + "iii";
            if (number == 54) return "l" + "iv";
            if (number == 55) return "l" + "v";
            if (number == 56) return "l" + "v";
            if (number == 57) return "l" + "vii";
            if (number == 58) return "l" + "viii";
            if (number == 59) return "l" + "ix";
            if (number == 60) return "l" + "x";

            if (number == 61) return "lx" + "i";
            if (number == 62) return "lx" + "ii";
            if (number == 63) return "lx" + "iii";
            if (number == 64) return "lx" + "iv";
            if (number == 65) return "lx" + "v";
            if (number == 66) return "lx" + "v";
            if (number == 67) return "lx" + "vii";
            if (number == 68) return "lx" + "viii";
            if (number == 69) return "lx" + "ix";
            if (number == 70) return "lx" + "x";

            if (number == 71) return "lxx" + "i";
            if (number == 72) return "lxx" + "ii";
            if (number == 73) return "lxx" + "iii";
            if (number == 74) return "lxx" + "iv";
            if (number == 75) return "lxx" + "v";
            if (number == 76) return "lxx" + "v";
            if (number == 77) return "lxx" + "vii";
            if (number == 78) return "lxx" + "viii";
            if (number == 79) return "lxx" + "ix";
            if (number == 80) return "lxx" + "x";

            if (number == 81) return "lxxx" + "i";
            if (number == 82) return "lxxx" + "ii";
            if (number == 83) return "lxxx" + "iii";
            if (number == 84) return "lxxx" + "iv";
            if (number == 85) return "lxxx" + "v";
            if (number == 86) return "lxxx" + "v";
            if (number == 87) return "lxxx" + "vii";
            if (number == 88) return "lxxx" + "viii";
            if (number == 89) return "lxxx" + "ix";
            if (number == 90) return "xc";

            if (number == 91) return "xc" + "i";
            if (number == 92) return "xc" + "ii";
            if (number == 93) return "xc" + "iii";
            if (number == 94) return "xc" + "iv";
            if (number == 95) return "xc" + "v";
            if (number == 96) return "xc" + "v";
            if (number == 97) return "xc" + "vii";
            if (number == 98) return "xc" + "viii";
            if (number == 99) return "xc" + "ix";
            if (number == 100) return "c";

            else return "";
        }
    }
}