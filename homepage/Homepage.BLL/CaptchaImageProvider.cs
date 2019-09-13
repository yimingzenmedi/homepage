using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace HomepageWeb.BLL
{
    public class CaptchaImageProvider
    {

        /// <summary>
        /// given a captcha text and returns an captcha image
        /// </summary>
        /// <param name="captchaText"></param>
        /// <returns>captcha image byte list in png</returns>
        public byte[] DrawCaptchaImage(string captchaText) 
        {
            //checke if the captcha is empty:
            if (captchaText == null || captchaText.Trim() == string.Empty)
            {
                throw new Exception("Empty captcha text!");
            }

            //create image:
            int iWidth = 100;
            int iHeight = 35;
            Bitmap image = new Bitmap(iWidth, iHeight);

            //get a Graphics from the image:
            Graphics g = Graphics.FromImage(image);

            try
            {
                Random r = new Random();
                //clear bg color:
                g.Clear(Color.White);
                //draw lines:
                for (int i = 0; i < 10; i++)
                {
                    int x1 = r.Next(image.Width);
                    int x2 = r.Next(image.Width);
                    int y1 = r.Next(image.Height);
                    int y2 = r.Next(image.Height);
                    
                    g.DrawLine(new Pen(Color.Gray), x1, y1, x2, y2);
                }
                //draw dots:
                for (int i = 0; i < 60; i++)
                {
                    int x = r.Next(image.Width);
                    int y = r.Next(image.Height);
                    image.SetPixel(x, y, Color.FromArgb(r.Next()));
                }
                //border:
                g.DrawRectangle(new Pen(Color.SaddleBrown), 0, 0, image.Width - 1, image.Height - 1);
                //font:
                Font f = new Font("Arial", 16, (FontStyle.Italic));
                // brush:
                System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.Purple, 1.2f, true);
                g.DrawString(captchaText, f, brush, 2, 2);
                //return the image:
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    //define the image in png
                    image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

                    return ms.ToArray();
                }
            }
            finally
            {
                //release Bitmap and Graphics
                g.Dispose();
                image.Dispose();
            }
        }
    }
}
