using Core.Cmn;
using Core.Cmn.Attributes;
using Core.Entity;
using Core.Rep;
using Core.Rep.DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml;

namespace Core.Service
{
    /// <summary>
    ///  1.zamani ke mikhaym chizi ro mahze etela dar log sabt konim az method e write estefade mikonim
    ///  2.zamani ke mikhaym exception dar log sabt beshe az method e handle estefade mikonim
    ///  3. faghat zamani ke mikhayd file, method , line , applicationName , source ro khodetoon poor konid az override Handle(LogInfo logInfo) estefade mikonim
    ///  4. log bardari be 2sorat anjam mishavad : sabt dar jadval logs , exceptionlogs dar sql  va agar natavanest sabt dar file xml(  makane save ro kilid e  LogPath dar config file moshakhas mikonad)
    /// </summary>
    /// <param name="customMessage"> too field e CustomMessage gharar migirad</param>
    /// <param name="raiseThrowException">agar true bashad throw exception raise mikonad </param>
    /// <param name="source"> az tarkibe meghdari ke developer mide-agar bedahad- + (file, method , line)</param>
    /// <param name="ip">too field e IP gharar migirad</param>
    /// <param name="file ,method,line"> har se motagheyer be sorate automatic poor mishe.aslan niaz be por kardan nadarad. </param>
    ///

    [Injectable(DomainName = "core", InterfaceType = typeof(ILogService))]
    public class LogService : ServiceBase<Log>, ILogService
    {
        private static readonly object _locker = new object();
        

        public LogService(IDbContextBase context)
                            : base(context)
        {
            _repositoryBase = new LogRepository(context);
        }

        public LogService()
            : base()
        {
            _repositoryBase = new LogRepository();
        }

        public string XmlLogFileName
        {
            get
            {
                return Path.Combine(ConfigurationManager.AppSettings["LogPath"],
                    $"{DateTime.Now.ToShortDateString().Replace("/", "")}_{ ConfigurationManager.AppSettings["ApplicationNameForLog"]}.Xml");
            }
        }
        public void BatchHandle(List<LogInfo> logs)
        {
            HandleBatchLogInfo(logs);
        }

        public IQueryable<LogDTO> GetAllLogDTOs()
        {
            _repositoryBase = new LogRepository();
            var dtos = _repositoryBase.GetDtoQueryable(_repositoryBase.All().AsQueryable()) as IQueryable<LogDTO>;
            return dtos;
        }

        public ExceptionLogDTO GetExceptionLogOfCorrespondentExceptionLog(int id)
        {
            LogRepository rep = new LogRepository();
            ExceptionLog excLog = rep.GetExceptionLogOfThisParent(id);
            ExceptionLogDTO excLogDto = null;
            if (excLog != null)
            {
                excLogDto = new ExceptionLogDTO
                {
                    Id = excLog.Id,
                    Message = excLog.Message,
                    Source = excLog.Source,
                    ParentId = id,
                    StackTrace = excLog.StackTrace,
                    ExceptionType = excLog.ExceptionType,
                    HasChildren = rep.HasAnyChildren(id)
                };
                return excLogDto;
            }
            else
            {
                return null;
            }
        }

        public ExceptionLogDTO GetExceptionLogOfCorrespondentLog(int logId)
        {
            LogRepository rep = new LogRepository();
            ExceptionLog excLog = rep.GetExceptionLog(logId);
            ExceptionLogDTO excLogDto = null;
            if (excLog != null)
            {
                excLogDto = new ExceptionLogDTO
                {
                    Id = excLog.Id,
                    Message = excLog.Message,
                    Source = excLog.Source,
                    ParentId = excLog.Id,
                    StackTrace = excLog.StackTrace,
                    ExceptionType = excLog.ExceptionType,
                    HasChildren = rep.HasAnyChildren(logId)
                };
                return excLogDto;
            }
            else
            {
                return null;
            }
        }

        //public Exception Handle(string IP, Exception ex, string customMessage, [CallerFilePath] string file = null, [CallerMemberName] string method = null, [CallerLineNumber] int line = 0)
        //{
        //    var info = new LogInfo($"File: {file} , Method: {method} , Line: {line}");
        //    info.OccuredException = ex;
        //    info.CustomMessage = customMessage;
        //    info.Request.IP = IP;
        //    HandleLogInfo(info);
        //    return ex;
        //}

