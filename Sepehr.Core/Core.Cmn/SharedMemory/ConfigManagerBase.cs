using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Cmn.SharedMemory
{
    internal class ConfigManagerBase : SharedMemoryBase<Config>
    {

        private const string Name = "MMFConfig";

        private const long Size = 20000000;  //20 MB

        private static ConfigManagerBase _current;

        private static ConfigManagerBase Current
        {
            get
            {
                if (_current == null)
                {
                    _current = new ConfigManagerBase();
                    _current.Open();
                }
                return _current;
            }
        }

        // public List<Config> Data { get; private set; }
        protected ConfigManagerBase() : base(Name, Size)
        {
           
        }
        public static ConfigManagerBase GetConfig()
        {
           
            return Current;
        }

        public void Write(List<Config> config)
        {
            Current.Data = config;
        }
        public List<Config> Read()
        {
            return Current.Data;
        }
       

    }
}
