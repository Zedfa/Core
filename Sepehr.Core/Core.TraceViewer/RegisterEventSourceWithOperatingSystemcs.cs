using System.Diagnostics;
using System.Reflection;

namespace Core.TraceViewer
{
    static class RegisterEventSourceWithOperatingSystem
    {

        private static string _logName = Assembly.GetExecutingAssembly().GetName().Name;

        public static void SimulateInstall(string eventSourceName)
        {
            EventLog eventLog = new EventLog(_logName);

            eventLog.Source = eventSourceName;

            if (!EventLog.SourceExists(eventSourceName))
            {
                EventLog.CreateEventSource(eventSourceName, _logName);

            }

        }

        /// <summary>
        /// Reverses the Install step 
        /// </summary>
        public static void SimulateUninstall(string eventSourceName)
        {
            if (EventLog.SourceExists(eventSourceName))
            {
                EventLog.DeleteEventSource(eventSourceName);
            }

            if (EventLog.Exists(_logName))
            {
                EventLog.Delete(_logName);

            }
        }
    }
}
