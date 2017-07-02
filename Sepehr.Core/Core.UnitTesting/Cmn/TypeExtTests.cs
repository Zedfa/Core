using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.Cmn.Extensions;
using System.Drawing;
using System.Text;

namespace Core.UnitTesting.Cmn
{
    [TestClass]
    public class TypeExtTests
    {
        enum JustForTest
        {
            Test1 =100,
            Test2,
            Test3
        }
        [TestMethod]
        public void IsSimple_Test_AllSimple_Type_And_Some_Complex()
        {
            Assert.IsTrue(typeof(string).IsSimple());
            Assert.IsTrue(typeof(int).IsSimple());
            Assert.IsTrue(typeof(decimal).IsSimple());
            Assert.IsTrue(typeof(float).IsSimple());
            Assert.IsTrue(typeof(StringComparison).IsSimple());  // enum
            Assert.IsTrue(typeof(int?).IsSimple());
            Assert.IsTrue(typeof(decimal?).IsSimple());
            Assert.IsTrue(typeof(JustForTest).IsSimple());
            Assert.IsTrue(typeof(StringComparison?).IsSimple());
            Assert.IsTrue(typeof(StringComparison?).IsSimple());
            Assert.IsFalse(typeof(object).IsSimple());
            Assert.IsFalse(typeof(Point).IsSimple());  // struct
            Assert.IsFalse(typeof(Point?).IsSimple());
            Assert.IsFalse(typeof(StringBuilder).IsSimple()); // refer
        }
    }
}
