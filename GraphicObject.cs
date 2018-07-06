using System.Collections.Generic;
using ImageMagick;
using PdfSharp.Pdf;
using PdfSharp.Drawing;

namespace PDF_graphic
{
    public enum SYMBOL
    {
        GO_DEFAULT = 0x0000,
        GO_INT     = 0x00A0,
        GO_SUM     = 0x00A1,
        GO_FRC     = 0x00A2,
        GO_OTHERS  = 0x00A3,
    }
    class StringMaker
    {
        public static string symbol_maker(string Base, int repeat)
        {
            string ret = "";
            for (int i = 0; i < repeat; i++)
                ret += Base;
            return ret;
        }
    }
    class GObject
    {
        public string  main;
        public GObject up;
        public GObject down;
        public string  symbol;
        public bool    up_child;
        public bool    down_child;
        public SYMBOL  id;
    }
    class OFormul
    {
        public List<GObject> objects;
    }
    class GraphicalObject
    {
        public static OFormul formula_container = new OFormul();
        public static List<GObject> custom_registered_GObjects = new List<GObject>();
        public static List<string> custom_registered_GObjects_user_commands = new List<string>();
        public static int custom_registered_GObjects_counter = 0;
        public static int is_in(string trg, List<string> list)
        {
            int cnt = list.Count;
            for (int i = 0; i < cnt; i++)
            {
                if (list[i] == trg)
                {
                    return i;
                }
            }
            return -1;
        }
        public void PdfSharp_PNG_handling()
        {
            PdfDocument document = new PdfDocument();
            PdfPage page  = document.AddPage();
            page.Width    = 595;
            page.Height   = 70;
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XPdfFontOptions options = new XPdfFontOptions(PdfFontEncoding.Unicode, PdfFontEmbedding.Always);
            XFont _18Font           = new XFont("Times New Roman", 18, XFontStyle.Italic, options);
            XFont _12Font           = new XFont("Times New Roman", 12, XFontStyle.Italic, options);
            XFont _6Font            = new XFont("Times New Roman", 6, XFontStyle.Italic, options);
            XFont _4Font            = new XFont("Times New Roman", 4, XFontStyle.Italic, options);
            double sum   = 0.00;
            int step     = 1;
            int ref_step = 0;
            for (int i = 0; i < formula_container.objects.Count; i++)
            {
                if (formula_container.objects[i].up_child == true && formula_container.objects[i].down_child == false)
                {
                    gfx.DrawString(formula_container.objects[i].symbol,          _18Font, XBrushes.Black, new XRect(sum + ref_step * 12.00,      12, page.Width, page.Height), XStringFormats.TopLeft);
                    gfx.DrawString(formula_container.objects[i].main,            _12Font, XBrushes.Black, new XRect(sum + step *     12.00,      15, page.Width, page.Height), XStringFormats.TopLeft);
                    gfx.DrawString(formula_container.objects[i].up.symbol,       _6Font,  XBrushes.Black, new XRect(sum + step *     12.00 - 5,  5,  page.Width, page.Height), XStringFormats.TopLeft);
                    gfx.DrawString("   " + formula_container.objects[i].up.main, _6Font,  XBrushes.Black, new XRect(sum + step *     12.00 - 5,  5,  page.Width, page.Height), XStringFormats.TopLeft);
                    gfx.DrawString(formula_container.objects[i].up.up.main,      _4Font,  XBrushes.Black, new XRect(sum + step *     12.00 - 3,  2,  page.Width, page.Height), XStringFormats.TopLeft);
                    gfx.DrawString(formula_container.objects[i].down.main,       _6Font,  XBrushes.Black, new XRect(sum + step *     12.00 - 12, 31, page.Width, page.Height), XStringFormats.TopLeft);
                    gfx.DrawString(formula_container.objects[i].up.down.main,    _4Font,  XBrushes.Black, new XRect(sum + step *     12.00 - 5,  12, page.Width, page.Height), XStringFormats.TopLeft);
                    sum += gfx.MeasureString(formula_container.objects[i].main, _12Font).Width;
                    step++;
                    ref_step++;
                }
                else if (formula_container.objects[i].up_child == false && formula_container.objects[i].down_child == true)
                {
                    gfx.DrawString(formula_container.objects[i].symbol,            _18Font, XBrushes.Black, new XRect(sum + ref_step * 12.00,      12, page.Width, page.Height), XStringFormats.TopLeft);
                    gfx.DrawString(formula_container.objects[i].main,              _12Font, XBrushes.Black, new XRect(sum + step *     12.00,      15, page.Width, page.Height), XStringFormats.TopLeft);
                    gfx.DrawString(formula_container.objects[i].down.symbol,       _6Font,  XBrushes.Black, new XRect(sum + step *     12.00 - 5,  5,  page.Width, page.Height), XStringFormats.TopLeft);
                    gfx.DrawString("   " + formula_container.objects[i].down.main, _6Font,  XBrushes.Black, new XRect(sum + step *     12.00 - 5,  5,  page.Width, page.Height), XStringFormats.TopLeft);
                    gfx.DrawString(formula_container.objects[i].down.up.main,      _4Font,  XBrushes.Black, new XRect(sum + step *     12.00 - 3,  2,  page.Width, page.Height), XStringFormats.TopLeft);
                    gfx.DrawString(formula_container.objects[i].up.main,           _6Font,  XBrushes.Black, new XRect(sum + step *     12.00 - 12, 31, page.Width, page.Height), XStringFormats.TopLeft);
                    gfx.DrawString(formula_container.objects[i].down.down.main,    _4Font,  XBrushes.Black, new XRect(sum + step *     12.00 - 5,  12, page.Width, page.Height), XStringFormats.TopLeft);
                    sum += gfx.MeasureString(formula_container.objects[i].main, _12Font).Width;
                    step++;
                    ref_step++;
                }
                else if (formula_container.objects[i].up_child == true && formula_container.objects[i].down_child == true)
                {
                    gfx.DrawString(formula_container.objects[i].symbol,            _18Font, XBrushes.Black, new XRect(sum + ref_step * 12.00,     12, page.Width, page.Height), XStringFormats.TopLeft);
                    gfx.DrawString(formula_container.objects[i].main,              _12Font, XBrushes.Black, new XRect(sum + step *     12.00,     15, page.Width, page.Height), XStringFormats.TopLeft);
                    gfx.DrawString(formula_container.objects[i].up.symbol,         _6Font,  XBrushes.Black, new XRect(sum + step *     12.00 - 5, 5,  page.Width, page.Height), XStringFormats.TopLeft);
                    gfx.DrawString("   " + formula_container.objects[i].up.main,   _6Font,  XBrushes.Black, new XRect(sum + step *     12.00 - 5, 5,  page.Width, page.Height), XStringFormats.TopLeft);
                    gfx.DrawString(formula_container.objects[i].up.up.main,        _4Font,  XBrushes.Black, new XRect(sum + step *     12.00 - 3, 2,  page.Width, page.Height), XStringFormats.TopLeft);
                    gfx.DrawString(formula_container.objects[i].up.down.main,      _4Font,  XBrushes.Black, new XRect(sum + step *     12.00 - 5, 12, page.Width, page.Height), XStringFormats.TopLeft);
                    gfx.DrawString("   " + formula_container.objects[i].down.main, _6Font,  XBrushes.Black, new XRect(sum + step *     12.00 - 5, 33, page.Width, page.Height), XStringFormats.TopLeft);
                    gfx.DrawString(formula_container.objects[i].down.symbol,       _6Font,  XBrushes.Black, new XRect(sum + step *     12.00 - 5, 33, page.Width, page.Height), XStringFormats.TopLeft);
                    gfx.DrawString(formula_container.objects[i].down.up.main,      _4Font,  XBrushes.Black, new XRect(sum + step *     12.00 - 3, 30, page.Width, page.Height), XStringFormats.TopLeft);
                    gfx.DrawString(formula_container.objects[i].down.down.main,    _4Font,  XBrushes.Black, new XRect(sum + step *     12.00 - 5, 40, page.Width, page.Height), XStringFormats.TopLeft);
                    sum += gfx.MeasureString(formula_container.objects[i].main, _12Font).Width;
                    step++;
                    ref_step++;
                }
                else
                {
                    if (formula_container.objects[i].id == SYMBOL.GO_FRC)
                    {
                        gfx.DrawString(formula_container.objects[i].symbol,    _18Font, XBrushes.Black, new XRect(sum + ref_step * 12.00, 0,  page.Width, page.Height), XStringFormats.TopLeft);
                        gfx.DrawString(formula_container.objects[i].up.main,   _12Font, XBrushes.Black, new XRect(sum + ref_step * 12.00, 6,  page.Width, page.Height), XStringFormats.TopLeft);
                        gfx.DrawString(formula_container.objects[i].down.main, _12Font, XBrushes.Black, new XRect(sum + ref_step * 12.00, 18, page.Width, page.Height), XStringFormats.TopLeft);
                        sum += gfx.MeasureString(formula_container.objects[i].main, _12Font).Width;
                        step++;
                        ref_step++;
                    }
                    else
                    {
                        gfx.DrawString(formula_container.objects[i].symbol, _18Font, XBrushes.Black, new XRect(sum + ref_step * 12.00, 15, page.Width, page.Height), XStringFormats.TopLeft);
                        gfx.DrawString(formula_container.objects[i].main,   _12Font, XBrushes.Black, new XRect(sum + step     * 12.00, 15, page.Width, page.Height), XStringFormats.TopLeft);
                        sum += gfx.MeasureString(formula_container.objects[i].main, _12Font).Width;
                        step++;
                        ref_step++;
                    }
                }
            }
            document.Save("__internal_ObjectiveFormula.pdf");
        }
        public void handle_objecvtive_formula(string command)
        {
            GObject tmp = null;
            GObject tmp_up = null;
            GObject tmp_down = null;
            formula_container.objects = new List<GObject>();
            int j = 0;
            int length = command.Length;
            while (j < length)
            {
                string t = "";
                int Nom = 0, Denom = 0;
                tmp = new GObject();
                tmp_up = new GObject();
                tmp_down = new GObject();
                while (command[j] != '.')
                {
                    t += command[j];
                    j++;
                }
                if (t == "INT") tmp.symbol = StringMaker.symbol_maker("∫ ", 1);
                else if (t == "SUM") tmp.symbol = StringMaker.symbol_maker("Σ ", 1);
                else if (t == "FRC")
                {
                    tmp.symbol = "FRCT_TMP_SYMBOL";
                    tmp.id = SYMBOL.GO_FRC;
                }
                else
                {
                    int ind = is_in(t, custom_registered_GObjects_user_commands);
                    tmp.symbol = custom_registered_GObjects[ind].symbol;
                }
                j++;
                t = "";
                while (command[j] != ',')
                {
                    t += command[j];
                    j++;
                }
                j++;
                tmp.main = t;
                tmp.up_child = false;
                tmp.down_child = false;
                t = "";

                //up
                if (command[j] == '{')
                {
                    j++;
                    tmp.up_child = true;
                    GObject new_tmp = new GObject();
                    GObject new_tmp_up = new GObject();
                    GObject new_tmp_down = new GObject();
                    tmp.up = new_tmp;
                    tmp.up.up = new_tmp_up;
                    tmp.up.down = new_tmp_down;
                    while (command[j] != '}')
                    {
                        while (command[j] != '.')
                        {
                            t += command[j];
                            j++;
                        }
                        if (t == "INT") tmp.up.symbol = StringMaker.symbol_maker("∫ ", 1);
                        else if (t == "SUM") tmp.up.symbol = StringMaker.symbol_maker("Σ ", 1);
                        else if (t == "FRC")
                        {
                            tmp.up.symbol = "FRCT_TMP_SYMBOL";
                            tmp.up.id = SYMBOL.GO_FRC;
                        }
                        j++;
                        t = "";
                        while (command[j] != ',')
                        {
                            t += command[j];
                            j++;
                        }
                        j++;
                        tmp.up.main = t;
                        t = "";
                        while (command[j] != ',')
                        {
                            t += command[j];
                            j++;
                        }
                        j++;
                        tmp.up.up.main = t;
                        t = "";
                        while (command[j] != ';')
                        {
                            t += command[j];
                            j++;
                        }
                        j++;
                        tmp.up.down.main = t;
                        t = "";
                    }
                    j += 2;
                }
                else
                {
                    tmp.up_child = false;
                    while (command[j] != ',')
                    {
                        t += command[j];
                        j++;
                    }
                    Nom = t.Length;
                    j++;
                    tmp.up = tmp_up;
                    tmp.up.main = t;
                    t = "";
                }

                //down
                if (command[j] == '{')
                {
                    j++;
                    tmp.down_child = true;
                    GObject new_tmp = new GObject();
                    GObject new_tmp_up = new GObject();
                    GObject new_tmp_down = new GObject();
                    tmp.down = new_tmp;
                    tmp.down.up = new_tmp_up;
                    tmp.down.down = new_tmp_down;
                    while (command[j] != '}')
                    {
                        while (command[j] != '.')
                        {
                            t += command[j];
                            j++;
                        }
                        if      (t == "INT") tmp.down.symbol = StringMaker.symbol_maker("∫ ", 1);
                        else if (t == "SUM") tmp.down.symbol = StringMaker.symbol_maker("Σ ", 1);
                        else if (t == "FRC")
                        {
                            tmp.down.symbol = "FRCT_TMP_SYMBOL";
                            tmp.down.id = SYMBOL.GO_FRC;
                        }
                        j++;
                        t = "";
                        while (command[j] != ',')
                        {
                            t += command[j];
                            j++;
                        }
                        j++;
                        tmp.down.main = t;
                        t = "";
                        while (command[j] != ',')
                        {
                            t += command[j];
                            j++;
                        }
                        j++;
                        tmp.down.up.main = t;
                        t = "";
                        while (command[j] != ';')
                        {
                            t += command[j];
                            j++;
                        }
                        j++;
                        tmp.down.down.main = t;
                        t = "";
                    }
                    j += 2;
                }
                else
                {
                    tmp.down_child = false;
                    while (command[j] != ';')
                    {
                        t += command[j];
                        j++;
                    }
                    Denom = t.Length;
                    j++;
                    tmp.down = tmp_down;
                    tmp.down.main = t;
                    t = "";
                }
                ///////////////////////////
                if (tmp.id == SYMBOL.GO_FRC)
                {
                    tmp.symbol = StringMaker.symbol_maker("_", Nom >= Denom ? Nom / 2 + 1 : Denom / 2 + 1);
                }
                formula_container.objects.Add(tmp);
                t = "";
                tmp_down = null;
                tmp_up = null;
                tmp = null;
            }
            PdfSharp_PNG_handling();
            MagickReadSettings settings = new MagickReadSettings();
            settings.Density = new Density(500, 500);
            using (MagickImageCollection images = new MagickImageCollection())
            {
                images.Read("__internal_ObjectiveFormula.pdf", settings);
                foreach (MagickImage image in images)
                {
                    image.Write("__internal_ObjectiveFormula" + Global_Formula_Counter.Counter.cntr.ToString() + ".png");
                    image.Format = MagickFormat.Ptif;
                    System.IO.File.Delete("__internal_ObjectiveFormula.pdf");
                }
            }
        }
        public void Register_custom_GObject(string user_command, string symbol)
        {
            GObject obj = new GObject();
            obj.symbol = symbol + " ";
            custom_registered_GObjects_user_commands.Add(user_command);
            custom_registered_GObjects.Add(obj);
        }
    }
}
