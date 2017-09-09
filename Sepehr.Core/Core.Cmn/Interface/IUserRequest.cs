using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Cmn
{
   public interface IUserRequest
    {
        string Url { get; set; }
        string Data { get; set; }
        string Method { get; set; }
        string IP { get; set; }
    }
}
