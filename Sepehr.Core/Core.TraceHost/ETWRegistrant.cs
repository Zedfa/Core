using Core.Cmn;
using Core.Cmn.Trace;
using Microsoft.Diagnostics.Tracing;
using Microsoft.Diagnostics.Tracing.Session;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
namespace Core.TraceHost
{
    internal class ETWRegistrant

    {
        protected internal static Dictionary<string, ConcurrentQueue<TraceDto>> TraceList { get; set; }

        private static Dictionary<string, long> WritersInfo { get; set; }

        public ETWRegistrant()
        {
            TraceList = new Dictionary<string, ConcurrentQueue<TraceDto>>();
            WritersInfo = new Dictionary<string, long>();
        }

        public void Setlistener(string EventSourceName)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    using (var session = new TraceEventSession("TraceViewerSession"))
                    {
                        session.Source.Dynamic.All += delegate (TraceEvent data)
                        {
                            if (!string.IsNullOrEmpty(data.FormattedMessage))
                                AddTraceEventToTraceList(data);
                        };

                        session.EnableProvider(EventSourceName);

                        Task.Delay(TimeSpan.FromSeconds(10)).Wait();

                        session.Source.Process();
                    }
                }
                catch (Exception exception)
                {
                    AddTraceToTraceList(new TraceDto
                    {
                        Message = exception.Message + "\t Details:" + exception?.InnerException?.Message,
                        Writer = "TraceHost",
                        TraceKey = "TraceHost",
                        Level = 1
                    });
                }
            },
                TaskCreationOptions.LongRunning);


        }

        public void SimulateInstall(string windowsEventFolder, string logName)
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
                //  var configArgs = $"sl {logName} /ms:{_maxLogSize}";

                // as a precaution uninstall the manifest.
                Process.Start(new ProcessStartInfo("wevtutil.exe", "um" + installCommandArgs.Substring(2)) { Verb = "runAs" }).WaitForExit();
                Thread.Sleep(2000);

                Process.Start(new ProcessStartInfo("wevtutil.exe", installCommandArgs) { Verb = "runAs" }).WaitForExit();
                //Process.Start(new ProcessStartInfo("wevtutil.exe", configArgs) { Verb = "runAs" }).WaitForExit();
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
            //if (proc.ExitCode > 0)
            //    throw new Exception($"core.mvc or eventRegister.exe is not available in this location:{locationManifest}");
        }

        private void AddTraceEventToTraceList(TraceEvent e)
        {
            var level = 2;
            switch (e.Level)
            {
                case TraceEventLevel.Always:
                case TraceEventLevel.Verbose:
                case TraceEventLevel.Informational:
                    level = 2;
                    break;

                case TraceEventLevel.Critical:
                case TraceEventLevel.Error:
                    level = 1;
                    break;

                case TraceEventLevel.Warning:
                    level = 3;
                    break;
            }
            var data = (string)e.PayloadByName("data");
            var trace = new TraceDto
            {
                Message = e.FormattedMessage,
                Level = level,
                TraceKey = (string)e.PayloadByName("traceKey") ?? string.Empty,
                Writer = (string)e.PayloadByName("writer") ?? string.Empty,
                Data = data != null ? Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(data) : null
            };

            AddTraceToTraceList(trace);

        }

        internal static void AddTraceToTraceList(TraceDto trace)
        {
            if (TraceList.ContainsKey(trace.Writer))
            {
                TraceList[trace.Writer].Enqueue(trace);
            }
            else
            {
                var traceWriterList = new ConcurrentQueue<TraceDto>();
                traceWriterList.Enqueue(trace);
                TraceList.Add(trace.Writer, traceWriterList);
            }

            CalculateWriterSize(trace);

            SparseWriter(trace.Writer);
        }

        private  static void CalculateWriterSize(TraceDto trace)
        {
           

            var traceSize = GetSizeOfTrace(trace);//Core.Cmn.Extensions.SerializationExtensions.SerializetoBinary(trace).LongLength;
            long writerSize;
            if (WritersInfo.TryGetValue(trace.Writer, out writerSize))
            {
                WritersInfo[trace.Writer] += traceSize;
            }
            else
            {
                WritersInfo.Add(trace.Writer, traceSize);
            }
        }
        private static void SparseWriter(string name)
        {

            if (WritersInfo[name] >= ConfigHelper.GetConfigValue<long>(GeneralConstant.TraceMaxSize))
            {
                TraceDto oldTrace;
                TraceList[name].TryDequeue(out oldTrace);
                WritersInfo[name] = WritersInfo[name] - GetSizeOfTrace(oldTrace);
            }

        }
        private static int GetSizeOfTrace(TraceDto trace) {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            MemoryStream memorystream = new MemoryStream();
           
            binaryFormatter.Serialize(memorystream, trace);
            return memorystream.ToArray().Length;
          
        }
        //private void ClearOldTraceEntity()
        //{
        //    TraceDto oldEvent;
        //    foreach (var writer in Writers)
        //    {
        //        var writerTraceList = TraceList.Select(t => t.Writer.Equals(writer));
        //        var currentTraceSize = Core.Cmn.Extensions.SerializationExtensions.SerializetoBinary(writerTraceList).LongLength;

        //        if (currentTraceSize >= AppBase.TraceWriter.MaxSize)
        //            for (int i = 0; i < 10000; i++)
        //            {
        //                TraceList.TryDequeue(out oldEvent);
        //            }
        //    }
        //}

        //private static PeriodicTaskFactory _stateTimer;

        //private void ManageTraceList()
        //{

        //    _stateTimer = new PeriodicTaskFactory((task) =>
        //    {
        //        ClearOldTraceEntity();
        //    },
        //   new TimeSpan(0, 10, 0),
        //   new TimeSpan(0, 10, 0));
        //    _stateTimer.Start();
        //}
    }
}

