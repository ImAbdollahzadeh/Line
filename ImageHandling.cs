namespace PDF_Image
{
    class ImageHandling
    {
        public static bool is_jpg(string address)
        {
            System.IO.StreamReader _stream = new System.IO.StreamReader(address);
            System.IO.Stream stream = _stream.BaseStream;
            stream.Seek(0, System.IO.SeekOrigin.Begin);
            var buf = new byte[4];
            stream.Read(buf, 0, 4);
            if (buf[0] == 0xFF && buf[1] == 0xD8 && buf[2] == 0xFF && buf[3] == 0xE0)
                return true;
            return false;
        }
        public static bool is_png(string address)
        {
            System.IO.StreamReader _stream = new System.IO.StreamReader(address);
            System.IO.Stream stream = _stream.BaseStream;
            stream.Seek(0, System.IO.SeekOrigin.Begin);
            var buf = new byte[8];
            stream.Read(buf, 0, 8);
            if (buf[0] == 0x89 && buf[1] == 0x50 && buf[2] == 0x4E && buf[3] == 0x47 && buf[4] == 0x0D && buf[5] == 0x0A && buf[6] == 0x1A && buf[7] == 0x0A)
                return true;
            return false;
        }
        public static bool is_bmp(string address)
        {
            System.IO.StreamReader _stream = new System.IO.StreamReader(address);
            System.IO.Stream stream = _stream.BaseStream;
            stream.Seek(0, System.IO.SeekOrigin.Begin);
            var buf = new byte[2];
            stream.Read(buf, 0, 2);
            if (buf[0] == 0x4D && buf[1] == 0x42)
                return true;
            return false;
        }
        public static bool is_gif(string address)
        {
            System.IO.StreamReader _stream = new System.IO.StreamReader(address);
            System.IO.Stream stream = _stream.BaseStream;
            stream.Seek(0, System.IO.SeekOrigin.Begin);
            var buf = new byte[3];
            stream.Read(buf, 0, 3);
            if (buf[0] == 'G' && buf[1] == 'I' && buf[2] == 'F')
                return true;
            return false;
        }
    }
}
