using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
   public interface ISharedMemoryService
    {
        void Dispose<T>(string sharedMemoryName) where T : class;
    }
}
