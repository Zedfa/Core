using Microsoft.Build.Evaluation;
using System;
using System.IO;
using System.Reflection;


namespace Core.WebClientModuleManagement
{
   internal class SubProjectSpec
    {
        public SubProjectSpec(string path, string dllName)
        {
            FullPath = path+"\\";
            DLLName = dllName;
            Assembly = AppDomain.CurrentDomain.Load(dllName.Replace(".dll",""));
            //#if DEBUG
            if (StartupProjectConfig.Debug)
            {
                ProjectFilePath = Directory.GetFiles(FullPath, "*.csproj")[0];
                Project = new Project(ProjectFilePath);
            }
//#endif
            ProjectName = Assembly.GetName().Name;
            RelativeAreaPath = String.Concat("Areas\\", ProjectName.Substring(0, ProjectName.ToLower().LastIndexOf(".")), "\\");
        }
       
        public Assembly Assembly { get; private set;}

        public string FullPath { get; private set; }
        public string ProjectFilePath { get; private set; }
        public Project Project { get; private set; }
        public string DLLName { get; private set; }
        public string ProjectName { get; private set; }
        public string RelativeAreaPath { get; private set; }

    }
}