        public Exception Handle(Exception ex, string customMessage, string source, [CallerFilePath] string file = null, [CallerMemberName] string method = null, [CallerLineNumber] int line = 0)
        {
            var info = new LogInfo($"{source}  .File: {file} , Method: {method} , Line: {line}");
            info.OccuredException = ex;
            info.CustomMessage = customMessage;
            HandleLogInfo(info);
            return ex;
        }
        

        public Exception Handle(Exception ex, string customMessage, bool raiseThrowException, string source, string platform, [CallerFilePath] string file = null, [CallerMemberName] string method = null, [CallerLineNumber] int line = 0)
        {
            var info = new LogInfo($"{source}  .File: {file} , Method: {method} , Line: {line}");
            info.OccuredException = ex;
            info.CustomMessage = customMessage;
            info.RaiseThrowException = raiseThrowException;
            info.Platform = platform;
            HandleLogInfo(info);
            return ex;
        }

        public Exception Handle(Exception ex, string customMessage, bool raiseThrowException, string source, [CallerFilePath] string file = null, [CallerMemberName] string method = null, [CallerLineNumber] int line = 0)
        {
            var info = new LogInfo($"{source}  .File: {file} , Method: {method} , Line: {line}");
            info.OccuredException = ex;
            info.CustomMessage = customMessage;
            info.RaiseThrowException = raiseThrowException;
            HandleLogInfo(info);
            return ex;
        }

        public Exception Handle(Exception ex, string customMessage, [CallerFilePath] string file = null, [CallerMemberName] string method = null, [CallerLineNumber] int line = 0)
        {
            var info = new LogInfo($"File: {file} , Method: {method} , Line: {line}");
            info.OccuredException = ex;
            info.CustomMessage = customMessage;
            HandleLogInfo(info);
            return ex;
        }

        public Exception Handle(LogInfo info)
        {
            HandleLogInfo(info);
            return info.OccuredException;
        }

        public void Write(string customMessage, [CallerFilePath] string file = null, [CallerMemberName] string method = null, [CallerLineNumber] int line = 0)
        {
            var info = new LogInfo($"File: {file} , Method: {method} , Line: {line}");
            info.CustomMessage = customMessage;
            HandleLogInfo(info);
        }

        //public void Write(string customMessage, string ip, [CallerFilePath] string file = null, [CallerMemberName] string method = null, [CallerLineNumber] int line = 0)
        //{
        //    var info = new LogInfo($"File: {file} , Method: {method} , Line: {line}");
        //    info.CustomMessage = customMessage;
        //    info.Request.IP = ip;
        //    HandleLogInfo(info);
        //}

        private void FillLogObjectList(LogInfo logInfo, List<LogInfo> logInfoList)
        {

            logInfoList.Add(logInfo);

            if (logInfo.OccuredException != null && logInfo.OccuredException.InnerException != null)
            {
                var childLog = new LogInfo(logInfo.Source);
                childLog.OccuredException = logInfo.OccuredException.InnerException;
                childLog.CustomMessage = logInfo.CustomMessage;
                childLog.Platform = logInfo.Platform;
                FillLogObjectList(childLog, logInfoList);
            }
        }

