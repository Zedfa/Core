using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Cmn
{
    public class FileHelper
    {
        public static byte[] ReadFile(string path)
        {
            return File.ReadAllBytes(path);
        }
        public static void SaveFile(string path, byte[] fileBytes)
        {
            /// inja check mikonim age folderi ke mikhaim file tosh ijad beshe vojod nadasht
            /// ono ijad mikonim
            (new FileInfo(path)).Directory.Create();

            File.WriteAllBytes(path, fileBytes);
        }
    }
}
