using Core.Cmn;
using Core.Entity;
using Core.Rep;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.IO;
using System.Configuration;
using Core.Rep.DTO;
using Core.Cmn.Attributes;

namespace Core.Service
{
    [Injectable(DomainName = "core", InterfaceType = typeof(ILogService))]
    public class LogService : ServiceBase<Log>, ILogService
    {
        #region Variable
        private ExceptionEventLogCreator _exceptionEventLogCreator;

        #endregion

        #region Constructors

        public LogService(IDbContextBase context)
            : base(context)
        {
            _exceptionEventLogCreator = new ExceptionEventLogCreator();
            _repositoryBase = new LogRepository(context);

        }
        public LogService()
            : base()
        {
            _exceptionEventLogCreator = new ExceptionEventLogCreator();
            _repositoryBase = new LogRepository();

        }

        #endregion
        #region method
        private static readonly object _locker = new object();
        private static readonly object _dbLocker = new object();
        public IQueryable<LogDTO> GetAllLogDTOs()
        {

            _repositoryBase = new LogRepository();
            var dtos = _repositoryBase.GetDtoQueryable(_repositoryBase.All().AsQueryable()) as IQueryable<LogDTO>;
            return dtos;
        }
        public ExceptionLogDTO GetExceptionLogOfCorrespondentLog(Guid logId)
        {
            LogRepository rep = new LogRepository();
            ExceptionLog excLog = rep.GetExceptionLog(logId);
            ExceptionLogDTO excLogDto = null;
            if (excLog != null)
            {

                excLogDto = new ExceptionLogDTO
                {
                    ID = excLog.ID,
                    Message = excLog.Message,
                    Source = excLog.Source,
                    ParentId = excLog.ID,
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

        private void LogToDbException(EventLog eventLog)//, string logFileName = "Log"
        {
            #if DEBUG
            if (eventLog.OccuredException == null)
            {
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }

            Console.WriteLine($"{eventLog.UserId}: {eventLog.CustomMessage} {eventLog.OccuredException?.Message} {DateTime.Now.ToLongTimeString()} {Environment.NewLine}");
            #endif

            var log = GenerateLog(eventLog);
            var repositoryBase = new LogRepository();
            //if (eventLog.OccuredException == null) return;
            repositoryBase.Create(log);
        }


        private Log GenerateLog(EventLog eventLog)
        {
            var log = new Log();
            var ex = eventLog.OccuredException;
            log.CustomMessage = eventLog.CustomMessage;
            log.UserId = eventLog.UserId;
            log.LogType = eventLog.LogType;
            log.InnerExceptionCount = 0;
            if (eventLog.CreateDate == null)
                log.CreateDate = DateTime.Now;
            else
                log.CreateDate = eventLog.CreateDate.Value;


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

        private void LogToFileException(Exception ex, string userId, string customMessage = "")
        {
            if (ex == null)
            { return; }

            //string assemblyPath = (new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase)).AbsolutePath;
            //string diretoryPath = assemblyPath.Remove(assemblyPath.LastIndexOf("/")) + "/Logs";
            string diretoryPath = Path.Combine(ConfigurationManager.AppSettings["LogPath"], "Logs");

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
        private void GenerateXmlDoc(Exception ex, string userId, string customMessage, XmlDocument doc)
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
        private void FillLogObjectList(Exception ex, List<LogObject> logObjects)
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
        #endregion

        public ExceptionLogDTO GetExceptionLogOfCorrespondentExceptionLog(Guid guid)
        {
            LogRepository rep = new LogRepository();
            ExceptionLog excLog = rep.GetExceptionLogOfThisParent(guid);
            ExceptionLogDTO excLogDto = null;
            if (excLog != null)
            {

                excLogDto = new ExceptionLogDTO
                {
                    ID = excLog.ID,
                    Message = excLog.Message,
                    Source = excLog.Source,
                    ParentId = guid,
                    StackTrace = excLog.StackTrace,
                    ExceptionType = excLog.ExceptionType,
                    HasChildren = rep.HasAnyChildren(guid)
                };
                return excLogDto;
            }
            else
            {
                return null;
            }
        }


        public void Handle(Exception ex, string logFileName, string customMessage)
        {
            Handle(ex, logFileName, "", customMessage);
        }
        public void Handle(Exception ex, string logFileName, string logUserId, string customMessage)
        {
            var errorLog = this.GetEventLogObj();
            errorLog.OccuredException = ex;
            errorLog.LogFileName = logFileName;
            errorLog.UserId = logUserId;
            errorLog.CustomMessage = customMessage;
            Handle(errorLog);
        }
        public void Handle(IEventLog el)
        {

            var eventLog = el as EventLog;
            try
            {
                //ToDo: DbContext should't be made new here!

                //if (_eventLog.LogEx)
                {
                    LogToDbException(eventLog);
                }
            }
            catch (Exception ex)
            {
                try
                {
                    ExceptionHandler.Handle(eventLog.OccuredException, eventLog.UserId, eventLog.CustomMessage);
                    ExceptionHandler.Handle(ex, eventLog.UserId, "خطا در هنگام ثبت لاگ در دیتابیس!");
                }
                catch
                {

                }
            }

            if (eventLog.ThrowEx)
            {
                throw eventLog.OccuredException;
            }
        }

        public void BatchHandle(List<EventLog> eventLogs)
        {
            try
            {
                BatchLogToDbException(eventLogs);
            }
            catch (Exception ex)
            {
                try
                {
                    var eventLog = new EventLog();
                    ExceptionHandler.Handle(ex, eventLog.UserId, "خطا در هنگام ثبت لاگ در دیتابیس!");
                }
                catch
                {

                }
            }
        }

        private void BatchLogToDbException(List<EventLog> eventLogs)
        {
            var repositoryBase = new LogRepository();
            foreach (var el in eventLogs)
            {
                repositoryBase.Create(GenerateLog(el), false);
            }

            repositoryBase.SaveChanges();
        }

        public EventLog GetEventLogObj()
        {
            return (EventLog)_exceptionEventLogCreator.BuildOccuredEvent();
        }


    }
}
