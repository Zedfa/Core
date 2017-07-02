using Core.Cmn;
using Core.Mvc.ViewModel;
using Core.Service;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace Core.Mvc.ApiControllers
{
    public class CaptchaApiController : Core.Mvc.Controller.ApiControllerBase
    {
        private IConstantService _constantService;
        public CaptchaApiController(IConstantService constantService)
        {
            _constantService = constantService;
        }
        private string RandomString(int size)
        {
            var chars = "0123456789";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, size)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());

            return result;
        }
        public CaptchaViewModel GetCaptchaImage()
        {

            var random = new Random();
            string randomString = RandomString(4);
            var encryptionKey = string.Empty;
            if (_constantService.TryGetValue<string>("EncryptionKey", out encryptionKey))
            {
                string encryptedKey = randomString + "-" + encryptionKey;
                encryptedKey = EncryptionUtil.Sha1Util.Sha1HashString(encryptedKey);

                var captcha = string.Format("{0}", randomString);

                byte[] content;
                using (var mem = new MemoryStream())
                using (var bmp = new Bitmap(130, 50, PixelFormat.Format32bppArgb))
                using (var gfx = Graphics.FromImage((System.Drawing.Image)bmp))
                {


                    gfx.SmoothingMode = SmoothingMode.AntiAlias;

                    // Create a graphics object for drawing.
                    Graphics g = Graphics.FromImage(bmp);
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    Rectangle rect = new Rectangle(0, 0, 150, 50);

                    // Fill in the background.
                    HatchBrush hatchBrush = new HatchBrush(HatchStyle.SmallConfetti, Color.Gray, Color.White);
                    g.FillRectangle(hatchBrush, rect);

                    // Set up the text font.
                    SizeF size;
                    float fontSize = rect.Height + 1;
                    Font font;
                    // Adjust the font size until the text fits within the image.
                    do
                    {
                        fontSize--;
                        font = new Font("Arial", fontSize, FontStyle.Bold);
                        size = g.MeasureString("", font);
                    } while (size.Width > rect.Width);


                    StringFormat format = new StringFormat();
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Center;

                    // Create a path using the text and warp it randomly.
                    GraphicsPath path = new GraphicsPath();
                    path.AddString("", font.FontFamily, (int)font.Style, font.Size, rect, format);
                    PointF[] points =
			{
				new PointF(random.Next(rect.Width) / -2f, random.Next(rect.Height) / 16f),
				new PointF(rect.Width - random.Next(rect.Width) / -2f, random.Next(rect.Height) / -15f),
				new PointF(random.Next(rect.Width) / -46f, rect.Height - random.Next(rect.Height) / -100f),
				new PointF(rect.Width - random.Next(rect.Width) / 12f, rect.Height - random.Next(rect.Height) / 10f)
			};
                    Matrix matrix = new Matrix();
                    matrix.Translate(0F, 0F);
                    path.Warp(points, rect, matrix, WarpMode.Perspective, 20F);

                    hatchBrush = new HatchBrush(HatchStyle.Percent60, Color.FromArgb(99, 99, 99), Color.DarkGray);
                    gfx.FillPath(hatchBrush, path);

                    gfx.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                    gfx.SmoothingMode = SmoothingMode.AntiAlias;
                    var rand = new Random((int)DateTime.Now.Ticks);
                    int r, x1, yz;
                    var pen = new Pen(Color.Yellow);
                    for (int i = 1; i < 10; i++)
                    {
                        pen.Color = Color.FromArgb(
                        (rand.Next(0, 255)),
                        (rand.Next(0, 255)),
                        (rand.Next(0, 255)));

                        r = rand.Next(0, (130 / 3));
                        x1 = rand.Next(0, 230);
                        yz = rand.Next(0, 30);

                        gfx.DrawEllipse(pen, x1 - r, yz - r, r, r);
                    }

                    int m = Math.Max(rect.Width, rect.Height);
                    for (int i = 0; i < (int)(rect.Width * rect.Height / 30F); i++)
                    {
                        int x = random.Next(rect.Width);
                        int y = random.Next(rect.Height);
                        int w = random.Next(m / 50);
                        int h = random.Next(m / 50);
                        gfx.FillEllipse(hatchBrush, x, y, w, h);
                    }
                    gfx.DrawString(captcha, new Font("Arial", 32, FontStyle.Bold), Brushes.Gray, 10, 3);

                    bmp.Save(mem, System.Drawing.Imaging.ImageFormat.Jpeg);

                    content = mem.GetBuffer();
                }
                return new CaptchaViewModel { Base64imgage = Convert.ToBase64String(content), EncryptedKey = encryptedKey };
               
            }

            else
            {

                throw new Exception("there is no encryptionKey");
            }
        }
        //// GET: api/CaptchaApi
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET: api/CaptchaApi/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST: api/CaptchaApi
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT: api/CaptchaApi/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/CaptchaApi/5
        //public void Delete(int id)
        //{
        //}
    }
}
