using System.Web.Configuration;

namespace Core.WebClientModuleManagement
{
    public class StartupProjectConfig
    {
        public static string DllName { get; set; }
        // public static string AreaPath { get; set; }
       
        public static bool Debug {
            get {
                return Core.Cmn.ConfigHelper.GetSection<CompilationSection>("system.web/compilation").Debug;
            }
        }

    }
}
