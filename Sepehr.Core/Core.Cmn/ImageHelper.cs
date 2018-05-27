using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace Core.Cmn
{
    public class ImageHelper
    {
        /// <summary>
        /// Extracts all bytes representation of any kinds of file such as an Image,by provided file path.
        /// </summary>
        /// <param name="imagefilePath">Path of the file</param>
        /// <returns>byte array</returns>
        public static byte[] ImageToByteArrayFromFilePath(string imagefilePath)
        {
            byte[] imageArray = File.ReadAllBytes(imagefilePath);
            return imageArray;
        }

        /// <summary>
        /// Retreives a System.Drawing.Image instance and then, converts it to a byte array by MemoryStream usage.
        /// </summary>
        /// <param name="imagefilePath">Path of the image file</param>
        /// <returns>byte array</returns>
        public static byte[] ImageToByteArray(string imagefilePath)
        {
            Image image = Image.FromFile(imagefilePath);
            byte[] imageByte = ImageToByteArraybyMemoryStream(image);
            return imageByte;
        }

        /// <summary>
        /// Converts an image,which is in form of a byte array,to System.Drawing.Image
        /// </summary>
        /// <param name="imageByte">image byte representation</param>
        public static Image GetImageFromByteArray(byte[] imageByte)
        {
            Image img = null;
            using (var ms = new MemoryStream(imageByte))
            {
                //using (var image = Image.FromStream(ms))
                {
                    img = Image.FromStream(ms);
                    return img;
                }
            }
        }

        /// <summary>
        /// Converts an image,which is in form of a byte array,to System.Drawing.Image
        /// </summary>
        /// <param name="imageByte">Image file byte[] representation</param>
        public static void ConvertByteArrayToImageSaveItToFileSystem(byte[] imageByte, string targetFileSystemSavingPath)
        {
            using (var ms = new MemoryStream(imageByte))
            {
                Image image = Image.FromStream(ms);
                image.Save(targetFileSystemSavingPath);
            }
        }

        public static string GetImageMimeType(byte[] imageBinary)
        {
            System.Drawing.Image image = ImageHelper.BinaryToImage((byte[])imageBinary);

            if (image.RawFormat.Equals(ImageFormat.Bmp))
            {
                return "image/bmp";
            }
            else if (image.RawFormat.Equals(ImageFormat.Gif))
            {
                return "image/gif";
            }
            else if (image.RawFormat.Equals(ImageFormat.Jpeg))
            {
                return "image/jpeg";
            }
            else if (image.RawFormat.Equals(ImageFormat.Png))
            {
                return "image/png";
            }
            else if (image.RawFormat.Equals(ImageFormat.Tiff))
            {
                return "image/tiff";
            }
            else
            {
                throw new Exception("image mimeType is not acceptable.error accoured in imagehelper/GetImageMimeType");
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        private static byte[] ImageToByteArraybyMemoryStream(System.Drawing.Image image)
        {
            MemoryStream ms = new MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return ms.ToArray();
        }

        public static byte[] GetByteArrayOfCurrentInputStream(Stream inputStream)
        {
            MemoryStream ms = new MemoryStream();
            inputStream.CopyTo(ms);
            return ms.ToArray();
        }

        public static bool IsImageExtension(string extension)
        {
            switch (extension.ToLower())
            {
                case "png":
                case "jpg":
                case "jpeg":
                case "bmp":
                case "tiff":
                case "gif":
                    return true;

                default:
                    return false;
            }
        }

        public static Image BinaryToImage(byte[] binaryData)
        {
            if (binaryData == null)
                return null;

            if (binaryData.Length == 0)
                return null;
            byte[] buffer = binaryData.ToArray();
            MemoryStream memStream = new MemoryStream();
            memStream.Write(buffer, 0, buffer.Length);
            return Image.FromStream(memStream);
        }

        public static byte[] ImageToBinary(string imagePath)
        {
            FileStream fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[fileStream.Length];
            fileStream.Read(buffer, 0, (int)fileStream.Length);
            fileStream.Close();
            return buffer;
        }

        public static byte[] ImageToBinary(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }

        public static Bitmap CreateThumbnail(Bitmap loBMP, int lnWidth, int lnHeight)
        {
            //if (System.IO.File.Exists(lcFilename))
            //{
            System.Drawing.Bitmap bmpOut = null;
            try
            {
                //System.Drawing.Bitmap loBMP = new Bitmap(lcFilename);
                ImageFormat loFormat = loBMP.RawFormat;

                if (lnWidth == 0 && lnHeight == 0)
                {
                    lnWidth = loBMP.Width;
                    lnHeight = loBMP.Height;
                }
                decimal lnRatio;
                int lnNewWidth = 0;
                int lnNewHeight = 0;

                //*** If the image is smaller than a thumbnail just return it
                if (loBMP.Width < lnWidth && loBMP.Height < lnHeight)
                {
                    return loBMP;
                }

                if (loBMP.Width > loBMP.Height)
                {
                    lnRatio = (decimal)lnWidth / loBMP.Width;
                    lnNewWidth = lnWidth;
                    decimal lnTemp = loBMP.Height * lnRatio;
                    lnNewHeight = (int)lnTemp;
                }
                else
                {
                    lnRatio = (decimal)lnHeight / loBMP.Height;
                    lnNewHeight = lnHeight;
                    decimal lnTemp = loBMP.Width * lnRatio;
                    lnNewWidth = (int)lnTemp;
                }
                bmpOut = new Bitmap(lnNewWidth, lnNewHeight);

                Graphics g = Graphics.FromImage(bmpOut);
                g.FillRectangle(Brushes.White, 0, 0, lnNewWidth, lnNewHeight);
                g.DrawImage(loBMP, 0, 0, lnNewWidth, lnNewHeight);
                loBMP.Dispose();
            }
            catch
            {
                //throw (new Exception(ex.Message + Environment.NewLine + lcFilename));
            }

            return bmpOut;
            //}
            //else
            //{
            //    throw (new Exception("Image does not exists ! " + Environment.NewLine + lcFilename));
            //}
        }

        public static Bitmap CreateThumbnail(string lcFilename, int lnWidth, int lnHeight)
        {
            if (System.IO.File.Exists(lcFilename))
            {
                System.Drawing.Bitmap bmpOut = null;
                try
                {
                    System.Drawing.Bitmap loBMP = new Bitmap(lcFilename);
                    ImageFormat loFormat = loBMP.RawFormat;

                    if (lnWidth == 0 && lnHeight == 0)
                    {
                        lnWidth = loBMP.Width;
                        lnHeight = loBMP.Height;
                    }
                    decimal lnRatio;
                    int lnNewWidth = 0;
                    int lnNewHeight = 0;

                    //*** If the image is smaller than a thumbnail just return it
                    if (loBMP.Width < lnWidth && loBMP.Height < lnHeight)
                    {
                        return loBMP;
                    }

                    if (loBMP.Width > loBMP.Height)
                    {
                        lnRatio = (decimal)lnWidth / loBMP.Width;
                        lnNewWidth = lnWidth;
                        decimal lnTemp = loBMP.Height * lnRatio;
                        lnNewHeight = (int)lnTemp;
                    }
                    else
                    {
                        lnRatio = (decimal)lnHeight / loBMP.Height;
                        lnNewHeight = lnHeight;
                        decimal lnTemp = loBMP.Width * lnRatio;
                        lnNewWidth = (int)lnTemp;
                    }
                    bmpOut = new Bitmap(lnNewWidth, lnNewHeight);

                    Graphics g = Graphics.FromImage(bmpOut);
                    g.FillRectangle(Brushes.White, 0, 0, lnNewWidth, lnNewHeight);
                    g.DrawImage(loBMP, 0, 0, lnNewWidth, lnNewHeight);
                    loBMP.Dispose();
                }
                catch (Exception ex)
                {
                    throw (new Exception(ex.Message + Environment.NewLine + lcFilename));
                }

                return bmpOut;
            }
            else
            {
                throw (new Exception("Image does not exists ! " + Environment.NewLine + lcFilename));
            }
        }
    }
}