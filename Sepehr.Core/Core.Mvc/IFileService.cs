using System.Web;

namespace Core.Mvc
{
      public  interface IFileService
      {
          void Save(HttpPostedFileBase file,string path,string fileName);
          void Remove(string fileName, string path);

      }
}