        private  void GenerateXmlElement(LogInfo logInfo, XmlDocument doc)
        {
            List<LogInfo> logObjects = new List<LogInfo>();
            FillLogObjectList(logInfo, logObjects);

            var xmlElement = (XmlElement)doc.DocumentElement.AppendChild(doc.CreateElement("Log"));
            xmlElement.SetAttribute("ApplicationName", logInfo.ApplicationName);

            for (int i = 0; i < logObjects.Count; i++)
            {
                if (i == 0)
                {


                    xmlElement.AppendChild(doc.CreateElement("Source")).InnerText = logObjects[i].Source;
                    xmlElement.AppendChild(doc.CreateElement("CustomMessage")).InnerText = logInfo.CustomMessage + "." + logInfo.OccuredException?.Message;
                    xmlElement.AppendChild(doc.CreateElement("InnerExceptionCount")).InnerText = (logObjects.Count - 1).ToString();
                    xmlElement.AppendChild(doc.CreateElement("CreateDateTime")).InnerText = DateTime.Now.ToString();
                    xmlElement.AppendChild(doc.CreateElement("IP")).InnerText = logObjects[i].Request?.IP;
                    xmlElement.AppendChild(doc.CreateElement("Data")).InnerText = logObjects[i].Request?.Data;
                    xmlElement.AppendChild(doc.CreateElement("Url")).InnerText = logObjects[i].Request?.Url;
                    xmlElement.AppendChild(doc.CreateElement("Method")).InnerText = logObjects[i].Request?.Method;
                    xmlElement.AppendChild(doc.CreateElement("Platform")).InnerText = logObjects[i].Platform;
                    xmlElement.AppendChild(doc.CreateElement(String.Format("ExceptionType", i))).InnerText = logObjects[i].OccuredException?.GetType().Name;

                }
                else if (i > 0)
                {
                    xmlElement.AppendChild(doc.CreateElement(String.Format("ExceptionType_{0}", i))).InnerText = logObjects[i].OccuredException?.GetType().Name;
                    xmlElement.AppendChild(doc.CreateElement(String.Format("InnerMessage_{0}", i))).InnerText = logObjects[i].OccuredException?.InnerException?.Message;
                    xmlElement.AppendChild(doc.CreateElement(String.Format("InnerStackTrace_{0}", i))).InnerText = logObjects[i].OccuredException?.StackTrace;
                    xmlElement.AppendChild(doc.CreateElement(String.Format("InnerSource_{0}", i))).InnerText = logObjects[i].OccuredException?.Source;
                    xmlElement.AppendChild(doc.CreateElement("CreateDateTime")).InnerText = DateTime.Now.ToString();
                    xmlElement.AppendChild(doc.CreateElement("IP")).InnerText = logObjects[i].Request?.IP;
                    xmlElement.AppendChild(doc.CreateElement("Data")).InnerText = logObjects[i].Request?.Data;
                    xmlElement.AppendChild(doc.CreateElement("Url")).InnerText = logObjects[i].Request?.Url;
                    xmlElement.AppendChild(doc.CreateElement("Method")).InnerText = logObjects[i].Request?.Method;
                    xmlElement.AppendChild(doc.CreateElement("Platform")).InnerText = logObjects[i].Platform;
                }
            }
        }

        private void LogToFileException(LogInfo logInfo)
        {

            string diretoryPath = ConfigurationManager.AppSettings["LogPath"];

            if (!Directory.Exists(diretoryPath))
            {
                Directory.CreateDirectory(diretoryPath);
            }

           // string fileName = Path.Combine(diretoryPath, String.Format("{0}_{1}.Xml", DateTime.Now.ToShortDateString().Replace("/", ""), logInfo.ApplicationName));

            XmlDocument doc = new XmlDocument();
            if (File.Exists(XmlLogFileName))
            {
                doc.Load(XmlLogFileName);
            }
            else
            {
                var root = doc.CreateElement("Logs");
                doc.AppendChild(root);
            }

            lock (_locker)
            {
                GenerateXmlElement(logInfo, doc);
                doc.Save(XmlLogFileName);
            }
        }

        private void LogToFileException(List<LogInfo> logInfoList)
        {
            if (logInfoList.Any(log => log.OccuredException != null))
            {

                string diretoryPath = Path.Combine(ConfigurationManager.AppSettings["LogPath"], "/Logs");

                if (!Directory.Exists(diretoryPath))
                {
                    Directory.CreateDirectory(diretoryPath);
                }

                string fileName = Path.Combine(diretoryPath, String.Format("{0}_{1}.Xml", DateTime.Now.ToShortDateString().Replace("/", ""), logInfoList[0].ApplicationName));

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
                    foreach (var logInfo in logInfoList)
                    {
                        GenerateXmlElement(logInfo, doc);

                    }
                    doc.Save(fileName);
                }
            }

        }

