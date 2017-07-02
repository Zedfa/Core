using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity
{
    public interface  IUserProfile
    {
        int Id { get; set; }
        string UserName { get; set; }
        string Password { get; set; }
      // IUser User { get; set; }
    }
}
