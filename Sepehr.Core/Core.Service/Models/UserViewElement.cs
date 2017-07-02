using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Models
{
    public class UserViewElement
    {
        //public string UserName { get; set; }
        public int UserId { get; set; }
        public IList<ViewElementInfo> ViewElements { get; set; }
    }
}
