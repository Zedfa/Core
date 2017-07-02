using Kendo.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Core.Mvc.Helpers
{
    public class JavaScriptInitializerCustom : JavaScriptInitializer
    {
        public override IJavaScriptSerializer CreateSerializer()
        {
            return new NewtonsoftScriptInitialize();
        }
        
    }
}
