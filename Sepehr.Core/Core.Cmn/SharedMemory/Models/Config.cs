using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Cmn.SharedMemory
{
    public class Config
    {
        public Config()
        {

        }
        public Config(string name, long size)
        {
            Size = size;
            Name = name;
        }
        
        public long Size { get; private set; }
        public string Name { get; private set; }
    }
}
