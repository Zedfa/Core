using Kendo.Mvc;
using Kendo.Mvc.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Kendo.Mvc.Extensions;

namespace Core.Mvc.Helpers.CustomWrapper.Infrastructure
{
    public class JavaScriptGeneratorCr : JavaScriptInitializer
    {
        public override IJavaScriptSerializer CreateSerializer()
        {
            return new CustomJavaScriptSerializer();
        }
    }
}
