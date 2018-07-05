using System;
using PdfSharp.Pdf;

namespace PDF_PAGE_NUMBER
{
    class FormattedPageNumbers
    {
        const int Begining = 110;
        public static string makeIt(string out_name, string source, ID_MACROS.IDS.PAGE_MODE md)
        {
            if (md == ID_MACROS.IDS.PAGE_MODE.mode1)
            {
                string filename = source;
                PdfDocument outputDocument = new PdfDocument();
                PdfSharp.Drawing.XFont font = new PdfSharp.Drawing.XFont("Times New Roman", 10, PdfSharp.Drawing.XFontStyle.Regular);
                PdfSharp.Drawing.XStringFormat format = new PdfSharp.Drawing.XStringFormat();
                format.Alignment = PdfSharp.Drawing.XStringAlignment.Near;
                format.LineAlignment = PdfSharp.Drawing.XLineAlignment.Far;
                PdfSharp.Drawing.XGraphics gfx;
                PdfSharp.Drawing.XRect box;
                PdfSharp.Drawing.XPdfForm form = PdfSharp.Drawing.XPdfForm.FromFile(filename);
                for (int idx = 0; idx < form.PageCount; idx++)
                {
                    PdfPage page = outputDocument.AddPage();
                    double width = page.Width;
                    double height = page.Height;
                    gfx = PdfSharp.Drawing.XGraphics.FromPdfPage(page);
                    form.PageNumber = idx + 1;
                    box = new PdfSharp.Drawing.XRect(0, 0, width, height);
                    gfx.DrawImage(form, box);
                    box.Inflate(0, -30);

                }
                filename = out_name;
                outputDocument.Save(filename);
                return filename;
            }
            else
            {
                int[] lst = PDF_MAKER.PDF_MAKER.Page_Number_List;
                string filename =  source;
                PdfDocument outputDocument = new PdfDocument();
                PdfSharp.Drawing.XFont font = new PdfSharp.Drawing.XFont("Times New Roman", 10, PdfSharp.Drawing.XFontStyle.Regular);
                PdfSharp.Drawing.XStringFormat format = new PdfSharp.Drawing.XStringFormat();
                format.Alignment = PdfSharp.Drawing.XStringAlignment.Center;
                format.LineAlignment = PdfSharp.Drawing.XLineAlignment.Far;
                PdfSharp.Drawing.XGraphics gfx;
                PdfSharp.Drawing.XRect box;
                PdfSharp.Drawing.XPdfForm form = PdfSharp.Drawing.XPdfForm.FromFile(filename);
                for (int idx = 0; idx < form.PageCount; idx++)
                {
                    PdfPage page = outputDocument.AddPage();
                    double width = page.Width;
                    double height = page.Height;
                    gfx = PdfSharp.Drawing.XGraphics.FromPdfPage(page);
                    form.PageNumber = idx + 1;
                    box = new PdfSharp.Drawing.XRect(0, 0, width, height);
                    gfx.DrawImage(form, box);
                    box.Inflate(0, -30);

                    if (lst[idx] == 2)
                    {
                        gfx.DrawString(String.Format("{1}", filename, idx + 1), font, PdfSharp.Drawing.XBrushes.Black, box, format);
                    }
                    else if (lst[idx] == 1)
                    {
                        gfx.DrawString(String.Format("{1}", filename, PAGE_MODE_II_NUMBERS.RomanNumbers.convert(idx + 1)), font, PdfSharp.Drawing.XBrushes.Black, box, format);
                    }
                    else { }
                }
                filename = out_name;
                outputDocument.Save(filename);
                return filename;
            }
        }
    }
}