using System;
using System.Collections.Generic;

namespace PAGE_MODE_II_PARSER
{
    class PAGE_2
    {
        static string whole = "";
        public struct BOUNDS
        {
            public int low;
            public int up;
            public void SetValues(int l, int u) { this.low = l; this.up = u; }
        }
        static public void get_string(string inside_mode_2)
        {
            whole += inside_mode_2;
        }
        static public BOUNDS bounds(string command)
        {
            string l = "", u = "";
            BOUNDS bnd = new BOUNDS();
            int i = 0;
            while (command[i] != '-')
            {
                l += command[i];
                i++;
            }
            i++;
            while (i < command.Length)
            {
                u += command[i];
                i++;
            }
            bnd.SetValues(Int32.Parse(l), Int32.Parse(u));
            return bnd;
        }
        static public void Fill_Page_Number_List(int[] List)
        {
            int i = 0;
            string command = "";
            int size = whole.Length;
            while (i < size)
            {
                if (whole[i] == 'n' && whole[i + 1] == 'u')
                {
                    i += 4;
                    while (whole[i] != '.')
                    {
                        command += whole[i];
                        i++;
                    }
                    i++;
                    BOUNDS bb = bounds(command);
                    int ll = bb.low;
                    int uu = bb.up;
                    for (int j = ll; j <= uu; j++)
                    {
                        List[j - 1] = 2;
                    }
                    command = "";
                }
                else if (whole[i] == 'r' && whole[i + 1] == 'm')
                {
                    i += 4;
                    while (whole[i] != '.')
                    {
                        command += whole[i];
                        i++;
                    }
                    i++;
                    BOUNDS bb = bounds(command);
                    int ll = bb.low;
                    int uu = bb.up;
                    for (int j = ll; j <= uu; j++)
                    {
                        List[j - 1] = 1;
                    }
                    command = "";
                }
            }
        }
    }
}