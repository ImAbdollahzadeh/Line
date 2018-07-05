using System;
using System.Diagnostics;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using System.Collections.Generic;

namespace ID_MACROS
{
    class IDS
    {
        public enum IDENTITY
        {
            TITLE_ID = 0x0AA,
            BODY_ID = 0x0BB,
            BLANKPAGE_ID = 0x0CC,
            PAGENUMBER_ID = 0x0DD,
            IMAGE_ID = 0x0EE,
            SUBTITLE_ID = 0x0FF,
            FORMULA_ID = 0x1AA,
            FRACTION_ID = 0x1BB
        }
        public enum TEXT_IDS
        {
            TITLTE = 1001,
            BODY = 1002,
            SUBSCRIPT = 1003,
            SUPERSCRIPT = 1004,
            BLANKPAGE = 1005,
            PAGENUMBER = 1006,
            IMAGE = 1007,
            SUBTITLE = 1008,
            FORMULA = 1009,
            FRACTION = 1010
        }
        public enum PAGE_MODE
        {
            mode1 = 0xF1,
            mode2 = 0xF2
        }
    }
}

namespace Global_Formula_Counter
{
    class Counter
    {
        static public int cntr = 0;
    }
}

namespace PDF_MAKER
{
    struct Fraction_Detail
    {
        public string down;
        public string up;
        public string symbol;
        public int symbol_length;
    }
    struct Integration_Detail
    {
        public string down;
        public string up;
        public string integrand;
    }
    struct SigmaSummation_Detail
    {
        public string down;
        public string up;
        public string summation_body;
    }
    struct Formula_Detail
    {
        public string hierarchy;
        public string[] body;
        public Integration_Detail[] int_src;
        public SigmaSummation_Detail[] sum_src;
    }
    struct IMAGE_Detail
    {
        public string _image_addresses;
        public int _width;
    }
    class PDF_MAKER
    {
        private static Document gl_doc;
        private static Paragraph gl_prg;

        private static bool global_pageNumber_allowance_mode_i = false;
        private static bool global_pageNumber_allowance_mode_ii = false;

        static List<string> title_strings = new List<string>();
        static List<string> subtitle_strings = new List<string>();
        static List<string> body_strings = new List<string>();
        static List<string> sub_strings = new List<string>();
        static List<string> sup_strings = new List<string>();
        static List<string> formula_strings = new List<string>();
        static List<string> image_addresses = new List<string>();
        static List<int> image_widths = new List<int>();

        static List<int> IDS = new List<int>();

        static int COUNTER_GLOBAL = 0;
        static public int[] Page_Number_List = new int[1024];
        static string inside_mode_2_args = "";
        public static Integration_Detail[] integration_sources = new Integration_Detail[1024];
        public static SigmaSummation_Detail[] sigma_sources = new SigmaSummation_Detail[1024];
        public static Formula_Detail[] formula_sources = new Formula_Detail[1024];
        public static Fraction_Detail[] fraction_sources = new Fraction_Detail[1024];

        static public int integral_count = 0;
        static public int sigma_count = 0;
        static public int formula_count = 0;
        static public int fraction_count = 0;

        static public System.Drawing.Bitmap img;

