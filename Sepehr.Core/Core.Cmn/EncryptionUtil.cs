using System.Security.Cryptography;
using System.Text;

namespace Core.Cmn
{
   public static  class EncryptionUtil
    {
       public static class Sha1Util
       {

           public static string Sha1HashString(string s)
           {
               byte[] bytes = Encoding.UTF8.GetBytes(s);

               var sha1 = SHA1.Create();
               byte[] hashBytes = sha1.ComputeHash(bytes);

               return HexStringFromBytes(hashBytes);
           }

           private static string HexStringFromBytes(byte[] bytes)
           {
               var sb = new StringBuilder();
               for (int index = 0; index < bytes.Length; index++)
               {
                   byte b = bytes[index];
                   var hex = b.ToString("x2");
                   sb.Append(hex);
               }
               return sb.ToString();
           }

       }
    }
}
