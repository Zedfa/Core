using Core.Cmn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Drawing.Imaging;
using System.Drawing;

namespace Core.Mvc
{
    public class MvcImageHelper
    {
        public static FileStreamResult GetImageResult(byte[] image)
        {
            if (image.Length == 0)
                return null;
            Image img = ImageHelper.BinaryToImage(image);

            if (img.RawFormat.Equals(ImageFormat.Bmp))
            {
                return new FileStreamResult(new System.IO.MemoryStream(image), "image/bmp");
            }
            else if (img.RawFormat.Equals(ImageFormat.Gif))
            {
                return new FileStreamResult(new System.IO.MemoryStream(image), "image/gif");
            }
            else if (img.RawFormat.Equals(ImageFormat.Jpeg))
            {
                return new FileStreamResult(new System.IO.MemoryStream(image), "image/jpeg");
            }
            else if (img.RawFormat.Equals(ImageFormat.Png))
            {
                return new FileStreamResult(new System.IO.MemoryStream(image), "image/png");
            }
            else if (img.RawFormat.Equals(ImageFormat.Tiff))
            {
                return new FileStreamResult(new System.IO.MemoryStream(image), "image/tiff");
            }
            return new FileStreamResult(new System.IO.MemoryStream(image), "image/jpeg");
        }
    }
}
