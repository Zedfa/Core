using Core.Cmn;
using Core.Cmn.Attributes;
using Core.Entity;
using Core.Rep;
using Core.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.UnitTesting.Entity;
using System.Configuration;
using System.IO;

namespace Core.UnitTesting.Service
{
    [TestClass]
    public class LogServiceUnitTest : ServiceUnitTestBase<LogService, ILogRepository, Log>
    {
        private static readonly object _locker = new object();

        private EntityUnitTestHelperBase<Log> _entityUnitTestHelper;
        protected override EntityUnitTestHelperBase<Log> EntityUnitTestHelper
        {
            get
            {
                return _entityUnitTestHelper ?? (_entityUnitTestHelper = new LogUnitTestHelper());
            }
        }
        //public string LogPath
        //{
        //    get
        //    {
        //        return ConfigurationManager.AppSettings["LogPath"].ToString();
        //    }
        //}

        //public string ApplicationName
        //{
        //    get
        //    {
        //        return ConfigurationManager.AppSettings["ApplicationNameForLog"].ToString();
        //    }
        //}


        [TestMethod()]
        public override void ConstructorTest()
        {
            base.ConstructorTest();
        }
       
        protected override LogService ConstructService()
        {
            IDbContextBase ctx = Mock.MockHelperBase.BuildMockContext();

            return new LogService(ctx);
        }

        [TestMethod()]
        public override void AllTest()
        {
            base.AllTest();
        }





        [UnitTest]
        [TestMethod()]
        public void Write_InDB_True_Test()
        {
            var message = $"تست لاگ برداری در متد Write_InDB_True_Test در تاریخ {DateTime.Now.ToShortDateString()}";

            var service = ConstructService();

            // act
            service.Write(message);

            //assert
            var registeredLog = service.Filter(log => log.CustomMessage.Equals(message));
            Assert.IsTrue(registeredLog.Any());
        }

        [UnitTest]
        [TestMethod()]
        public void Write_InDB_False_Test()
        {
            var message = $"تست لاگ برداری در متد Write_InDB_False_Test در تاریخ {DateTime.Now.ToShortDateString()}";

            var service = ConstructService();

            // act
            service.Write(message);

            //assert
            var wrongMessage = $"تست لاگ برداری در متد Write در تاریخ {DateTime.Now.AddDays(-1).ToShortDateString()}";

            var registeredLog = service.Filter(log => log.CustomMessage.Equals(wrongMessage));
            Assert.IsFalse(registeredLog.Any());

        }
        [TestMethod]
        public void Handle_InDB_True_Test()
        {
            var message = $"تست لاگ برداری در متد Handle_InDB_True_Test در تاریخ {DateTime.Now.ToShortDateString()}";

            var service = ConstructService();

            // act
            service.Handle(new NullReferenceException(), message);

            //assert
            var registeredLog = service.Filter(log => log.CustomMessage.Equals(message));
            Assert.IsTrue(registeredLog.Any());
        }
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Handle_InDB_ThrowException_True_Test()
        {
            var message = $"تست لاگ برداری در متد Handle_InDB_ThrowException_True_Test در تاریخ {DateTime.Now.ToShortDateString()}";

            var service = ConstructService();

            // act
            service.Handle(new NullReferenceException(), message, true, "Core.UnitTest");

            //assert
            var registeredLog = service.Filter(log => log.CustomMessage.Equals(message));
            Assert.IsTrue(registeredLog.Any());
        }


        [TestMethod]
        public void Handle_InDB_IP_False_Test()
        {
            string ip = "192.168.1.49";
            var message = $"تست لاگ برداری در متد Handle_InDB_IP_True_Test در تاریخ {DateTime.Now.ToShortDateString()}";

            var service = ConstructService();

            // act
            service.Handle(ip, new NotImplementedException(), message, "Core.UnitTest");

            //assert
            var registeredLog = service.Filter(log => log.IP.Equals("localhost"));
            Assert.IsFalse(registeredLog.Any());

        }
        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void Handle_InDB_IP_ThrowException_True_Test()
        {
            string ip = "192.168.1.49";
            var message = $"تست لاگ برداری در متد Handle_InDB_IP_True_Test در تاریخ {DateTime.Now.ToShortDateString()}";

            var service = ConstructService();

            // act
            var exception = service.Handle(ip, new NotImplementedException(), message, "Core.UnitTest");

            //assert
            var registeredLog = service.Filter(log => log.IP.Equals(ip));
            Assert.IsTrue(registeredLog.Any());

            throw exception;
        }

        [TestMethod]
        public void Handle_InDB_False_Test()
        {
            var message = $"تست لاگ برداری در متد Handle_InDB_False_Test در تاریخ {DateTime.Now.ToShortDateString()}";

            var service = ConstructService();

            // act
            service.Handle(new NullReferenceException(), message);

            //assert
            var wrongMessage = $"تست لاگ برداری در متد Handle در تاریخ {DateTime.Now.AddDays(-1).ToShortDateString()}";
            var registeredLog = service.Filter(log => log.CustomMessage.Equals(wrongMessage));
            Assert.IsFalse(registeredLog.Any());
        }

        [TestMethod()]
        public void Handle_InXMLFile_Test()
        {

            var date = DateTime.Now;
            var message = $"تست لاگ برداری در متد Handle_InXMLFile_Test در تاریخ {DateTime.Now.ToShortDateString()}";

            var service = ConstructService();

            // act
            var logInfo = new LogInfo(source: null);
            logInfo.CustomMessage = message;
            logInfo.OccuredException = new NotSupportedException();
            service.Handle(logInfo);

            //assert

            Assert.IsTrue(File.Exists(service.XmlLogFileName) && File.GetLastWriteTime(service.XmlLogFileName) > date);

        }


        [TestMethod()]
        public void Handle_InXMLFile_InnerException6Level_Test()
        {

            var date = DateTime.Now;
            var message = $"تست لاگ برداری در متد Handle_InXMLFile_InnerException6Level_Test در تاریخ {DateTime.Now.ToShortDateString()}";

            var service = ConstructService();

            // act
            var logInfo = new LogInfo(source: null);
            logInfo.CustomMessage = message;
            logInfo.OccuredException = new Exception("First level Error",
                new NotSupportedException("Second level Error",
                new OverflowException("third level Error",
                new NotImplementedException("fourth level Error",
                new KeyNotFoundException("fifth level Error",
                new FieldAccessException("sixth level Error"))))));
            service.Handle(logInfo);

            //assert

            Assert.IsTrue(File.Exists(service.XmlLogFileName) && File.GetLastWriteTime(service.XmlLogFileName) > date);

        }
    }
}
