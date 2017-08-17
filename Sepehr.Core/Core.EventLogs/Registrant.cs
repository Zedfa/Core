using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.EventLogs
{
    class Registrant
    {
        private static int _maxLogSize = 16384;//1GB
       public void SimulateInstall(string windowsEventFolder,string logName)
        {
               
            var sourceFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            CreateManifestFiles(sourceFolder);


            // create deployment folder if needed
            if (Directory.Exists(windowsEventFolder))
            {
                SimulateUninstall(windowsEventFolder);
            }

            Directory.CreateDirectory(windowsEventFolder);

            foreach (var filename in Directory.GetFiles(sourceFolder, "*.etwManifest.???"))
            {
                var destPath = Path.Combine(windowsEventFolder, Path.GetFileName(filename));

                File.Copy(filename, destPath, true);
            }


            foreach (var filename in Directory.GetFiles(windowsEventFolder, "*.etwManifest.man"))
            {
                var installCommandArgs = string.Format("im {0} /rf:\"{1}\" /mf:\"{1}\"",
                    filename,
                    Path.Combine(windowsEventFolder, Path.GetFileNameWithoutExtension(filename) + ".dll"));
                var configArgs = $"sl {logName} /ms:{_maxLogSize}";
                   

                // as a precaution uninstall the manifest.      
                Process.Start(new ProcessStartInfo("wevtutil.exe", "um" + installCommandArgs.Substring(2)) { Verb = "runAs" }).WaitForExit();
                Thread.Sleep(2000);

                Process.Start(new ProcessStartInfo("wevtutil.exe", installCommandArgs) { Verb = "runAs" }).WaitForExit();
                Process.Start(new ProcessStartInfo("wevtutil.exe", configArgs) { Verb = "runAs" }).WaitForExit();

            }

            Thread.Sleep(1000);

        }


       public void SimulateUninstall(string windowsEventFolder)
        {
            // run wevtutil elevated to unregister the ETW manifests

            foreach (var filename in Directory.GetFiles(windowsEventFolder, "*.etwManifest.man"))
            {
                var commandArgs = string.Format("um {0}", filename);

                // The 'RunAs' indicates it needs to be elevated.  
                var process = Process.Start(new ProcessStartInfo("wevtutil.exe", commandArgs) { Verb = "runAs" });
                process.WaitForExit();
            }

            // If this fails, it means that something is using the directory.  Typically this is an eventViewer or 
            // a command prompt in that directory or visual studio.If all else fails, rebooting should fix this.  
            if (Directory.Exists(windowsEventFolder))
                Directory.Delete(windowsEventFolder, true);



        }

        private static void CreateManifestFiles(string locationManifest)
        {
            var proc = Process.Start(new ProcessStartInfo($"{locationManifest}\\eventRegister.exe", $"{locationManifest}\\Core.Service.dll") { Verb = "runAs" });
            proc.WaitForExit();
            if (proc.ExitCode > 0)
                throw new Exception($"core.mvc or eventRegister.exe is not available in this location:{locationManifest}");

        }

    }

}