        private static List<string> List_string_handling(List<string> list, string str, ID_MACROS.IDS.IDENTITY id)
        {
            if (id == ID_MACROS.IDS.IDENTITY.IMAGE_ID)
            {
                IDS.Add((int)ID_MACROS.IDS.TEXT_IDS.IMAGE);
                list.Add(str);
                COUNTER_GLOBAL++;
                return list;
            }
            if (id == ID_MACROS.IDS.IDENTITY.BLANKPAGE_ID)
            {
                IDS.Add((int)ID_MACROS.IDS.TEXT_IDS.BLANKPAGE);
                COUNTER_GLOBAL++;
                return null;
            }
            if (id == ID_MACROS.IDS.IDENTITY.PAGENUMBER_ID)
            {
                return null;
            }
            if (id == ID_MACROS.IDS.IDENTITY.FRACTION_ID)
            {
                IDS.Add((int)ID_MACROS.IDS.TEXT_IDS.FRACTION);
                COUNTER_GLOBAL++;
                return null;
            }
            int size = str.Length;
            string tmp = "";
            int i = 0;
            while (i < size)
            {
                if (str[i] == '\r' && str[i + 1] == '\n' && str[i + 2] == '\r' && str[i + 3] == '\n')
                {
                    tmp += str[i];
                    tmp += str[i + 1];
                    i += 4;
                    list.Add(tmp);
                    if (id == ID_MACROS.IDS.IDENTITY.TITLE_ID)
                    {
                        IDS.Add((int)ID_MACROS.IDS.TEXT_IDS.TITLTE);
                        COUNTER_GLOBAL++;
                    }
                    else if (id == ID_MACROS.IDS.IDENTITY.BODY_ID)
                    {
                        IDS.Add((int)ID_MACROS.IDS.TEXT_IDS.BODY);
                        COUNTER_GLOBAL++;
                    }
                    else if (id == ID_MACROS.IDS.IDENTITY.SUBTITLE_ID)
                    {
                        IDS.Add((int)ID_MACROS.IDS.TEXT_IDS.SUBTITLE);
                        COUNTER_GLOBAL++;
                    }
                    tmp = "";
                }
                else if (str[i] == '\r' && str[i + 1] == '\n' && str[i + 2] != '\r' && str[i + 3] != '\n')
                {
                    tmp += ' ';
                    i += 2;
                }
                else
                {
                    if (str[i] != '(' || str[i + 1] != 's' || str[i + 2] != 'u' || str[i + 3] != 'b')
                    {
                        if (str[i] != '(' || str[i + 1] != 's' || str[i + 2] != 'u' || str[i + 3] != 'p')
                        {
                            tmp += str[i];
                            i++;
                        }
                        else
                        {
                            list.Add(tmp);
                            if (id == ID_MACROS.IDS.IDENTITY.TITLE_ID) IDS.Add((int)ID_MACROS.IDS.TEXT_IDS.TITLTE);
                            else if (id == ID_MACROS.IDS.IDENTITY.BODY_ID) IDS.Add((int)ID_MACROS.IDS.TEXT_IDS.BODY);
                            else if (id == ID_MACROS.IDS.IDENTITY.SUBTITLE_ID) IDS.Add((int)ID_MACROS.IDS.TEXT_IDS.SUBTITLE);
                            COUNTER_GLOBAL++;
                            i += 6;
                            tmp = "";
                            while (str[i] != ')')
                            {
                                tmp += str[i];
                                i++;
                            }
                            sup_strings.Add(tmp);
                            IDS.Add((int)ID_MACROS.IDS.TEXT_IDS.SUPERSCRIPT);
                            COUNTER_GLOBAL++;
                            i++;
                            tmp = "";
                        }
                    }
                    else
                    {
                        list.Add(tmp);
                        if (id == ID_MACROS.IDS.IDENTITY.TITLE_ID) IDS.Add((int)ID_MACROS.IDS.TEXT_IDS.TITLTE);
                        else if (id == ID_MACROS.IDS.IDENTITY.BODY_ID) IDS.Add((int)ID_MACROS.IDS.TEXT_IDS.BODY);
                        else if (id == ID_MACROS.IDS.IDENTITY.SUBTITLE_ID) IDS.Add((int)ID_MACROS.IDS.TEXT_IDS.SUBTITLE);
                        COUNTER_GLOBAL++;
                        i += 6;
                        tmp = "";
                        while (str[i] != ')')
                        {
                            tmp += str[i];
                            i++;
                        }
                        sub_strings.Add(tmp);
                        IDS.Add((int)ID_MACROS.IDS.TEXT_IDS.SUBSCRIPT);
                        COUNTER_GLOBAL++;
                        i++;
                        tmp = "";
                    }
                }
            }
            list.Add(tmp);
            if (id == ID_MACROS.IDS.IDENTITY.TITLE_ID)
            {
                IDS.Add((int)ID_MACROS.IDS.TEXT_IDS.TITLTE);
                COUNTER_GLOBAL++;
            }
            else if (id == ID_MACROS.IDS.IDENTITY.BODY_ID)
            {
                IDS.Add((int)ID_MACROS.IDS.TEXT_IDS.BODY);
                COUNTER_GLOBAL++;
            }
            else if (id == ID_MACROS.IDS.IDENTITY.SUBTITLE_ID)
            {
                IDS.Add((int)ID_MACROS.IDS.TEXT_IDS.SUBTITLE);
                COUNTER_GLOBAL++;
            }
            else if (id == ID_MACROS.IDS.IDENTITY.FORMULA_ID)
            {
                IDS.Add((int)ID_MACROS.IDS.TEXT_IDS.FORMULA);
                COUNTER_GLOBAL++;
            }
            return list;
        }
        private static int gl_Lines = 0;
        public static string Unicode_list(string str)
        {
            if (str == "00") return "α";
            else if (str == "01") return "β";
            else if (str == "02") return "γ";
            else if (str == "03") return "δ";
            else if (str == "04") return "γ";
            else if (str == "05") return "ε";
            else if (str == "06") return "ζ";
            else if (str == "07") return "η";
            else if (str == "08") return "θ";
            else if (str == "09") return "ι";
            else if (str == "10") return "κ";
            else if (str == "11") return "λ";
            else if (str == "12") return "μ";
            else if (str == "13") return "ν";
            else if (str == "14") return "ξ";
            else if (str == "15") return "ο";
            else if (str == "16") return "π";
            else if (str == "17") return "ρ";
            else if (str == "18") return "ς";
            else if (str == "19") return "σ";
            else if (str == "20") return "τ";
            else if (str == "21") return "υ";
            else if (str == "22") return "φ";
            else if (str == "23") return "χ";
            else if (str == "24") return "ψ";
            else if (str == "25") return "ω";
            else if (str == "26") return "ϊ";
            else if (str == "27") return "ϋ";
            else if (str == "28") return "ό";
            else if (str == "29") return "ύ";
            else if (str == "30") return "ώ";
            else if (str == "31") return "ϐ";
            else if (str == "32") return "ϑ";
            else if (str == "33") return "Α";
            else if (str == "34") return "Β";
            else if (str == "35") return "Γ";
            else if (str == "36") return "Δ";
            else if (str == "37") return "Ε";
            else if (str == "38") return "Ζ";
            else if (str == "39") return "Η";
            else if (str == "40") return "Θ";
            else if (str == "41") return "Ι";
            else if (str == "42") return "Κ";
            else if (str == "43") return "Λ";
            else if (str == "44") return "Μ";
            else if (str == "45") return "Ν";
            else if (str == "46") return "Ξ";
            else if (str == "47") return "Ο";
            else if (str == "48") return "Π";
            else if (str == "49") return "Ρ";
            else if (str == "50") return "Σ";
            else if (str == "51") return "Τ";
            else if (str == "52") return "Υ";
            else if (str == "53") return "Φ";
            else if (str == "54") return "Χ";
            else if (str == "55") return "Ψ";
            else if (str == "56") return "Ω";
            else if (str == "57") return "Ϊ";
            else if (str == "58") return "Ϋ";
            else if (str == "59") return "ά";
            else if (str == "60") return "έ";
            else if (str == "61") return "ή";
            else if (str == "62") return "ί";
            else if (str == "63") return "ΰ";
            else if (str == "64") return "∞";
            else if (str == "65") return "•";
            else if (str == "66") return "∂";
            else return " ";
        }
        private static string GetOutputAddress(string input)
        {
            int i = 0;
            string ret = "";
            while (input[i] != '.')
            {
                ret += input[i];
                i++;
            }
            return ret += ".pdf";
        }
        private static string Multiply(string input, int n)
        {
            string ret = "";
            for (int i = 0; i < n; i++)
            {
                ret += input;
            }
            return ret;
        }
        private static string BluntBegin(string input)
        {
            int sz = input.Length;
            string ret = "";
            for (int i = 1; i < sz; i++)
            {
                ret += input[i];
            }
            return ret;
        }
        private static string GetCopyFile(string input)
        {
            int i = 0;
            string ret = "";
            while (input[i] != '.')
            {
                ret += input[i];
                i++;
            }
            return ret += "_.pdf";
        }
        private static int GetLines(string str)
        {
            int i = 0, res = 0, j = 0;
            while (j < str.Length)
            {
                if (str[i] == '[' && str[i + 1] == ']')
                {
                    res++;
                    i++;
                }
                else i++;
                j++;
            }
            return res;
        }
        static void Main(string[] args)
        {
            int number_of_cmd_args = args.Length;
            string FilePath = "";
            string OutPath = "";
            string OutPath_cpy = "";
            if (number_of_cmd_args != 0) FilePath = args[0];
            if (number_of_cmd_args == 0) FilePath = "SOURCE.line";
            Console.WriteLine();
            Console.WriteLine("line file: " + FilePath + "\n");
            Console.WriteLine("-------------------------------------------------------------------------------");
            System.IO.FileInfo fInfo = new System.IO.FileInfo(FilePath);
            if (!fInfo.Exists)
            {
                Console.WriteLine("Error: The provided file path does not exist\n\n\nPress Enter to continue");
                Console.ReadLine();
                return;
            }
            OutPath += GetOutputAddress(FilePath);
            OutPath_cpy += GetCopyFile(OutPath);
            string text = System.IO.File.ReadAllText(FilePath);
            int
                global_countetr = 0,
                count = 0,
                line = 0,
                i = 0;
            gl_Lines = GetLines(text);
            int title_list_count = 0;
            int body_list_count = 0;
            int subtitlestrings_list_count = 0;
            while (global_countetr < gl_Lines)
            {
                string target = "";
                while (text[line + count] != '[' || text[line + count + 1] != ']') count++;
                line += count;
                if (
                    text[line - count + 0] == '[' &&
                    text[line - count + 1] == 't' &&
                    text[line - count + 2] == 'i' &&
                    text[line - count + 3] == 't' &&
                    text[line - count + 4] == 'l' &&
                    text[line - count + 5] == 'e' &&
                    text[line - count + 6] == '_' &&
                    text[line - count + 7] == '_' &&
                    text[line - count + 8] == ']'
                )
                {
                    i += 9;
                    while (i < line)
                    {
                        if (text[i] != '&' && text[i + 1] != '<')
                        {
                            target += text[i];
                            i++;
                        }
                        else
                        {
                            string tmp = "";
                            tmp += text[i + 2];
                            tmp += text[i + 3];
                            target += Unicode_list(tmp);
                            i += 6;
                        }
                    }
                    List_string_handling(title_strings, target, ID_MACROS.IDS.IDENTITY.TITLE_ID);
                    int tot_sz = title_strings.Count;
                    line += 4;
                    count = 0;
                    i += 4;
                    title_list_count += tot_sz;
                }
                else if (
                    text[line - count + 0] == '[' &&
                    text[line - count + 1] == 'b' &&
                    text[line - count + 2] == 'o' &&
                    text[line - count + 3] == 'd' &&
                    text[line - count + 4] == 'y' &&
                    text[line - count + 5] == '_' &&
                    text[line - count + 6] == '_' &&
                    text[line - count + 7] == '_' &&
                    text[line - count + 8] == ']'
                )
                {
                    i += 9;
                    while (i < line)
                    {
                        if (text[i] != '&' && text[i + 1] != '<')
                        {
                            target += text[i];
                            i++;
                        }
                        else
                        {
                            string tmp = "";
                            tmp += text[i + 2];
                            tmp += text[i + 3];
                            target += Unicode_list(tmp);
                            i += 6;
                        }
                    }
                    List_string_handling(body_strings, target, ID_MACROS.IDS.IDENTITY.BODY_ID);
                    int tot_sz = body_strings.Count;
                    line += 4;
                    count = 0;
                    i += 4;
                    body_list_count += tot_sz;
                }
                else if (
                    text[line - count + 0] == '[' &&
                    text[line - count + 1] == 's' &&
                    text[line - count + 2] == 'u' &&
                    text[line - count + 3] == 'b' &&
                    text[line - count + 4] == 't' &&
                    text[line - count + 5] == 'i' &&
                    text[line - count + 6] == 't' &&
                    text[line - count + 7] == '_' &&
                    text[line - count + 8] == ']'
                )
                {
                    i += 9;
                    while (i < line)
                    {
                        if (text[i] != '&' && text[i + 1] != '<')
                        {
                            target += text[i];
                            i++;
                        }
                        else
                        {
                            string tmp = "";
                            tmp += text[i + 2];
                            tmp += text[i + 3];
                            target += Unicode_list(tmp);
                            i += 6;
                        }
                    }
                    List_string_handling(subtitle_strings, target, ID_MACROS.IDS.IDENTITY.SUBTITLE_ID);
                    int tot_sz = subtitle_strings.Count;
                    line += 4;
                    count = 0;
                    i += 4;
                    subtitlestrings_list_count += tot_sz;
                }
                //else if (
                //    text[line - count + 0] == '[' &&
                //    text[line - count + 1] == 'f' &&
                //    text[line - count + 2] == 'r' &&
                //    text[line - count + 3] == 'a' &&
                //    text[line - count + 4] == 'c' &&
                //    text[line - count + 5] == 't' &&
                //    text[line - count + 6] == '_' &&
                //    text[line - count + 7] == '_' &&
                //    text[line - count + 8] == ']'
                //)
                //{
                //    i += 9;
                //    string tmp_nom = "", tmp_denom = "";
                //    while (i < line)
                //    {
                //        while (text[i] != '[' && text[i + 1] != ']')
                //        {
                //            if (text[i] == 'n' && text[i + 1] == 'o' && text[i + 2] == 'm' && text[i + 3] == ':')
                //            {
                //                i += 4;
                //                while (text[i] != ',')
                //                {
                //                    if (text[i] != '&' && text[i + 1] != '<')
                //                    {
                //                        tmp_nom += text[i];
                //                        i++;
                //                    }
                //                    else
                //                    {
                //                        string tmp = "";
                //                        tmp += text[i + 2];
                //                        tmp += text[i + 3];
                //                        tmp_nom += Unicode_list(tmp);
                //                        i += 6;
                //                    }
                //                }
                //                i++;
                //            }
                //            else if (text[i] == 'd' && text[i + 1] == 'e' && text[i + 2] == 'n' && text[i + 3] == 'o' && text[i + 4] == 'm' && text[i + 5] == ':')
                //            {
                //                i += 6;
                //                while (text[i] != '.')
                //                {
                //                    if (text[i] != '&' && text[i + 1] != '<')
                //                    {
                //                        tmp_denom += text[i];
                //                        i++;
                //                    }
                //                    else
                //                    {
                //                        string tmp = "";
                //                        tmp += text[i + 2];
                //                        tmp += text[i + 3];
                //                        tmp_denom += Unicode_list(tmp);
                //                        i += 6;
                //                    }
                //                }
                //                i++;
                //            }
                //        }
                //    }
                //    fraction_sources[fraction_count].up = tmp_nom;
                //    fraction_sources[fraction_count].down = tmp_denom;
                //    fraction_sources[fraction_count].symbol_length = (tmp_nom.Length >= tmp_denom.Length) ? tmp_nom.Length + 2 : tmp_denom.Length + 2;
                //    fraction_sources[fraction_count].symbol = PDF_GraphicObject.StringManip.Duplicate("_", fraction_sources[fraction_count].symbol_length);
                //    line += 4;
                //    count = 0;
                //    i += 4;
                //    List_string_handling(null, "", ID_MACROS.IDS.IDENTITY.FRACTION_ID);
                //    fraction_count++;
                //}
                else if (
                    text[line - count + 0] == '[' &&
                    text[line - count + 1] == 'b' &&
                    text[line - count + 2] == 'l' &&
                    text[line - count + 3] == 'n' &&
                    text[line - count + 4] == 'k' &&
                    text[line - count + 5] == 'p' &&
                    text[line - count + 6] == 'g' &&
                    text[line - count + 7] == '_' &&
                    text[line - count + 8] == ']'
                )
                {
                    i += 9;
                    List_string_handling(null, "", ID_MACROS.IDS.IDENTITY.BLANKPAGE_ID);
                    line += 4;
                    count = 0;
                    i += 4;
                }
                else if (
                    text[line - count + 0] == '[' &&
                    text[line - count + 1] == 'p' &&
                    text[line - count + 2] == 'g' &&
                    text[line - count + 3] == 'N' &&
                    text[line - count + 4] == 'u' &&
                    text[line - count + 5] == 'm' &&
                    text[line - count + 6] == '1' &&
                    text[line - count + 7] == '_' &&
                    text[line - count + 8] == ']'
                )
                {
                    i += 9;
                    List_string_handling(null, "", ID_MACROS.IDS.IDENTITY.PAGENUMBER_ID);
                    line += 4;
                    count = 0;
                    i += 4;
                    BlockPageNumber_Mode_II();
                    AllowPageNumber_Mode_I();
                }
                else if (
                    text[line - count + 0] == '[' &&
                    text[line - count + 1] == 'p' &&
                    text[line - count + 2] == 'g' &&
                    text[line - count + 3] == 'N' &&
                    text[line - count + 4] == 'u' &&
                    text[line - count + 5] == 'm' &&
                    text[line - count + 6] == '2' &&
                    text[line - count + 7] == '_' &&
                    text[line - count + 8] == ']'
                )
                {
                    i += 9;
                    while (i < line)
                    {
                        if (text[i] != '[' && text[i + 1] != ']')
                        {
                            inside_mode_2_args += text[i];
                            i++;
                        }
                        else break;
                    }
                    List_string_handling(null, "", ID_MACROS.IDS.IDENTITY.PAGENUMBER_ID);
                    line += 4;
                    count = 0;
                    i += 4;
                    BlockPageNumber_Mode_I();
                    AllowPageNumber_Mode_II();
                }
                else if (
                    text[line - count + 0] == '[' &&
                    text[line - count + 1] == 'i' &&
                    text[line - count + 2] == 'm' &&
                    text[line - count + 3] == 'a' &&
                    text[line - count + 4] == 'g' &&
                    text[line - count + 5] == 'e' &&
                    text[line - count + 6] == '_' &&
                    text[line - count + 7] == '_' &&
                    text[line - count + 8] == ']'
                )
                {
                    i += 9;
                    string tmp_img_addr = "";
                    string if_widths = "";
                    while (i < line)
                    {
                        if (text[i] != '[' && text[i + 1] != ']')
                        {
                            while (text[i] != ',')
                            {
                                tmp_img_addr += text[i];
                                i++;
                            }
                            i++;
                            while (text[i] != ';')
                            {
                                if_widths += text[i];
                                i++;
                            }
                            i++;
                        }
                        else break;
                    }
                    List_string_handling(image_addresses, tmp_img_addr, ID_MACROS.IDS.IDENTITY.IMAGE_ID);
                    image_widths.Add(Int32.Parse(if_widths));
                    line += 4;
                    count = 0;
                    i += 4;
                }
                else if (
                    text[line - count + 0] == '[' &&
                    text[line - count + 1] == 'G' &&
                    text[line - count + 2] == 'O' &&
                    text[line - count + 3] == 'F' &&
                    text[line - count + 4] == 'o' &&
                    text[line - count + 5] == 'r' &&
                    text[line - count + 6] == 'm' &&
                    text[line - count + 7] == '_' &&
                    text[line - count + 8] == ']')
                {
                    i += 9;
                    string tmp_GObj = "";
                    while (i < line)
                    {
                        if (text[i] != '&' && text[i + 1] != '<')
                        {
                            tmp_GObj += text[i];
                            i++;
                        }
                        else
                        {
                            string Tmp = "";
                            Tmp += text[i + 2];
                            Tmp += text[i + 3];
                            tmp_GObj += Unicode_list(Tmp);
                            i += 6;
                        }
                    }
                    PDF_graphic.GraphicalObject go = new PDF_graphic.GraphicalObject();
                    go.handle_objecvtive_formula(tmp_GObj);
                    List_string_handling(image_addresses, "__internal_ObjectiveFormula" + Global_Formula_Counter.Counter.cntr.ToString() + ".png", ID_MACROS.IDS.IDENTITY.IMAGE_ID);
                    i += 4;
                    line += 4;
                    count = 0;
                    Global_Formula_Counter.Counter.cntr++;
                }
                else if (
                    text[line - count + 0] == '[' &&
                    text[line - count + 1] == 'G' &&
                    text[line - count + 2] == 'O' &&
                    text[line - count + 3] == 'R' &&
                    text[line - count + 4] == 'e' &&
                    text[line - count + 5] == 'g' &&
                    text[line - count + 6] == '_' &&
                    text[line - count + 7] == '_' &&
                    text[line - count + 8] == ']')
                {
                    i += 9;
                    string tmp_symb = "", tmp_user_command = "";
                    while (i < line)
                    {
                        if (text[i] != '[' && text[i + 1] != ']')
                        {
                            while (text[i] != ',')
                            {
                                tmp_symb += text[i];
                                i++;
                            }
                            i++;
                            while (text[i] != '.')
                            {
                                tmp_user_command += text[i];
                                i++;
                            }
                            i++;
                        }
                    }
                    PDF_graphic.GraphicalObject go = new PDF_graphic.GraphicalObject();
                    go.Register_custom_GObject(tmp_user_command, tmp_symb);
                    i += 4;
                    line += 4;
                    count = 0;
                }
                else if (
                    text[line - count + 0] == '[' &&
                    text[line - count + 1] == 'f' &&
                    text[line - count + 2] == 'o' &&
                    text[line - count + 3] == 'r' &&
                    text[line - count + 4] == 'm' &&
                    text[line - count + 5] == 'u' &&
                    text[line - count + 6] == 'l' &&
                    text[line - count + 7] == '_' &&
                    text[line - count + 8] == ']'
                )
                {
                    formula_sources[formula_count].int_src = new Integration_Detail[64];
                    formula_sources[formula_count].sum_src = new SigmaSummation_Detail[64];
                    formula_sources[formula_count].body = new string[64];
                    i += 9;
                    string tmp_down = "", tmp_up = "", tmp_integrand = "", tmp_body = "", tmp_sigma = "";
                    int b_count = 0, in_count = 0, su_count = 0;
                    while (i < line)
                    {
                        while (text[i] != '[' && text[i + 1] != ']')
                        {
                            if (text[i] == 'I' && text[i + 1] == 'N' && text[i + 2] == 'T' && text[i + 3] == '.')
                            {
                                i += 4;
                                while (text[i] != ',')
                                {
                                    if (text[i] != '&' && text[i + 1] != '<')
                                    {
                                        tmp_down += text[i];
                                        i++;
                                    }
                                    else
                                    {
                                        string tmp = "";
                                        tmp += text[i + 2];
                                        tmp += text[i + 3];
                                        tmp_down += Unicode_list(tmp);
                                        i += 6;
                                    }
                                }
                                i++;
                                while (text[i] != ',')
                                {
                                    if (text[i] != '&' && text[i + 1] != '<')
                                    {
                                        tmp_up += text[i];
                                        i++;
                                    }
                                    else
                                    {
                                        string tmp = "";
                                        tmp += text[i + 2];
                                        tmp += text[i + 3];
                                        tmp_up += Unicode_list(tmp);
                                        i += 6;
                                    }
                                }
                                i++;
                                while (text[i] != '.')
                                {
                                    if (text[i] != '&' && text[i + 1] != '<')
                                    {
                                        tmp_integrand += text[i];
                                        i++;
                                    }
                                    else
                                    {
                                        string tmp = "";
                                        tmp += text[i + 2];
                                        tmp += text[i + 3];
                                        tmp_integrand += Unicode_list(tmp);
                                        i += 6;
                                    }
                                }
                                formula_sources[formula_count].hierarchy += "i";
                                formula_sources[formula_count].int_src[in_count].down = tmp_down;
                                formula_sources[formula_count].int_src[in_count].up = tmp_up;
                                formula_sources[formula_count].int_src[in_count].integrand = tmp_integrand;
                                in_count++;
                                i++;
                                tmp_down = "";
                                tmp_up = "";
                                tmp_integrand = "";
                            }
                            if (text[i] == 'S' && text[i + 1] == 'U' && text[i + 2] == 'M' && text[i + 3] == '.')
                            {
                                i += 4;
                                while (text[i] != ',')
                                {
                                    if (text[i] != '&' && text[i + 1] != '<')
                                    {
                                        tmp_down += text[i];
                                        i++;
                                    }
                                    else
                                    {
                                        string tmp = "";
                                        tmp += text[i + 2];
                                        tmp += text[i + 3];
                                        tmp_down += Unicode_list(tmp);
                                        i += 6;
                                    }
                                }
                                i++;
                                while (text[i] != ',')
                                {
                                    if (text[i] != '&' && text[i + 1] != '<')
                                    {
                                        tmp_up += text[i];
                                        i++;
                                    }
                                    else
                                    {
                                        string tmp = "";
                                        tmp += text[i + 2];
                                        tmp += text[i + 3];
                                        tmp_up += Unicode_list(tmp);
                                        i += 6;
                                    }
                                }
                                i++;
                                while (text[i] != '.')
                                {
                                    if (text[i] != '&' && text[i + 1] != '<')
                                    {
                                        tmp_sigma += text[i];
                                        i++;
                                    }
                                    else
                                    {
                                        string tmp = "";
                                        tmp += text[i + 2];
                                        tmp += text[i + 3];
                                        tmp_sigma += Unicode_list(tmp);
                                        i += 6;
                                    }
                                }
                                formula_sources[formula_count].hierarchy += "s";
                                formula_sources[formula_count].sum_src[su_count].down = tmp_down;
                                formula_sources[formula_count].sum_src[su_count].up = tmp_up;
                                formula_sources[formula_count].sum_src[su_count].summation_body = tmp_sigma;
                                su_count++;
                                i++;
                                tmp_down = "";
                                tmp_up = "";
                                tmp_sigma = "";
                            }
                            if (text[i] == 'B' && text[i + 1] == 'D' && text[i + 2] == 'Y' && text[i + 3] == '.')
                            {
                                i += 4;
                                while (text[i] != '.')
                                {
                                    if (text[i] != '&' && text[i + 1] != '<')
                                    {
                                        tmp_body += text[i];
                                        i++;
                                    }
                                    else
                                    {
                                        string tmp = "";
                                        tmp += text[i + 2];
                                        tmp += text[i + 3];
                                        tmp_body += Unicode_list(tmp);
                                        i += 6;
                                    }
                                }
                                formula_sources[formula_count].hierarchy += "b";
                                formula_sources[formula_count].body[b_count] = tmp_body;
                                b_count++;
                                i++;
                                tmp_body = "";
                            }
                        }
                    }
                    line += 4;
                    count = 0;
                    i += 4;
                    List_string_handling(formula_strings, tmp_body, ID_MACROS.IDS.IDENTITY.FORMULA_ID);
                    formula_count++;
                }
                else if (
                    text[line - count + 0] == '[' &&
                    text[line - count + 1] == 'C' &&
                    text[line - count + 2] == 'O' &&
                    text[line - count + 3] == 'M' &&
                    text[line - count + 4] == 'M' &&
                    text[line - count + 5] == 'E' &&
                    text[line - count + 6] == 'N' &&
                    text[line - count + 7] == 'T' &&
                    text[line - count + 8] == ']'
                )
                {
                    i += 9;
                    while (text[i] != '[' && text[i + 1] != ']')
                    {
                        i++;
                    }
                    line += 4;
                    count = 0;
                    i += 4;
                }
                global_countetr++;
            }
            Document document = CreateDocument();
            PdfDocumentRenderer renderer = new PdfDocumentRenderer(true, PdfSharp.Pdf.PdfFontEmbedding.Always);
            renderer.Document = document;
            renderer.RenderDocument();
            renderer.PdfDocument.Save(OutPath);
            System.IO.File.Copy(OutPath, OutPath_cpy, true);
            System.IO.File.Delete(OutPath);
            string filename = "";
            if (global_pageNumber_allowance_mode_i)
            {
                filename = PDF_PAGE_NUMBER.FormattedPageNumbers.makeIt(OutPath, OutPath_cpy, ID_MACROS.IDS.PAGE_MODE.mode1);
            }
            else if (global_pageNumber_allowance_mode_ii)
            {
                PAGE_MODE_II_PARSER.PAGE_2.get_string(inside_mode_2_args);
                PAGE_MODE_II_PARSER.PAGE_2.Fill_Page_Number_List(Page_Number_List);
                filename = PDF_PAGE_NUMBER.FormattedPageNumbers.makeIt(OutPath, OutPath_cpy, ID_MACROS.IDS.PAGE_MODE.mode2);
            }
            System.IO.File.Delete(OutPath_cpy);
            Console.WriteLine();
            Console.WriteLine("output PDF file: " + filename + "\n");
            Console.WriteLine("-------------------------------------------------------------------------------\n");
            Console.WriteLine("*******************************");
            Console.WriteLine("press a key to see the PDF file");
            Console.WriteLine("*******************************");
            for (int I = 0; I < Global_Formula_Counter.Counter.cntr; I++)
            {
                if (System.IO.File.Exists("__internal_ObjectiveFormula" + I.ToString() + ".png"))
                    System.IO.File.Delete("__internal_ObjectiveFormula" + I.ToString() + ".png");
            }

            Console.ReadKey();
            Process.Start(filename);
        }
        public static Document CreateDocument()
        {
            Document document = new Document();
            DefineStyles(document);
            DefineContentSection(document);
            DefineParagraphs(document);
            return document;
        }
        public static void DefineStyles(Document document)
        {
            Style style = document.Styles["Normal"];

            // body font attribs ..
            style.Font.Name = "Times New Roman";
            style.Font.Size = 12;

            // Title font attribs ..
            style = document.Styles["Heading1"];
            style.Font.Name = "Helvetica";
            style.Font.Size = 16;
            style.Font.Bold = true;
            style.Font.Italic = true;
            style.Font.Color = Colors.Black;
            style.ParagraphFormat.PageBreakBefore = false;
            style.ParagraphFormat.SpaceAfter = 30;
        }
        public static void DefineContentSection(Document document)
        {
            Section section = document.AddSection();
            section.PageSetup.OddAndEvenPagesHeaderFooter = true;
            section.PageSetup.StartingNumber = 1;
            if (global_pageNumber_allowance_mode_i)
            {
                HeaderFooter header = section.Footers.Primary;
                Paragraph paragraph = new Paragraph();
                paragraph.Format.Alignment = ParagraphAlignment.Center;
                paragraph.AddPageField();
                section.Footers.Primary.Add(paragraph);
                section.Footers.EvenPage.Add(paragraph.Clone());
            }
            if (global_pageNumber_allowance_mode_ii) { }
        }
        public static void DefineParagraphs(Document document)
        {
            Paragraph paragraph = new Paragraph();
            gl_doc = document;
            gl_prg = paragraph;
            FormattedText formattedText = new FormattedText();

            int tit = 0;
            int subtit = 0;
            int bdy = 0;
            int sub = 0;
            int sup = 0;
            int frm = 0;
            int ImG = 0;
            int frc = 0;
            int FORM_IMG = 0;

            for (int KK = 0; KK < COUNTER_GLOBAL; KK++)
            {
                if (IDS[KK] == (int)ID_MACROS.IDS.TEXT_IDS.TITLTE)
                {
                    if (KK > 0 && IDS[KK - 1] == (int)ID_MACROS.IDS.TEXT_IDS.SUBSCRIPT)
                    {
                        paragraph.AddText(title_strings[tit]);
                    }
                    else if (KK > 0 && IDS[KK - 1] == (int)ID_MACROS.IDS.TEXT_IDS.SUPERSCRIPT)
                    {
                        paragraph.AddText(title_strings[tit]);
                    }
                    else paragraph = document.LastSection.AddParagraph(title_strings[tit], "Heading1");
                    paragraph.Format.Alignment = ParagraphAlignment.Center;
                    if (KK < COUNTER_GLOBAL - 1 && IDS[KK + 1] != (int)ID_MACROS.IDS.TEXT_IDS.SUBSCRIPT)
                    {
                        if (IDS[KK + 1] != (int)ID_MACROS.IDS.TEXT_IDS.SUPERSCRIPT)
                        {
                            paragraph = document.LastSection.AddParagraph();
                        }
                    }
                    tit++;
                }
                else if (IDS[KK] == (int)ID_MACROS.IDS.TEXT_IDS.BODY)
                {
                    paragraph.AddText(body_strings[bdy]);
                    paragraph.Format.Alignment = ParagraphAlignment.Justify;
                    if (KK < COUNTER_GLOBAL - 1)
                    {
                        if (KK < COUNTER_GLOBAL - 1 && IDS[KK + 1] != (int)ID_MACROS.IDS.TEXT_IDS.SUBSCRIPT)
                        {
                            if (IDS[KK + 1] != (int)ID_MACROS.IDS.TEXT_IDS.SUPERSCRIPT)
                            {
                                paragraph = document.LastSection.AddParagraph();
                            }
                        }
                    }
                    bdy++;
                }
                else if (IDS[KK] == (int)ID_MACROS.IDS.TEXT_IDS.SUBTITLE)
                {
                    paragraph.AddText(subtitle_strings[subtit]);
                    paragraph.Format.Alignment = ParagraphAlignment.Left;
                    paragraph.Format.Font.Name = "Helvetica";
                    paragraph.Format.Font.Size = 12;
                    paragraph.Format.Font.Bold = true;
                    paragraph.Format.Font.Italic = true;
                    paragraph.Format.SpaceBefore = 10;
                    paragraph.Format.SpaceAfter = 10;
                    if (KK < COUNTER_GLOBAL - 1)
                    {
                        if (KK < COUNTER_GLOBAL - 1 && IDS[KK + 1] != (int)ID_MACROS.IDS.TEXT_IDS.SUBSCRIPT)
                        {
                            if (IDS[KK + 1] != (int)ID_MACROS.IDS.TEXT_IDS.SUPERSCRIPT)
                            {
                                paragraph = document.LastSection.AddParagraph();
                            }
                        }
                    }
                    subtit++;
                }
                else if (IDS[KK] == (int)ID_MACROS.IDS.TEXT_IDS.IMAGE)
                {
                    var pp = document.LastSection.AddParagraph();
                    pp.Format.Alignment = ParagraphAlignment.Center;
                    MigraDoc.DocumentObjectModel.Shapes.Image __img = pp.AddImage(image_addresses[ImG]);
                    if (System.IO.File.Exists(image_addresses[ImG]) == false)
                    {
                        Console.WriteLine(" *** Error => Image with address: " + image_addresses[ImG] + " does not exist");
                    }
                    __img.LockAspectRatio = true;
                    if (image_addresses[ImG] == "__internal_ObjectiveFormula" + FORM_IMG.ToString() + ".png")
                    {
                        __img.Width = 595;
                        pp.Format.LeftIndent = 210;
                        FORM_IMG++;
                    }
                    else
                    {
                        if (image_widths[ImG] != 0)
                        {
                            __img.Width = image_widths[ImG];
                        }
                        else
                        {
                            __img.Width = 100;
                        }
                    }
                    paragraph = document.LastSection.AddParagraph();
                    ImG++;
                }
                else if (IDS[KK] == (int)ID_MACROS.IDS.TEXT_IDS.SUBSCRIPT)
                {
                    formattedText = paragraph.AddFormattedText(sub_strings[sub]);
                    formattedText.Subscript = true;
                    formattedText.Font.Size = 8;
                    formattedText.Font.Italic = false;
                    sub++;
                }
                else if (IDS[KK] == (int)ID_MACROS.IDS.TEXT_IDS.SUPERSCRIPT)
                {
                    formattedText = paragraph.AddFormattedText(sup_strings[sup]);
                    formattedText.Superscript = true;
                    formattedText.Font.Size = 8;
                    formattedText.Font.Italic = false;
                    sup++;
                }
                else if (IDS[KK] == (int)ID_MACROS.IDS.TEXT_IDS.BLANKPAGE)
                {
                    AddBlankPage();
                    gl_prg = gl_doc.LastSection.AddParagraph();
                    paragraph = gl_prg;
                }
                else if (IDS[KK] == (int)ID_MACROS.IDS.TEXT_IDS.FORMULA)
                {
                    int bb = 0, ii = 0, ss = 0;
                    bool init = true;
                    string codes = formula_sources[frm].hierarchy;
                    int This = codes.Length;

                    //ups
                    for (int n = 0; n < This; n++)
                    {
                        if (codes[n] == 'b')
                        {
                            if (init == true)
                            {
                                formattedText = paragraph.AddFormattedText("\t");
                                init = false;
                            }
                            formattedText = paragraph.AddFormattedText(formula_sources[frm].body[bb]);
                            formattedText.Font.Size = 12;
                            paragraph.Format.Font.Italic = true;
                            formattedText.Font.Color = Colors.White;
                            bb++;
                        }
                        else if (codes[n] == 'i')
                        {
                            if (init == true)
                            {
                                formattedText = paragraph.AddFormattedText("\t");
                                init = false;
                            }
                            formattedText = paragraph.AddFormattedText(formula_sources[frm].int_src[ii].up);
                            formattedText.Font.Size = 6;
                            paragraph.Format.Font.Italic = true;
                            formattedText.Font.Color = Colors.Black;
                            string blunted = formula_sources[frm].int_src[ii].integrand;
                            int golden_size = formula_sources[frm].int_src[ii].up.Length - 5;
                            if (golden_size > 0)
                            {
                                for (int M = 0; M < golden_size / 2 + 1 - ii; M++)
                                {
                                    blunted = BluntBegin(blunted);
                                }
                                formattedText = paragraph.AddFormattedText(blunted);
                                formattedText.Font.Size = 12;
                                paragraph.Format.Font.Italic = true;
                                formattedText.Font.Color = Colors.White;
                            }
                            else
                            {
                                if (formula_sources[frm].int_src[ii].up.Length == 1)
                                {
                                    formattedText = paragraph.AddFormattedText(formula_sources[frm].int_src[ii].integrand + Multiply(".", 3 - ii));
                                    formattedText.Font.Size = 12;
                                    paragraph.Format.Font.Italic = true;
                                    formattedText.Font.Color = Colors.White;
                                }
                                else if (formula_sources[frm].int_src[ii].up.Length == 2)
                                {
                                    formattedText = paragraph.AddFormattedText(formula_sources[frm].int_src[ii].integrand + Multiply(".", 2 - ii));
                                    formattedText.Font.Size = 12;
                                    paragraph.Format.Font.Italic = true;
                                    formattedText.Font.Color = Colors.White;
                                }
                                else if (formula_sources[frm].int_src[ii].up.Length == 3)
                                {
                                    formattedText = paragraph.AddFormattedText(formula_sources[frm].int_src[ii].integrand + Multiply(".", 1 - ii));
                                    formattedText.Font.Size = 12;
                                    paragraph.Format.Font.Italic = true;
                                    formattedText.Font.Color = Colors.White;
                                }
                                else if (formula_sources[frm].int_src[ii].up.Length == 4)
                                {
                                    formattedText = paragraph.AddFormattedText(formula_sources[frm].int_src[ii].integrand);
                                    formattedText.Font.Size = 12;
                                    paragraph.Format.Font.Italic = true;
                                    formattedText.Font.Color = Colors.White;
                                }
                                else if (formula_sources[frm].int_src[ii].up.Length == 5)
                                {
                                    formattedText = paragraph.AddFormattedText(formula_sources[frm].int_src[ii].integrand);
                                    formattedText.Font.Size = 12;
                                    paragraph.Format.Font.Italic = true;
                                    formattedText.Font.Color = Colors.White;
                                }
                            }
                            ii++;
                        }
                        else if (codes[n] == 's')
                        {
                            if (init == true)
                            {
                                formattedText = paragraph.AddFormattedText("\t");
                                init = false;
                            }
                            formattedText = paragraph.AddFormattedText(formula_sources[frm].sum_src[ss].up);
                            formattedText.Font.Size = 6;
                            paragraph.Format.Font.Italic = true;
                            formattedText.Font.Color = Colors.Black;
                            string blunted = formula_sources[frm].sum_src[ss].summation_body;
                            int golden_size = formula_sources[frm].sum_src[ss].up.Length - 5;
                            if (golden_size > 0)
                            {
                                for (int M = 0; M < golden_size / 2 + 1 - ss; M++)
                                {
                                    blunted = BluntBegin(blunted);
                                }
                                formattedText = paragraph.AddFormattedText(blunted);
                                formattedText.Font.Size = 12;
                                paragraph.Format.Font.Italic = true;
                                formattedText.Font.Color = Colors.White;
                            }
                            else
                            {
                                if (formula_sources[frm].sum_src[ss].up.Length == 1)
                                {
                                    formattedText = paragraph.AddFormattedText(formula_sources[frm].sum_src[ss].summation_body + Multiply(".", 3 - ss));
                                    formattedText.Font.Size = 12;
                                    paragraph.Format.Font.Italic = true;
                                    formattedText.Font.Color = Colors.White;
                                }
                                else if (formula_sources[frm].sum_src[ss].up.Length == 2)
                                {
                                    formattedText = paragraph.AddFormattedText(formula_sources[frm].sum_src[ss].summation_body + Multiply(".", 2 - ss));
                                    formattedText.Font.Size = 12;
                                    paragraph.Format.Font.Italic = true;
                                    formattedText.Font.Color = Colors.White;
                                }
                                else if (formula_sources[frm].sum_src[ss].up.Length == 3)
                                {
                                    formattedText = paragraph.AddFormattedText(formula_sources[frm].sum_src[ss].summation_body + Multiply(".", 1 - ss));
                                    formattedText.Font.Size = 12;
                                    paragraph.Format.Font.Italic = true;
                                    formattedText.Font.Color = Colors.White;
                                }
                                else if (formula_sources[frm].sum_src[ss].up.Length == 4)
                                {
                                    formattedText = paragraph.AddFormattedText(formula_sources[frm].sum_src[ss].summation_body);
                                    formattedText.Font.Size = 12;
                                    paragraph.Format.Font.Italic = true;
                                    formattedText.Font.Color = Colors.White;
                                }
                                else if (formula_sources[frm].sum_src[ss].up.Length == 5)
                                {
                                    formattedText = paragraph.AddFormattedText(formula_sources[frm].sum_src[ss].summation_body);
                                    formattedText.Font.Size = 12;
                                    paragraph.Format.Font.Italic = true;
                                    formattedText.Font.Color = Colors.White;
                                }
                            }
                            ss++;
                        }
                    }
                    paragraph = document.LastSection.AddParagraph();
                    bb = 0;
                    ii = 0;
                    ss = 0;
                    init = true;

                    //mains
                    for (int n = 0; n < This; n++)
                    {
                        if (codes[n] == 'b')
                        {
                            if (init == true)
                            {
                                formattedText = paragraph.AddFormattedText("\t");
                                init = false;
                            }
                            formattedText = paragraph.AddFormattedText(formula_sources[frm].body[bb]);
                            formattedText.Font.Size = 12;
                            paragraph.Format.Font.Italic = true;
                            formattedText.Font.Color = Colors.Black;
                            bb++;
                        }
                        else if (codes[n] == 'i')
                        {
                            if (init == true)
                            {
                                formattedText = paragraph.AddFormattedText("\t");
                                init = false;
                            }
                            formattedText = paragraph.AddFormattedText("∫ ");
                            formattedText.Font.Size = 18;
                            paragraph.Format.Font.Italic = true;
                            formattedText = paragraph.AddFormattedText(formula_sources[frm].int_src[ii].integrand);
                            formattedText.Font.Size = 12;
                            paragraph.Format.Font.Italic = true;
                            formattedText.Font.Color = Colors.Black;
                            ii++;
                        }
                        else if (codes[n] == 's')
                        {
                            if (init == true)
                            {
                                formattedText = paragraph.AddFormattedText("\t");
                                init = false;
                            }
                            formattedText = paragraph.AddFormattedText("Σ ");
                            formattedText.Font.Size = 18;
                            paragraph.Format.Font.Italic = true;
                            formattedText = paragraph.AddFormattedText(formula_sources[frm].sum_src[ss].summation_body);
                            formattedText.Font.Size = 12;
                            paragraph.Format.Font.Italic = true;
                            formattedText.Font.Color = Colors.Black;
                            ss++;
                        }
                    }
                    paragraph = document.LastSection.AddParagraph();
                    bb = 0;
                    ii = 0;
                    ss = 0;
                    init = true;

                    //downs
                    for (int n = 0; n < This; n++)
                    {
                        if (codes[n] == 'b')
                        {
                            if (init == true)
                            {
                                formattedText = paragraph.AddFormattedText("\t");
                                init = false;
                            }
                            formattedText = paragraph.AddFormattedText(formula_sources[frm].body[bb]);
                            formattedText.Font.Size = 12;
                            paragraph.Format.Font.Italic = true;
                            formattedText.Font.Color = Colors.White;
                            bb++;
                        }
                        else if (codes[n] == 'i')
                        {
                            if (init == true)
                            {
                                formattedText = paragraph.AddFormattedText("\t");
                                init = false;
                            }
                            formattedText = paragraph.AddFormattedText(formula_sources[frm].int_src[ii].down);
                            formattedText.Font.Size = 6;
                            paragraph.Format.Font.Italic = true;
                            formattedText.Font.Color = Colors.Black;
                            string blunted = formula_sources[frm].int_src[ii].integrand;
                            int golden_size = formula_sources[frm].int_src[ii].down.Length - 5;
                            if (golden_size > 0)
                            {
                                for (int M = 0; M < golden_size / 2 + 1 - ii; M++)
                                {
                                    blunted = BluntBegin(blunted);
                                }
                                formattedText = paragraph.AddFormattedText(blunted);
                                formattedText.Font.Size = 12;
                                paragraph.Format.Font.Italic = true;
                                formattedText.Font.Color = Colors.White;
                            }
                            else
                            {
                                if (formula_sources[frm].int_src[ii].down.Length == 1)
                                {
                                    formattedText = paragraph.AddFormattedText(formula_sources[frm].int_src[ii].integrand + Multiply(".", 3 - ii));
                                    formattedText.Font.Size = 12;
                                    paragraph.Format.Font.Italic = true;
                                    formattedText.Font.Color = Colors.White;
                                }
                                else if (formula_sources[frm].int_src[ii].down.Length == 2)
                                {
                                    formattedText = paragraph.AddFormattedText(formula_sources[frm].int_src[ii].integrand + Multiply(".", 2 - ii));
                                    formattedText.Font.Size = 12;
                                    paragraph.Format.Font.Italic = true;
                                    formattedText.Font.Color = Colors.White;
                                }
                                else if (formula_sources[frm].int_src[ii].down.Length == 3)
                                {
                                    formattedText = paragraph.AddFormattedText(formula_sources[frm].int_src[ii].integrand + Multiply(".", 1 - ii));
                                    formattedText.Font.Size = 12;
                                    paragraph.Format.Font.Italic = true;
                                    formattedText.Font.Color = Colors.White;
                                }
                                else if (formula_sources[frm].int_src[ii].down.Length == 4)
                                {
                                    formattedText = paragraph.AddFormattedText(formula_sources[frm].int_src[ii].integrand);
                                    formattedText.Font.Size = 12;
                                    paragraph.Format.Font.Italic = true;
                                    formattedText.Font.Color = Colors.White;
                                }
                                else if (formula_sources[frm].int_src[ii].down.Length == 5)
                                {
                                    formattedText = paragraph.AddFormattedText(formula_sources[frm].int_src[ii].integrand);
                                    formattedText.Font.Size = 12;
                                    paragraph.Format.Font.Italic = true;
                                    formattedText.Font.Color = Colors.White;
                                }
                            }
                            ii++;
                        }
                        else if (codes[n] == 's')
                        {
                            if (init == true)
                            {
                                formattedText = paragraph.AddFormattedText("\t");
                                init = false;
                            }
                            formattedText = paragraph.AddFormattedText(formula_sources[frm].sum_src[ss].down);
                            formattedText.Font.Size = 6;
                            paragraph.Format.Font.Italic = true;
                            formattedText.Font.Color = Colors.Black;
                            string blunted = formula_sources[frm].sum_src[ss].summation_body;
                            int golden_size = formula_sources[frm].sum_src[ss].down.Length - 5;
                            if (golden_size > 0)
                            {
                                for (int M = 0; M < golden_size / 2 + 1 - ss; M++)
                                {
                                    blunted = BluntBegin(blunted);
                                }
                                formattedText = paragraph.AddFormattedText(blunted);
                                formattedText.Font.Size = 12;
                                paragraph.Format.Font.Italic = true;
                                formattedText.Font.Color = Colors.White;
                            }
                            else
                            {
                                if (formula_sources[frm].sum_src[ss].down.Length == 1)
                                {
                                    formattedText = paragraph.AddFormattedText(formula_sources[frm].sum_src[ss].summation_body + Multiply(".", 3 - ss));
                                    formattedText.Font.Size = 12;
                                    paragraph.Format.Font.Italic = true;
                                    formattedText.Font.Color = Colors.White;
                                }
                                else if (formula_sources[frm].sum_src[ss].down.Length == 2)
                                {
                                    formattedText = paragraph.AddFormattedText(formula_sources[frm].sum_src[ss].summation_body + Multiply(".", 2 - ss));
                                    formattedText.Font.Size = 12;
                                    paragraph.Format.Font.Italic = true;
                                    formattedText.Font.Color = Colors.White;
                                }
                                else if (formula_sources[frm].sum_src[ss].down.Length == 3)
                                {
                                    formattedText = paragraph.AddFormattedText(formula_sources[frm].sum_src[ss].summation_body + Multiply(".", 1 - ss));
                                    formattedText.Font.Size = 12;
                                    paragraph.Format.Font.Italic = true;
                                    formattedText.Font.Color = Colors.White;
                                }
                                else if (formula_sources[frm].sum_src[ss].down.Length == 4)
                                {
                                    formattedText = paragraph.AddFormattedText(formula_sources[frm].sum_src[ss].summation_body);
                                    formattedText.Font.Size = 12;
                                    paragraph.Format.Font.Italic = true;
                                    formattedText.Font.Color = Colors.White;
                                }
                                else if (formula_sources[frm].sum_src[ss].down.Length == 5)
                                {
                                    formattedText = paragraph.AddFormattedText(formula_sources[frm].sum_src[ss].summation_body);
                                    formattedText.Font.Size = 12;
                                    paragraph.Format.Font.Italic = true;
                                    formattedText.Font.Color = Colors.White;
                                }
                            }
                            ss++;
                        }
                    }
                    paragraph = document.LastSection.AddParagraph();
                    bb = 0;
                    ii = 0;
                    ss = 0;
                    init = true;
                    frm++;
                }
                else if (IDS[KK] == (int)ID_MACROS.IDS.TEXT_IDS.FRACTION)
                {
                    formattedText = paragraph.AddFormattedText("\t");
                    formattedText = paragraph.AddFormattedText(fraction_sources[frc].up);
                    formattedText.Font.Size = 12;
                    paragraph.Format.Font.Italic = true;
                    formattedText.Font.Color = Colors.Black;
                    paragraph = document.LastSection.AddParagraph();

                    formattedText = paragraph.AddFormattedText("\t");
                    formattedText = paragraph.AddFormattedText(fraction_sources[frc].symbol);
                    formattedText.Font.Size = 12;
                    formattedText.Font.Section.LastParagraph.Format.SpaceBefore = -10;
                    paragraph.Format.Font.Italic = true;
                    formattedText.Font.Color = Colors.Black;
                    paragraph = document.LastSection.AddParagraph();

                    formattedText = paragraph.AddFormattedText("\t");
                    formattedText = paragraph.AddFormattedText(fraction_sources[frc].down);
                    formattedText.Font.Size = 12;
                    paragraph.Format.Font.Italic = true;
                    formattedText.Font.Color = Colors.Black;
                    paragraph = document.LastSection.AddParagraph();
                    frc++;
                }
            }
        }
        private static void AddBlankPage()
        {
            gl_doc.LastSection.AddPageBreak();
        }
        private static void AllowPageNumber_Mode_I()
        {
            global_pageNumber_allowance_mode_i = true;
        }
        private static void AllowPageNumber_Mode_II()
        {
            global_pageNumber_allowance_mode_ii = true;
        }
        private static void BlockPageNumber_Mode_I()
        {
            global_pageNumber_allowance_mode_i = false;
        }
        private static void BlockPageNumber_Mode_II()
        {
            global_pageNumber_allowance_mode_ii = false;
        }
        //private static System.Drawing.Bitmap ConstructImage(string address)
        //{
        //    string tmp_address = "";
        //    int i = 0;
        //    while (i < address.Length)
        //    {
        //        if (address[i] != '.')
        //        {
        //            tmp_address += address[i];
        //            i++;
        //        }
        //        else break;               
        //    }
        //    string PNG_address = "";
        //    if (PDF_Image.ImageHandling.is_png(address))
        //    {
        //        PNG_address = address;
        //    }
        //    else if (PDF_Image.ImageHandling.is_jpg(address))
        //    {
        //        System.IO.StreamReader _stream = new System.IO.StreamReader(address);
        //        System.IO.Stream stream = _stream.BaseStream;
        //        System.Drawing.Bitmap b = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromStream(stream);
        //        var bitmap = System.Drawing.Bitmap.FromFile(address);
        //        b.Save(System.IO.Path.GetFileName(tmp_address) + ".png", System.Drawing.Imaging.ImageFormat.Png);
        //        PNG_address = System.IO.Path.GetFileName(tmp_address) + ".png";
        //    }
        //    else if (PDF_Image.ImageHandling.is_bmp(address))
        //    {
        //        System.IO.StreamReader _stream = new System.IO.StreamReader(address);
        //        System.IO.Stream stream = _stream.BaseStream;
        //        System.Drawing.Bitmap b = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromStream(stream);
        //        var bitmap = System.Drawing.Bitmap.FromFile(address);
        //        b.Save(System.IO.Path.GetFileName(tmp_address) + ".png", System.Drawing.Imaging.ImageFormat.Png);
        //        PNG_address = System.IO.Path.GetFileName(tmp_address) + ".png";
        //    }
        //    img = new System.Drawing.Bitmap(PNG_address);
        //    return img;
        //}
    }
}