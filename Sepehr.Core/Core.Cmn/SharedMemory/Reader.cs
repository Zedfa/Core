using Core.Cmn.SharedMemory;

namespace Core.Cmn.SharedMemory
{
    public class Reader<T>where  T : class
    {
        private static string Name { get; set; }
        private static long Size { get; set; }

        public SharedMemoryBase<T> Memory { get; private set; }

        private static Reader<T> _current;

        private static Reader<T> Current
        {
            get
            {
                if (_current == null)
                    _current = new Reader<T>(Name, Size);
                return _current;
            }
        }

        protected Reader(string name, long size )
        {

            Memory = new SharedMemoryBase<T>(name, size);
            if (!Memory.Open())
                throw new System.Exception("reader was not opened.check your available memory");
        }
          
        public static Reader<T> GetReader(string name, long size)
        {
            Name = name;
            Size = size;
            return Current;
        }
         
    }
}