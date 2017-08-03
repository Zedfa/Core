using System.Diagnostics;
using System.Reflection;
using Core.Service;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Core.TraceViewer
{
    static class RegisterEventLogSourceWithOperatingSystem
    {
        //private static string _eventSourceName = "TraceViewerService";
        //private static string _logName = Assembly.GetExecutingAssembly().GetName().Name;
        internal static void SimulateInstall(string windowsEventFolder)
        {
           //  CheckEventLogExist();

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
                var commandArgs = string.Format("im {0} /rf:\"{1}\" /mf:\"{1}\"",
                    filename,
                    Path.Combine(windowsEventFolder, Path.GetFileNameWithoutExtension(filename) + ".dll"));

                // as a precaution uninstall the manifest.      
                Process.Start(new ProcessStartInfo("wevtutil.exe", "um" + commandArgs.Substring(2)) { Verb = "runAs" }).WaitForExit();
                Thread.Sleep(200);

                Process.Start(new ProcessStartInfo("wevtutil.exe", commandArgs) { Verb = "runAs" }).WaitForExit();
            }

            Thread.Sleep(1000);

        }

        //    }
        //}
        internal static void SimulateUninstall( string windowsEventFolder)
        {
            // run wevtutil elevated to unregister the ETW manifests
            // try
            //{
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
            //}
            //finally
            //{
            //    DeleteEventLog();

            //}


        }

        private static void CreateManifestFiles(string locationManifest)
        {
            Process.Start(new ProcessStartInfo($"{locationManifest}\\eventRegister.exe", $"{locationManifest}\\Core.Service.dll") { Verb = "runAs" }).WaitForExit();

        }
        //private static void CheckEventLogExist()
        //{

        //    EventLog eventLog = new EventLog(_logName);
        //    eventLog.Source = _eventSourceName;
        //    //eventLog.MaximumKilobytes = 8388608;//1GB

        //    if (!EventLog.SourceExists(_eventSourceName))
        //    {
        //        EventLog.CreateEventSource(_eventSourceName, _logName);

        //    }
        //}

        //private static void DeleteEventLog() {

        //    if (EventLog.SourceExists(_eventSourceName))
        //    {
        //        EventLog.DeleteEventSource(_eventSourceName);
        //    }

        //    if (EventLog.Exists(_logName))
        //    {
        //        EventLog.Delete(_logName);
    }
}
