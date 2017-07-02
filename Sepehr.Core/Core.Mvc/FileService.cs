using System.IO;
using System.Web;

namespace Core.Mvc
{
    public class FileService : IFileService
    {
        public void Save(HttpPostedFileBase file, string path, string fileName)
        {
            if (file != null)
            {
                if (fileName != null)
                {
                    var physicalPath = Path.Combine(path, fileName);
                    file.SaveAs(physicalPath);
                }
            }
        }

        public void Remove(string fileName, string path)
        {
            if (fileName != null)
            {
                if (fileName != null)
                {
                    var physicalPath = Path.Combine(path, fileName);
                    if (File.Exists(physicalPath))
                    {
                        File.Delete(physicalPath);
                    }
                }
            }

        }
    }
}
