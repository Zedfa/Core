using System;
using System.Collections.Generic;
using System.Diagnostics;
using Core.Cmn.Extensions;

namespace Core.EventLogs
{
    class Viewer
    {
        private Process RunCommandInWEVTUTIL(string args)
        {

            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "wevtutil.exe",
                    Arguments = args,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
            return proc;
        }
        public List<string> GetAllEventLogsName()
        {

            var eventLogsName = new List<string>();
            var proc = RunCommandInWEVTUTIL("el");
            proc.Start();
            while (!proc.StandardOutput.EndOfStream)
            {
                eventLogsName.Add(proc.StandardOutput.ReadLine());

            }

            return eventLogsName;
        }

        public List<LogEntry> GetEventLogData(string name)
        {
            var eventLogs = new List<LogEntry>();
            var proc = RunCommandInWEVTUTIL($"qe {name}");
            proc.Start();
            var events = proc.StandardOutput.ReadToEnd();
            var eventsWithRootElement = $"<Events > {events} </Events>";
            eventLogs = eventsWithRootElement.DeSerializeXMLToObject<Events>().Event; //Core.Cmn.Extensions.SerializationExtensions.DeSerializeXMLIntoObject<List< LogEntry>>(events)
            eventLogs.Reverse();

            return eventLogs; 
        }
    }
}
