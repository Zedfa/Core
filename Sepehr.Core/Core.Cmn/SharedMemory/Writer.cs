using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Cmn.SharedMemory
{
    public class Writer<T> where T : class
    {

        private static string Name { get; set; }
        private static long Size { get; set; }

        public SharedMemoryBase<T> Memory { get; private set; }

        private static Writer<T> _current;

        private static Writer<T> Current
        {
            get
            {
                if (_current == null)
                    _current = new Writer<T>(Name, Size);
                return _current;
            }
        }

        protected Writer(string name, long size)
        {

            Memory = new SharedMemoryBase<T>(name, size);

            if (!Memory.Open())
                throw new System.Exception("writer was not opened.check your available memory");
            else
            {
                //add to sharedMemory config
                var configData = new List<Config>();

                configData.Add(new Config(Name, Size));

                var configBase = ConfigManagerBase.GetConfig();

                configBase.Write(configData);
            }
        }

        public static Writer<T> GetWriter(string name, long size)
        {
            Name = name;
            Size = size;
            return Current;
        }

    }
}
