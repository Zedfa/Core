using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Enum
{
    
    public enum LogType
    {
        [Description("درج")]
        Insert,
        [Description("حذف")]
        Delete,
        [Description("ویرایش")]
        Update,
        [Description("ورود")]
         Login,
        [Description("خروج")]
        Logout

    }
}