        private void FillInnerException(Exception ex, ExceptionLog innerExceptionLog, ExceptionLog pExceptionLog, ref int i)
        {
            innerExceptionLog.ExceptionType = ex.GetType().Name;
            innerExceptionLog.Message = ex.Message;
            innerExceptionLog.StackTrace = ex.StackTrace;
            innerExceptionLog.Source = ex.Source;
            if (pExceptionLog != null)
            {
                pExceptionLog.InnerException = innerExceptionLog;
                i = ++i;
            }

            if (ex.InnerException != null)
            {
                innerExceptionLog.InnerException = new ExceptionLog();
                FillInnerException(ex.InnerException, innerExceptionLog.InnerException, innerExceptionLog, ref i);
            }
        }



        private Log GenerateLog(LogInfo eventLog)
        {
            var log = new Log();
            var ex = eventLog.OccuredException;
            log.CustomMessage = eventLog.CustomMessage;
            log.ApplicationName = eventLog.ApplicationName;
            log.InnerExceptionCount = 0;
            log.ClientPlatform = eventLog.Platform;
            log.CreateDate = DateTime.Now;
            log.Source = eventLog.Source;
            if (eventLog.Request != null )
            {
                log.Request = new Request
                {                   
                    Url = eventLog.Request.Url,
                    Data = eventLog.Request.Data,
                    Method = eventLog.Request.Method
                };

                try
                {
                    log.Request.IP = eventLog.Request.IP;
                }
                catch
                {
                    // It may Request is not ready to get IP throw exception!
                }
            }
            
            if (ex != null)
            {
                ExceptionLog exceptionLog = new ExceptionLog();
                ExceptionLog innerException = null;
                exceptionLog.ExceptionType = ex.GetType().Name;
                exceptionLog.Message = ex.Message;
                exceptionLog.StackTrace = ex.StackTrace;
                exceptionLog.Source = ex.Source;
                if (ex.InnerException != null)
                {
                    int i = 1;
                    innerException = new ExceptionLog();
                    FillInnerException(ex.InnerException, innerException, null, ref i);
                    log.InnerExceptionCount = i;
                }

                exceptionLog.InnerException = innerException;
                log.ExceptionLog = exceptionLog;
            }
            return log;
        }



        private void HandleBatchLogInfo(List<LogInfo> info)
        {
            try
            {
                LogToDbException(info);
            }
            catch (Exception ex)
            {
                try
                {

                    LogToFileException(info);
                    var log = new LogInfo();
                    log.OccuredException = ex;
                    log.CustomMessage = "خطا در هنگام ثبت batch log  در دیتابیس!";
                    LogToFileException(log);

                }
                catch
                {
                }
            }


        }

        private void HandleLogInfo(LogInfo info)
        {
            try
            {                 
                info.Request = DependencyInjectionFactory.TryToResolveIRequest()?.UserRequest;
                LogToDbException(info);
            }
            catch (Exception ex)
            {
                try
                {
                   
                    LogToFileException(info);
                    var log = new LogInfo();
                    log.OccuredException = ex;
                    log.CustomMessage = "خطا در هنگام ثبت لاگ در دیتابیس!";
                  
                    LogToFileException(log);

                }
                catch
                {
                }
            }

            if (info.RaiseThrowException)
            {
                throw info.OccuredException;
            }
        }
        private void LogToDbException(LogInfo logInfo)
        {
            if (logInfo.Source == null) throw new NotSupportedException("you must set Source");
            
            WriteInConsole(logInfo);
            var log = GenerateLog(logInfo);
            // hatman bayad new shavad choon dbcontext nemitavanad thread safe bashad
            var repositoryBase = new LogRepository();
            repositoryBase.Create(log);
        }

        private void LogToDbException(List<LogInfo> logInfoList)
        {
            var logs = new List<Log>();
            foreach (var logInfo in logInfoList)
            {

                WriteInConsole(logInfo);

                logs.Add(GenerateLog(logInfo));
            }

            var repositoryBase = new LogRepository();
            repositoryBase.Create(logs);
        }

        private void WriteInConsole(LogInfo logInfo)
        {

#if DEBUG
            if (logInfo.OccuredException == null)
            {
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }

            Console.WriteLine($"{logInfo.ApplicationName}: {logInfo.CustomMessage} {logInfo.OccuredException?.Message} {DateTime.Now.ToLongTimeString()} {Environment.NewLine}");
#endif

        }
    }
}