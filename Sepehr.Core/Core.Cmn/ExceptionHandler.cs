using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlServerCe;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace Core.Cmn
{
    public static class ExceptionHandler
    {
        private static Dictionary<string, Stopwatch> _stopwatchDic = new Dictionary<string, Stopwatch>();
        private static readonly object _locker = new object();
        private static readonly object _dbLocker = new object();

        public static void Handle(Exception exception, string userId, string customMessage = "", string logFileName = "Log", bool logEx = true, bool throwEx = false)
        {
            try
            {
                if (logEx)
                {
                    LogToDbException(exception, userId, customMessage, logFileName);
                }
            }
            catch (Exception ex)
            {
                try
                {
                    LogToFileException(exception, userId, customMessage);
                    LogToFileException(ex, userId);
                }
                catch
                { }
            }

            if (throwEx)
            {
                throw exception;
            }
        }

        private static void LogToDbException(Exception ex, string userId, string customMessage = "", string logFileName = "Log")
        {
            if (ex == null)
            { return; }

            if (String.IsNullOrEmpty(logFileName.Trim()))
            {
                logFileName = "Log";
            }

            XmlDocument doc = new XmlDocument();
            var root = doc.CreateElement("Logs");
            doc.AppendChild(root);

            GenerateXmlDoc(ex, userId, customMessage, doc);
            //string assemblyPath = (new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase)).AbsolutePath;
            //string dbfile = String.Concat(assemblyPath.Remove(assemblyPath.LastIndexOf("/")), "/", logFileName, ".sdf");
            string dbfile = Path.Combine(ConfigurationManager.AppSettings["LogPath"], logFileName + ".sdf");
            string connectionString = String.Format("Data Source={0};File Mode=read write;", dbfile);

            lock (_dbLocker)
            {
                if (File.Exists(dbfile))
                {
                    FileInfo fileInfo = new FileInfo(dbfile);
                    if (fileInfo.Length / 1024 / 1024 > 200)
                    {
                        File.Delete(dbfile);
                    }
                }

                if (!File.Exists(dbfile))
                {
                    CreateLogDB(dbfile, connectionString);
                }

                using (SqlCeConnection connection = new SqlCeConnection(connectionString))
                {
                    using (SqlCeCommand command = new SqlCeCommand("insert into Exception(Id, ExceptionText) values (@Val1, @val2)", connection))
                    {
                        command.CommandType = System.Data.CommandType.Text;
                        command.Parameters.AddWithValue("@Val1", Guid.NewGuid());
                        command.Parameters.AddWithValue("@Val2", doc.InnerXml);

                        //SqlCeEngine engine = new SqlCeEngine(connection.ConnectionString);
                        //engine.Upgrade();

                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        private static void CreateLogDB(string fileName, string connectionString)
        {
            try
            {
                SqlCeEngine en = new SqlCeEngine(connectionString);
                en.CreateDatabase();
                using (SqlCeConnection connection = new SqlCeConnection(connectionString))
                {
                    using (SqlCeCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "CREATE TABLE Exception(Id uniqueidentifier PRIMARY KEY, ExceptionText ntext)";
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
                System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(ex, true);
                System.Diagnostics.StackFrame[] frames = st.GetFrames();
                string x = "";
                // Iterate over the frames extracting the information you need
                foreach (System.Diagnostics.StackFrame frame in frames)
                {
                    //   x = ""+ frame.GetFileName()+"";
                        x += "    ** FileName:" + frame.GetFileName() + "** MethodName:" + frame.GetMethod().Name + "** LineNumber:" + frame.GetFileLineNumber() + "** ColumnNumber:" + frame.GetFileColumnNumber();
                }
                LogToFileException(ex,"CreateLogDB"+ x);
            }
        }

        private static void LogToFileException(Exception ex, string userId, string customMessage = "")
        {
            if (ex == null)
            { return; }

            //string assemblyPath = (new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase)).AbsolutePath;
            //string diretoryPath = assemblyPath.Remove(assemblyPath.LastIndexOf("/")) + "/Logs";
            string diretoryPath = Path.Combine(ConfigurationManager.AppSettings["LogPath"], "/Logs");

            if (!Directory.Exists(diretoryPath))
            {
                Directory.CreateDirectory(diretoryPath);
            }

            string fileName = Path.Combine(diretoryPath, String.Format("{0}_{1}.Xml", DateTime.Now.ToShortDateString().Replace("/", ""), userId));

            XmlDocument doc = new XmlDocument();
            if (File.Exists(fileName))
            {
                doc.Load(fileName);
            }
            else
            {
                var root = doc.CreateElement("Logs");
                doc.AppendChild(root);
            }

            lock (_locker)
            {
                GenerateXmlDoc(ex, userId, customMessage, doc);
                doc.Save(fileName);
            }
        }

        private static void GenerateXmlDoc(Exception ex, string userId, string customMessage, XmlDocument doc)
        {
            List<LogObject> logObjects = new List<LogObject>();
            FillLogObjectList(ex, logObjects);

            var xmlElement = (XmlElement)doc.DocumentElement.AppendChild(doc.CreateElement("Log"));
            xmlElement.SetAttribute("UserId", userId);

            for (int i = 0; i < logObjects.Count; i++)
            {
                if (i == 0)
                {
                    xmlElement.AppendChild(doc.CreateElement("ExceptionType")).InnerText = logObjects[i].ExceptionType;
                    xmlElement.AppendChild(doc.CreateElement("Message")).InnerText = logObjects[i].Message;
                    xmlElement.AppendChild(doc.CreateElement("StackTrace")).InnerText = logObjects[i].StackTrace;
                    xmlElement.AppendChild(doc.CreateElement("Source")).InnerText = logObjects[i].Source;
                    xmlElement.AppendChild(doc.CreateElement("CustomMessage")).InnerText = customMessage;
                    xmlElement.AppendChild(doc.CreateElement("InnerExceptionCount")).InnerText = (logObjects.Count - 1).ToString();
                    xmlElement.AppendChild(doc.CreateElement("CreateDateTime")).InnerText = DateTime.Now.ToString();
                }
                else if (i > 0)
                {
                    xmlElement.AppendChild(doc.CreateElement(String.Format("InnerExceptionType_{0}", i))).InnerText = logObjects[i].ExceptionType;
                    xmlElement.AppendChild(doc.CreateElement(String.Format("InnerMessage_{0}", i))).InnerText = logObjects[i].Message;
                    xmlElement.AppendChild(doc.CreateElement(String.Format("InnerStackTrace_{0}", i))).InnerText = logObjects[i].StackTrace;
                    xmlElement.AppendChild(doc.CreateElement(String.Format("InnerSource_{0}", i))).InnerText = logObjects[i].Source;
                }
            }
        }

        private static void FillLogObjectList(Exception ex, List<LogObject> logObjects)
        {
            logObjects.Add(new LogObject
            {
                Message = ex.Message,
                Source = ex.Source,
                StackTrace = ex.StackTrace,
                ExceptionType = ex.GetType().Name
            });

            if (ex.InnerException != null)
            {
                FillLogObjectList(ex.InnerException, logObjects);
            }
        }

        public static void StartWatch(string outputKey)
        {
            if (!_stopwatchDic.ContainsKey(outputKey))
            {
                _stopwatchDic[outputKey] = new Stopwatch();
            }
            _stopwatchDic[outputKey].Start();

        }

        public static void StopWatch(string outputKey)
        {
            _stopwatchDic[outputKey].Stop();
            LogToDbException(new Exception(String.Format("{0} : {1}", outputKey, _stopwatchDic[outputKey].ElapsedMilliseconds)), "StopWatch");
            Debug.WriteLine(String.Format("{0} : {1}", outputKey, _stopwatchDic[outputKey].ElapsedMilliseconds));
            _stopwatchDic.Remove(outputKey);

        }

        private static string GetParameterWithValue(List<LogParametersInfo> parameters)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var item in parameters)
            {
                sb.Append(String.Format("{0}: {1}\n", item.Name, item.Value));
            }

            return sb.ToString();
        }
    }
}
