using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Mvc.Helpers
{
   public interface IEssentialItem<T> where T : EssentialItem<T>
    {
       IList<T> Items { get; }
    }
}
