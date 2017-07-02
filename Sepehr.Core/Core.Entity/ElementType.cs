using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Enum
{
    public enum ElementType
    {
        [Description("منو")]
        Menu,
        [Description("اکشن")]
        Action,
        [Description("دکمه")]
        Button,
        [Description("صفحه")]
        Page
    }
}
