using Core.Cmn.Trace;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.TraceHost
{
    public partial class ETWRegistrantService : ServiceBase
    {
        private static string _logName = Assembly.GetExecutingAssembly().GetName().Name;
        private ETWRegistrant _registrant;
        private static string DeploymentFolder
        {
            get
            {
                return System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), _logName);
            }
        }

        public string DefaultEventLog { get { return "TraceWriterService/Admin"; } }
        public string DefaultEventSource { get { return "TraceWriterService"; } }

        public ETWRegistrantService()
        {
            InitializeComponent();
            _registrant = new ETWRegistrant();
        }

        public void Start()
        {

            try
            {
                _registrant.SimulateInstall(DeploymentFolder, DefaultEventLog);

                _registrant.Setlistener(DefaultEventSource);

                Thread.Sleep(10000);

                TraceHostService.Start();
            }
            catch (Exception ex)
            {
                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Application";
                    eventLog.WriteEntry($"TraceHost Start Error: {ex.Message}{Environment.NewLine}{ex?.InnerException?.Message}", EventLogEntryType.Error, 101, 1);
                }
                throw;
            }
        }

        protected override void OnStart(string[] args)
        {
            Start();
        }

        protected override void OnStop()
        {
            //_registrant.SimulateUninstall(DeploymentFolder);
        }



    }
}
