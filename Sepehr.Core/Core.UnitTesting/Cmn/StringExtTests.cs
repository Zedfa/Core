using Core.Cmn.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Core.Rep.Tests.Cmn
{
    [TestClass()]
    public class StringExtTests
    {
        [TestMethod]
        public void ExtractNumbers_Test_for_specific_string_is_equals()
        {
            Assert.AreEqual("1sdsd4h20".ExtractNumbers(), "1420");
        }
    }
}