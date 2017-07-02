using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Cmn.Attributes
{
    public class UnitTestAttribute: TestCategoryBaseAttribute
    {
        public override IList<string> TestCategories
        {
            get { return new List<string> { "UnitTest" }; }
        }
    }
}
