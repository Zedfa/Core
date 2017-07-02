using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Cmn.Extensions;

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
