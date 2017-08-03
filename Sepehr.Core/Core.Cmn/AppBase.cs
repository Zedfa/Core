using Core.Cmn.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
namespace Core.Cmn
{
    public class AppBase
    {
        private static bool _isApplicationStarted;
        private static object _lockObject = new object();

        public static event EventHandler UserLoginEvent;
        public static event EventHandler UserSignOutEvent;

        public static List<LogInfo> Logs { get; set; }
        public static Assembly StartupProject { get; private set; }
        public static ILogService LogService { get; set; }
        public static IDependencyInjectionManager DependencyInjectionManager { get; set; }
        private static DependencyInjectionFactory _dependencyInjectionFactory;
        public static DependencyInjectionFactory DependencyInjectionFactory
        {
            get
            {
                if (_dependencyInjectionFactory == null)
                    _dependencyInjectionFactory = new DependencyInjectionFactory();
                return _dependencyInjectionFactory;
            }
        }
        public static ITraceViewer TraceViewer { get; set; }


        static AppBase()
        {
            Logs = new List<LogInfo>();
            if (ConfigHelper.GetConfigValue<bool>("EnableLogFirstChanceException"))
                System.AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {


        }

        static void CurrentDomain_FirstChanceException(object sender,
            System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
        {
            //var domain = e.Exception.Source.Split('.')[0];
            // if (bool.Parse(ConfigHelper.GetConfigValue("EnableLogFirstChanceException")))

            if (e.Exception.Source != "Glimpse.Core" && e.Exception.Source != "mscorlib")
            {
                var eLog = new LogInfo();    //LogService.GetEventLogObj();
                eLog.OccuredException = e.Exception;
                eLog.CustomMessage = "It's basic Exception!";
                List<LogInfo> logs = null;
                lock (Logs)
                {
                    Logs.Add(eLog);
                    if (Logs.Count == 100)
                    {
                        logs = Logs.ToList();
                        Logs.Clear();
                    }
                }

                if (logs != null)

                    LogService.BatchHandle(logs);
            }
        }

        public static void OnAfterUserLogin(EventArgs e)
        {
            UserLoginEvent?.Invoke(null, e);
        }
        public static void OnAfterUserSignOut(EventArgs e)
        {
            UserSignOutEvent?.Invoke(null, e);
        }

        public static bool IsWebApp
        {
            get
            {
                return AppDomain.CurrentDomain.RelativeSearchPath != null;
            }
        }
        public static IEnumerable<Assembly> GetAssemblies()
        {

            ///chegini:ountori k man motavje shodam, dll ha dar iis hame load migardand va dar baghie mavared zaheran dll ha dar zamane niazeshan load migardand.
            ///chon ma mikhahim tamame dll haye app ro peymayesh konim man hameye dll ha ro dar code zir load mikonam. 
            string binPath;
            if (IsWebApp)
                binPath = AppDomain.CurrentDomain.RelativeSearchPath;
            else
                binPath = AppDomain.CurrentDomain.BaseDirectory;

            foreach (string dll in Directory.GetFiles(binPath, "*.dll", SearchOption.AllDirectories))
            {
                try
                {
                    Assembly loadedAssembly = Assembly.LoadFrom(dll);
                }
                catch
                { }
            }

            return AppDomain.CurrentDomain.GetAssemblies()
                .Where(a =>
               a.ManifestModule.Name != "<In Memory Module>"
                && !a.FullName.StartsWith("System")
                && !a.FullName.StartsWith("Microsoft")
                && a.Location.IndexOf("App_Web") == -1
                && a.Location.IndexOf("App_global") == -1
                && a.FullName.IndexOf("CppCodeProvider") == -1
                && a.FullName.IndexOf("WebMatrix") == -1
                && a.FullName.IndexOf("SMDiagnostics") == -1
                && a.FullName.IndexOf("Stimulsoft") == -1
                && !string.IsNullOrEmpty(a.Location)
                );
        }
        public static IList<Type> GetAlltypes()
        {
            List<Type> allTypes = new List<Type>();

            GetAssemblies().ToList().ForEach(
                        ass =>
                        {
                            try
                            {
                                allTypes.AddRange(ass.GetExportedTypes().ToList());
                            }
                            catch
                            {


                            }
                        }
                    );

            return allTypes;
        }

        private static List<Type> GetAllApplicationStart(IList<Type> allTypes)
        {
            Type iApplicationStartBaseType = typeof(ApplicationStartBase);
            return allTypes.Where(type => iApplicationStartBaseType.IsAssignableFrom(type) && !type.IsAbstract).ToList();
        }


        private static void LogMinThreads()
        {
            int workerThreads;
            int portThreads;

            System.Threading.ThreadPool.GetMinThreads(out workerThreads, out portThreads);

            //LogService.Handle(null, "", $"MinThreads: workerThreads({workerThreads}) portThreads({portThreads})");
            LogService.Write($"MinThreads: workerThreads({workerThreads}) portThreads({portThreads})");

        }

        private static void LogMaxThreads()
        {
            int workerThreads;
            int portThreads;

            System.Threading.ThreadPool.GetMaxThreads(out workerThreads, out portThreads);

            //LogService.Handle(null, "", $"MaxThreads: workerThreads({workerThreads}) portThreads({portThreads})");
            LogService.Write($"MaxThreads: workerThreads({workerThreads}) portThreads({portThreads})");
        }

        public static void StartApplication()
        {
            lock (_lockObject)
            {
                if (!_isApplicationStarted)
                {
                    Stopwatch s = new Stopwatch();
                    s.Start();
                    StartupProject = Assembly.GetCallingAssembly();
                    List<Type> allTypes = GetAlltypes().ToList();
                    UnityDependencyInjectionManager dependencyInjectionManager = new UnityDependencyInjectionManager(allTypes);
                    DependencyInjectionManager = dependencyInjectionManager;
                    List<ApplicationStartBase> applicationStartInstaces = GetAllApplicationStart(allTypes).Select(type => Activator.CreateInstance(type) as ApplicationStartBase)
                        .OrderBy(item => item.ExecutionPriorityBeforeApplicationStart).ToList();

                    applicationStartInstaces.ForEach(instanceType =>
                    {
                        instanceType.BeforeApplicationStart();
                    });

                    EntityInfo.BuildEntityInfoDic(allTypes);

                    applicationStartInstaces.OrderBy(i => i.ExecutionPriorityOnApplicationStart).ToList().ForEach(instance =>
                      {
                          instance.OnApplicationStart();

                      });

                    LogService.Write("Application started");
                    LogService.Write($"Is64BitProcess: {Environment.Is64BitProcess}");
                    LogService.Write($"ProcessorCount: {Environment.ProcessorCount}");
                    LogMinThreads();
                    LogMaxThreads();


                    _isApplicationStarted = true;
                    s.Stop();
                }
                else
                {
                    throw new Exception("Application can't start for second time.");
                }
            }
        }
    }
}
