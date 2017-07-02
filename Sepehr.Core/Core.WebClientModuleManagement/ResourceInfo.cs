using System;
using System.IO;


namespace Core.WebClientModuleManagement
{
    internal class ResourceInfo
    {
        public ResourceInfo( SubProjectSpec project,string fullPath)
        {
            var relativePath = fullPath.Replace(project.FullPath, "");
            DllName = project.DLLName;
            CSProjFilePath = $"{project.RelativeAreaPath}{relativePath}";
            SourceVersionFilePath = Path.GetFullPath(String.Concat(project.FullPath, "FilesVersionInfo.txt"));
            FullPath = Path.GetFullPath(String.Concat(AppDomain.CurrentDomain.BaseDirectory, project.RelativeAreaPath, relativePath));
            SourcePath = Path.GetFullPath(String.Concat(project.FullPath, relativePath));
        }
        public string SourceVersionFilePath { get; }
        public string DllName { get;  }
        //public string Key { get; set; }
        //public string Name { get; set; }
        //public string Extention { get; set; }
        public string FullPath { get;  }
        public string SourcePath { get; }
        public string BundlePath { get;  }
        public string CSProjFilePath { get;  }
      
    }
}
