using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mvc.Helpers
{
    [Serializable]
   public class Template
   {

       public string Name { get; set; }
       public int Width { get; set; }
       public int Height { get; set; }
    }
}
