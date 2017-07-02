using System;
using System.Collections.Generic;

namespace Core.Ef
{
    public class EntityChangeEventArgs<T> : EventArgs
    {
        public IEnumerable<T> Results { get; set; }
        public bool ContinueListening { get; set; }
    }
}
