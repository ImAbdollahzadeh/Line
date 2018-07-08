using System;
using PdfSharp.Pdf;
using PdfSharp.Drawing;

namespace PDF_Maker
{
    public class _Fraction_detail
    {
        public string up, down, symbol;
        public int symbol_size;
    }
    public class FRC_UP_DOWN
    {
        public string uup, ddown;
    }
    public class Fraction
    {
        public _Fraction_detail fraction(string up, string down)
        {
            int up_sz = up.Length;
            int down_sz = down.Length;
            int symbol_size = up_sz >= down_sz ? up_sz : down_sz;
            string TTmp = up_sz >= down_sz ? up : down;
            int check = 0;
            for (int i = 0; i < symbol_size; i++)
            {
                if (TTmp[i] == ' ')
                {
                    if (check == 1)
                    {
                        symbol_size--;
                        check = 0;
                    }
                    else check++;
                }
            }
            _Fraction_detail frc = new _Fraction_detail();
            frc.up = up;
            frc.down = down;
            string symbol = "";
            for (int i = 0; i < (int)((double)(12.00/18.00) * (double)symbol_size); i++)
                symbol += "_";
            frc.symbol = symbol;
            frc.symbol_size = symbol_size;
            return frc;
        }
        public FRC_UP_DOWN sort_fraction(_Fraction_detail frc)
        {
            int up_space = frc.symbol_size - frc.up.Length;
            int down_space = frc.symbol_size - frc.down.Length;
            if (up_space > down_space)
            {
                string tmp = frc.up;
                frc.up = "";
                for (int i = 0; i < up_space / 2; i++)
                    frc.up += " ";
                frc.up += tmp;
                for (int i = 0; i < up_space / 2; i++)
                    frc.up += " ";
            }
            else if (up_space <= down_space)
            {
                string tmp = frc.down;
                frc.down = "";
                for (int i = 0; i < down_space / 2; i++)
                    frc.down += " ";
                frc.down += tmp;
                for (int i = 0; i < down_space / 2; i++)
                    frc.down += " ";
            }
            FRC_UP_DOWN fud = new FRC_UP_DOWN();
            fud.uup = frc.up;
            fud.ddown = frc.down;
            return fud;
        }
    }
}