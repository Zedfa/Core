using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Core.Mvc.Helpers
{
    public static class Captcha
    {

        public static MvcHtmlString CaptchaCr(this HtmlHelper helper, string captchaTextBoxName)
        {
           
            TagBuilder captchaTag = new TagBuilder("span");
            UrlHelper urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
            captchaTag.InnerHtml = @"
            <div>
                    <div style='float:right;'>
                        <span id='captchaContainer'></span>
                        <img id='imgCaptchaLoading' src='" + urlHelper.Content(string.Format("Areas/{0}/Content/images/captcha-loading.gif",new CoreAreaRegistration().AreaName)) + @"' alt='در حال بارگذاری' />
                        <input id='hdnCaptchaGuid' name='hdnCaptchaGuid' type='hidden' value='" + Guid.NewGuid().ToString("N") + @"' />
                    </div>
                    <div style='float: left;margin: 7px 0px 7px 25px;'>
                       <a class='" + StyleKind.Button + @"'>
                          <span id='btnCaptchaRefresh' class ='" + StyleKind.Icons.Refresh + @"' ></span>
                       </a>
                    </div>
            </div>      
            <script type='text/javascript'>
            $(document).ready(function () {
                 loadCaptcha();
                 var interval = setInterval(function () {

                 if (!$('#captchaContainer').length) {
                         clearInterval(interval);
                     }
            else {
                loadCaptcha();
            }
        }, 600000);

     });
          </script>";

            TagBuilder container = new TagBuilder("div");
            TagBuilder commentPart = new TagBuilder("div");
            commentPart.SetInnerText("عبارت زیر را وارد نمایید");
            container.InnerHtml = commentPart + helper.TextBoxCr(captchaTextBoxName).ToHtmlString() + captchaTag;

            return MvcHtmlString.Create(container.ToString());
        }

        public class CaptchaControl : System.Web.UI.WebControls.Image
        {

            public enum CharSets
            {
                Numbers,
                Letters,
                AllLowerCaseLetters,
                NumberAndAllLowerCaseLetters,
                NumberAndLetters
            }

            public string Guid { get; private set; }

            public bool IgnoreCase { get; set; }

            public int CharCount { get; set; }

            public CharSets CharSet { get; set; }

            private float _currX { get; set; }

            public string Password { get; set; }

            private FontFamily _fontFamily { get; set; }

            private float _fontSize { get; set; }

            private string _code { get; set; }

            protected override void OnLoad(EventArgs e)
            {
                base.OnLoad(e);
            }

            //protected override object SaveViewState()
            //{
            //    object baseState = base.SaveViewState();
            //    object[] allStates = new object[3];
            //    allStates[0] = baseState;
            //    allStates[1] = CaptchaHelper.EncryptString(_code, Password);
            //    allStates[2] = this.ImageUrl;
            //    return allStates;
            //}

            //protected override void LoadViewState(object savedState)
            //{
            //    if (savedState != null)
            //    {
            //        object[] myState = (object[])savedState;
            //        if (myState[0] != null)
            //            base.LoadViewState(myState[0]);
            //        if (myState[1] != null)
            //            _code = CaptchaHelper.DecryptString((string)myState[1], Password);
            //        if (myState[2] != null)
            //            ImageUrl = (string)myState[2];
            //    }
            //}


            public static CaptchaControl CreateControl(bool ignoreCase, CharSets charSets, int charCount, int width, int height, string guid)
            {
                CaptchaControl control = new CaptchaControl(ignoreCase, charSets, charCount, width, height, guid);
                return control;
            }

            public void RemovedCallback(String k, Object v, CacheItemRemovedReason r)
            {
                //itemRemoved = true;
                //reason = r;
                //this.Context.Response.p = Captcha.CurrentContext;
                //var response = new System.Net.Http.HttpResponseMessage(HttpStatusCode.RequestTimeout);
                // response.Content.Headers.Add("sdfsf", "dddddd");
               // CurrentContext.Response.StatusCode = (int)HttpStatusCode.RequestTimeout;
               // CurrentContext.Response.StatusDescription = "dfgdgddgdgd";
                //var context = new HttpContext(CurrentContext.Request, CurrentContext.Response);
               
                //HttpContext.Current = context;
               // throw new Exception("fdijgfilgf");
                //this.Context.Response.StatusCode = (int)HttpStatusCode.RequestTimeout;
               // var c = System.Web.HttpContext.Current;
               
            }
            public Bitmap GetImage()
            {
                _code = CaptchaHelper.GetCode(this.CharCount, CharSet);

                var onRemove = new CacheItemRemovedCallback(this.RemovedCallback);


                if (HttpRuntime.Cache.Get(Guid) != null)
                {
                    HttpRuntime.Cache.Remove(Guid);
                }

                HttpRuntime.Cache.Add(Guid, _code, null, DateTime.Now.AddMinutes(10), Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, onRemove);

                var hatchBrush = new HatchBrush(HatchStyle.LargeConfetti, Color.Gray, Color.DarkGray);

                return CaptchaImageMaker.GetImage(_code, (int)this.Width.Value, (int)this.Height.Value, hatchBrush);
            }

            private CaptchaControl(bool ignoreCase, CharSets charSets, int charCount, int width, int height, string guid)
            {
                Guid = guid;
                IgnoreCase = ignoreCase;
                CharCount = charCount;
                this.Width = new Unit(width);
                this.Height = new Unit(height);
                Password = this.GetType().AssemblyQualifiedName;
                CharSet = CharSet;
            }

            public static bool Validate(string input, string key)
            {

                return string.Compare((string)HttpRuntime.Cache.Get(key), input, true) == 0;

            }


        }

        internal class CaptchaHelper
        {
            internal static char GetChar(CaptchaControl.CharSets charSet)
            {
                var num = 0;
                switch (charSet)
                {
                    case CaptchaControl.CharSets.NumberAndLetters:
                        num = Random.Next(0, 3);
                        break;
                    case CaptchaControl.CharSets.NumberAndAllLowerCaseLetters:
                        num = Random.Next(0, 2);
                        break;
                    case CaptchaControl.CharSets.AllLowerCaseLetters:
                        num = 1;
                        break;
                    case CaptchaControl.CharSets.Letters:
                        num = Random.Next(1, 2);
                        break;
                    default:
                        num = 0;
                        break;
                }
                return GetChar(num);
            }

            internal static char GetChar(int charSet)
            {
                int num = 48;
                switch (charSet)
                {
                    case 0:
                        num = Random.Next(48, 58);   //numbers
                        break;
                    case 1:
                        num = Random.Next(97, 123);  //lower case letters
                        break;
                    case 2:
                        num = Random.Next(65, 91);  //uppercase letters

                        break;
                }
                return Convert.ToChar(num);
            }

            internal static Random Random = new Random();

            internal static string GetCode(int charCount, CaptchaControl.CharSets charSet)
            {
                string s = string.Empty;
                for (int i = 0; i < charCount; i++)
                {
                    var a = GetChar(charSet);
                    s += a;
                }
                return s;
            }

            #region crypter
            internal static string EncryptString(string Message, string Passphrase)
            {
                byte[] Results;
                System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

                // Step 1. We hash the passphrase using MD5
                // We use the MD5 hash generator as the result is a 128 bit byte array
                // which is a valid length for the TripleDES encoder we use below

                MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
                byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(Passphrase));

                // Step 2. Create a new TripleDESCryptoServiceProvider object
                TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

                // Step 3. Setup the encoder
                TDESAlgorithm.Key = TDESKey;
                TDESAlgorithm.Mode = CipherMode.ECB;
                TDESAlgorithm.Padding = PaddingMode.PKCS7;

                // Step 4. Convert the input string to a byte[]
                byte[] DataToEncrypt = UTF8.GetBytes(Message);

                // Step 5. Attempt to encrypt the string
                try
                {
                    ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();
                    Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
                }
                finally
                {
                    // Clear the TripleDes and Hashprovider services of any sensitive information
                    TDESAlgorithm.Clear();
                    HashProvider.Clear();
                }

                // Step 6. Return the encrypted string as a base64 encoded string
                return Convert.ToBase64String(Results);
            }

            internal static string DecryptString(string Message, string Passphrase)
            {
                byte[] Results;
                System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

                // Step 1. We hash the passphrase using MD5
                // We use the MD5 hash generator as the result is a 128 bit byte array
                // which is a valid length for the TripleDES encoder we use below

                MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
                byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(Passphrase));

                // Step 2. Create a new TripleDESCryptoServiceProvider object
                TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

                // Step 3. Setup the decoder
                TDESAlgorithm.Key = TDESKey;
                TDESAlgorithm.Mode = CipherMode.ECB;
                TDESAlgorithm.Padding = PaddingMode.PKCS7;

                // Step 4. Convert the input string to a byte[]
                byte[] DataToDecrypt = Convert.FromBase64String(Message);

                // Step 5. Attempt to decrypt the string
                try
                {
                    ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();
                    Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
                }
                finally
                {
                    // Clear the TripleDes and Hashprovider services of any sensitive information
                    TDESAlgorithm.Clear();
                    HashProvider.Clear();
                }

                // Step 6. Return the decrypted string in UTF8 format
                return UTF8.GetString(Results);
            }

            #endregion
        }

        internal class CaptchaImageMaker
        {
            private Bitmap _image { get; set; }
            private HatchBrush _hatchBrush { get; set; }

            private float _currX { get; set; }
            private int _width { get; set; }
            private int _height { get; set; }

            private FontFamily _fontFamily { get; set; }

            private float _fontSize { get; set; }

            private string _code { get; set; }

            internal static Bitmap GetImage(string code, int width, int height, HatchBrush hatchBrush)
            {
                if (string.IsNullOrEmpty(code)) throw new ArgumentException("code can not be null or empty.");
                var cim = new CaptchaImageMaker(code, width, height, hatchBrush);
                return cim.GetImage();
            }

            private CaptchaImageMaker(string code, int width, int height, HatchBrush hatchBrush)
            {
                this._width = width;
                this._height = height;
                _code = code;
                _fontFamily = FontFamily.GenericSerif;
                _fontSize = 10f;
                _hatchBrush = hatchBrush;
                _image = new Bitmap(this._width, this._height, PixelFormat.Format32bppArgb);
            }

            private Bitmap GetImage()
            {
                var g = Graphics.FromImage(_image);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                var rect = GetFrame(g);

                foreach (var c in _code)
                {
                    DrawText(g, c.ToString());
                }

                AddNoise(rect, g);
                g.Save();
                g.Dispose();
                return _image;
            }

            private void DrawText(Graphics g, string text)
            {
                _fontSize = (float)this._height / 2;
                var font = new Font(this._fontFamily, _fontSize, FontStyle.Bold);
                var size = g.MeasureString(text, font);
                var bitmap = GetCharBitMap(text, _image.Height, g);
                var gr = Graphics.FromImage(bitmap);
                DistortImage(gr);
                gr.DrawString(text, font, _hatchBrush, 0, 0);
                g.DrawImage(bitmap, _currX, 0);
                _currX += size.Width;
            }

            private Rectangle GetFrame(Graphics g)
            {
                var rect = new Rectangle(0, 0, (int)this._width, (int)this._height);
                var brush = new HatchBrush(HatchStyle.SmallConfetti, Color.LightGray, Color.White);
                g.FillRectangle(brush, rect);
                return rect;
            }

            private Bitmap GetCharBitMap(string text, float maxHeight, Graphics g)
            {
                var fontSize = maxHeight;
                var font = new Font(this._fontFamily, fontSize, FontStyle.Bold);
                var size = g.MeasureString(text, font);
                while (size.Height > maxHeight)
                {
                    fontSize--;
                    font = new Font(this._fontFamily, fontSize, FontStyle.Bold);
                    size = g.MeasureString(text, font);
                }

                return new Bitmap((int)size.Width, (int)size.Height, PixelFormat.Format32bppArgb);
            }

            private void DistortImage(Graphics g)
            {
                Matrix tran = new Matrix();
                var method = CaptchaHelper.Random.Next(3);

                switch (method)
                {
                    case 0:
                        tran.Rotate(15);
                        break;
                    case 1:
                        tran.Shear(0.2f, 0.3f);
                        break;
                    case 2:
                        tran.Scale(1.3f, 1, MatrixOrder.Append);
                        break;
                }

                g.MultiplyTransform(tran);
            }

            private void AddNoise(Rectangle rect, Graphics g)
            {
                for (var i = 0; i < (int)(rect.Width * rect.Height / 50F); i++)
                {
                    var x = CaptchaHelper.Random.Next(rect.Width);
                    var y = CaptchaHelper.Random.Next(rect.Height);
                    g.DrawString("*", new Font(_fontFamily, 2, FontStyle.Strikeout), new HatchBrush(HatchStyle.SmallConfetti, Color.DarkGray, Color.Gray), x, y);
                }
            }
        }

    }
}
