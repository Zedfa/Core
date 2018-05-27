using Core.Cmn.DataSource;
using Core.Cmn.Exceptions;
using Core.Cmn.Monitoring;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Core.UnitTesting.Cmn.EntityInfoTests;

namespace Core.UnitTesting.Cmn
{
    [TestClass()]
    public class DataSourceTests
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidFilterOnIndexablePropertyException))]
        public void FirstMethod_InvalidFilterException_Test()
        {
            List<User> lst;
            var count = 100;
            lst = Init(count);
            var ds = new DataSource<User>(lst);
            var users = ds.ApplyFilter(item => item.LName, "System").ApplyFilter(item => item.Id, 123).ApplyFilter(item => item.FName, "Sepehr").ApplyFilter(item => item.Email, "somthings").First();
        }
        [TestMethod]
        public void All_Test1()
        {
            List<User> lst;
            var count = 4;
            lst = new List<User>();
            for (int i = 0; i < count; i++)
            {
                lst.Add(new Cmn.EntityInfoTests.User { Id = i, FName = "Sepehr", CompanyChartId = 123456789, LName = "System", UserProfile = new Core.Entity.UserProfile() { Id = 10 } });
            }
            var ds = new DataSource<User>(lst);
            var users = ds.ApplyFilter(item => item.Id, 0).First();
            var users2 = ds.ApplyFilter(item => item.Id, 3).First();
            //Assert.AreEqual(users.First().LName, users2.First().LName);
            //Assert.AreEqual(users.First().Id, users2.First().Id);
            //Assert.AreEqual(users.First().FName, users2.First().FName);
            //Assert.AreEqual(users.Count(), users2.Count);
        }


        [TestMethod]
        public void First_Test()
        {
            for (int i = 0; i < 10; i++)
            {
                List<User> lst;
                var count = 100000;
                lst = Init(count);
                lst.Insert(count / 2, new Cmn.EntityInfoTests.User { Id = 123, FName = "Sepehr", CompanyChartId = 123456789, LName = "System", UserProfile = new Core.Entity.UserProfile() { Id = 10 } });
                lst.Insert(count / 2, new Cmn.EntityInfoTests.User { Id = 123, FName = "Sepehr", CompanyChartId = 123456789, LName = "System", UserProfile = new Core.Entity.UserProfile() { Id = 10 } });
                lst.Insert(count / 2, new Cmn.EntityInfoTests.User { Id = 123, FName = "Sepehr", CompanyChartId = 123456789, LName = "System", UserProfile = new Core.Entity.UserProfile() { Id = 10 } });
                lst.Insert(count / 2, new Cmn.EntityInfoTests.User { Id = 123, FName = "Sepehr" });
                lst.Insert(count / 2, new Cmn.EntityInfoTests.User { Id = 123, FName = "www" });
                var ds = new DataSource<User>(lst, true);
                var ds1 = new DataSource<User>(lst, true);
                var users = ds.ApplyFilter(item => item.LName, "System").ApplyFilter(item => item.Id, 123).ApplyFilter(item => item.FName, "Sepehr").First();
                var users2 = lst.First(item => item.LName == "System" && item.Id == 123 && item.FName == "Sepehr");
                Assert.AreEqual(users.LName, users2.LName);
                Assert.AreEqual(users.Id, users2.Id);
                Assert.AreEqual(users.FName, users2.FName);
            }
        }

        [TestMethod]
        public void TestForFirstAndLastItems()
        {
            List<User> lst;
            var count = 4;
            lst = new List<User>();
            for (int i = 0; i < count; i++)
            {
                lst.Add(new Cmn.EntityInfoTests.User { Id = i, FName = "Sepehr", CompanyChartId = 123456789, LName = "System", UserProfile = new Core.Entity.UserProfile() { Id = 10 } });

            }
            var ds = new DataSource<User>(lst);
            var users = ds.ApplyFilter(item => item.Id, 0).First();
            var users2 = ds.ApplyFilter(item => item.Id, 3).First();
            //Assert.AreEqual(users.First().LName, users2.First().LName);
            //Assert.AreEqual(users.First().Id, users2.First().Id);
            //Assert.AreEqual(users.First().FName, users2.First().FName);
            //Assert.AreEqual(users.Count(), users2.Count);
        }

        [TestMethod]
        public void All_Test()
        {
            for (int i = 0; i < 10; i++)
            {
                List<User> lst;
                var count = 100000;
                lst = Init(count);

                lst.Insert(count / 2, new Cmn.EntityInfoTests.User { Id = 123, FName = "Sepehr", CompanyChartId = 123456789, LName = "System", UserProfile = new Core.Entity.UserProfile() { Id = 10 } });
                lst.Insert(count / 2, new Cmn.EntityInfoTests.User { Id = 123, FName = "Sepehr", CompanyChartId = 123456789, LName = "System", UserProfile = new Core.Entity.UserProfile() { Id = 10 } });
                lst.Insert(count / 2, new Cmn.EntityInfoTests.User { Id = 123, FName = "Sepehr", CompanyChartId = 123456789, LName = "System", UserProfile = new Core.Entity.UserProfile() { Id = 10 } });
                lst.Insert(count / 2, new Cmn.EntityInfoTests.User { Id = 123, FName = "Sepehr" });
                lst.Insert(count / 2, new Cmn.EntityInfoTests.User { Id = 123, FName = "www" });
                var ds = new DataSource<User>(lst);
                var users = ds.ApplyFilter(item => item.LName, "System").ApplyFilter(item => item.Id, 123).ApplyFilter(item => item.FName, "Sepehr").All();
                var users2 = lst.Where(item => item.LName == "System" && item.Id == 123 && item.FName == "Sepehr").ToList();
                Assert.AreEqual(users.First().LName, users2.First().LName);
                Assert.AreEqual(users.First().Id, users2.First().Id);
                Assert.AreEqual(users.First().FName, users2.First().FName);
                Assert.AreEqual(users.Count(), users2.Count);
            }
        }
        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void IndexOutOfRangeException_When_IndexOrder_Is_Not_Valid_Test()
        {
            List<ObjectT_est_For_Out_of_Range_Exception> lst = new List<ObjectT_est_For_Out_of_Range_Exception>();
            lst.Add(new ObjectT_est_For_Out_of_Range_Exception { });
            var w = new WatchElapsed();
            var ds = new DataSource<ObjectT_est_For_Out_of_Range_Exception>(lst);
            var users = ds.ApplyFilter(item => item.LName, "System").ApplyFilter(item => item.Id, 123).ApplyFilter(item => item.FName, "Sepehr").All();
        }
        [TestMethod]
        public void Elapsed_Time_For_AllMethod_VS_Linq_Test()
        {
            List<User> lst;
            var count = 500000;
            lst = Init(count);
            lst.Insert(count / 2, new Cmn.EntityInfoTests.User { Id = 123, FName = "Sepehr", CompanyChartId = 123456789, LName = "System", UserProfile = new Core.Entity.UserProfile() { Id = 10 } });
            lst.Insert(count / 2, new Cmn.EntityInfoTests.User { Id = 123, FName = "Sepehr", CompanyChartId = 123456789, LName = "System", UserProfile = new Core.Entity.UserProfile() { Id = 10 } });
            lst.Insert(count / 2, new Cmn.EntityInfoTests.User { Id = 123, FName = "Sepehr", CompanyChartId = 123456789, LName = "System", UserProfile = new Core.Entity.UserProfile() { Id = 10 } });
            lst.Insert(count / 2, new Cmn.EntityInfoTests.User { Id = 123, FName = "Sepehr" });
            lst.Insert(count / 2, new Cmn.EntityInfoTests.User { Id = 123, FName = "www" });

            var w = new WatchElapsed();
            var ds = new DataSource<User>(lst);
            using (new Watch(out w))
            {
                for (int i = 0; i < 1000; i++)
                {
                    var users = ds.ApplyFilter(item => item.LName, "System").ApplyFilter(item => item.Id, 123).ApplyFilter(item => item.FName, "Sepehr").All();
                }
            }

            System.Diagnostics.Debug.WriteLine(w.ElapsedMilliseconds);
            var w1 = new WatchElapsed();
            using (new Watch(out w1))
            {
                for (int i = 0; i < 1000; i++)
                {
                    var user = lst.Where(item => item.LName == "System" && item.Id == 123 && item.FName == "Sepehr").ToList();
                }
            }

            System.Diagnostics.Debug.WriteLine(w1.ElapsedMilliseconds);
            Assert.IsTrue(w1.ElapsedMilliseconds > w.ElapsedMilliseconds * 200);
        }


        [TestMethod]
        public void test()
        {
            List<User> lst;
            var count = 500000;
            lst = Init(count);
            lst.Insert(count / 2, new Cmn.EntityInfoTests.User { Id = 123, FName = "Sepehr", CompanyChartId = 123456789, LName = "System", UserProfile = new Core.Entity.UserProfile() { Id = 10 } });
            lst.Insert(count / 2, new Cmn.EntityInfoTests.User { Id = 123, FName = "Sepehr", CompanyChartId = 123456789, LName = "System", UserProfile = new Core.Entity.UserProfile() { Id = 10 } });
            lst.Insert(count / 2, new Cmn.EntityInfoTests.User { Id = 123, FName = "Sepehr", CompanyChartId = 123456789, LName = "System", UserProfile = new Core.Entity.UserProfile() { Id = 10 } });
            lst.Insert(count / 2, new Cmn.EntityInfoTests.User { Id = 123, FName = "Sepehr" });
            var xx = new Cmn.EntityInfoTests.User { Id = 123, FName = "www" };
            lst.Insert(count / 2, xx);

            var w = new WatchElapsed();
            using (new Watch(out w))
            {
                for (int i = 0; i < 1000; i++)
                {
                    lst.RemoveAt(8594);
                }
            }

            System.Diagnostics.Debug.WriteLine(w.ElapsedMilliseconds);
            var w1 = new WatchElapsed();
            using (new Watch(out w1))
            {
                for (int i = 0; i < 1000; i++)
                {
                    var user = lst.Where(item => item.LName == "System" && item.Id == 123 && item.FName == "Sepehr").ToList();
                }
            }

            System.Diagnostics.Debug.WriteLine(w1.ElapsedMilliseconds);
            Assert.IsTrue(w1.ElapsedMilliseconds > w.ElapsedMilliseconds * 20);
        }
        private static List<User> Init(int count)
        {
            var lst = new List<User>();
            for (int i = 0; i < count; i++)
            {
                lst.Add(User.BuildSampleEntity<User>());
            }
            return lst;
        }
    }
}
