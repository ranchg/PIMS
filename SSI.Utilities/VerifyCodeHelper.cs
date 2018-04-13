using System;
using System.Drawing;
using System.IO;

namespace SSI.Utilities
{
    public class VerifyCodeHelper
    {
        public static byte[] Create()
        {
            int codeWidth = 24;
            int codeHeight = 36;
            int codeLength = 4;
            string strCodes = "23456789ABCDEFGHJKMNPQRSTUVWXYZabcdefghjkmnpqrstuvwxyz";
            string strCode = string.Empty;
            Random random = new Random();
            for (int i = 0; i < codeLength; i++)
            {
                strCode += strCodes[random.Next(strCodes.Length)];
            }
            SessionHelper.Add("session_verifycode", Md5Helper.MD5(strCode.ToLower(), 0x20));
            Bitmap image = new Bitmap(strCode.Length * codeWidth, codeHeight);
            Graphics g = Graphics.FromImage(image);
            g.Clear(Color.White);
            g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
            for (int i = 0; i < 25; i++)
            {
                int x1 = random.Next(image.Width);
                int x2 = random.Next(image.Width);
                int y1 = random.Next(image.Height);
                int y2 = random.Next(image.Height);
                g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
            }
            StringFormat format = new StringFormat(StringFormatFlags.NoClip);
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;
            Font font = new Font("Arial", 18, (FontStyle.Bold | FontStyle.Italic));
            Brush brush = new SolidBrush(Color.Blue);
            for (int i = 0; i < strCode.Length; i++)
            {
                Point dot = new Point(18, 18);
                float angle = random.Next(-45, 45);
                g.TranslateTransform(dot.X, dot.Y);
                g.RotateTransform(angle);
                g.DrawString(strCode[i].ToString(), font, brush, 1, 1, format);
                g.RotateTransform(-angle);
                g.TranslateTransform(0, -dot.Y);
            }
            for (int i = 0; i < 100; i++)
            {
                int x = random.Next(image.Width);
                int y = random.Next(image.Height);
                image.SetPixel(x, y, Color.FromArgb(random.Next()));
            }
            using (MemoryStream stream = new MemoryStream())
            {
                image.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                return stream.ToArray();
            }
        }
        public static bool Check(string verifyCode)
        {
            return Md5Helper.MD5(verifyCode.ToLower(), 0x20) == SessionHelper.Get("session_verifycode").ToString();
        }
    }
}
