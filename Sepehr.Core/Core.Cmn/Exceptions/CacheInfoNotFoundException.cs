using Core.Cmn.Exceptions;
using System;
using System.Runtime.Serialization;

namespace Core.Cmn.Cache
{
    [Serializable]
    public class CacheInfoNotFoundException : CoreExceptionBase
    {
        public CacheInfoNotFoundException(Delegate cacheFunc)
        {
            CacheFunc = cacheFunc;
        }
        protected CacheInfoNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }

        public override string Message
        {
            get
            {
                return $"CacheInfo not found for '{CacheFunc.Method.Name}' method cache! Usually this happen when 'Cacheble' attribute is missed on cache method or cache method is used in projects with wrong naming convention. Correct project naming convetion for using cache is *.Rep or *.Repository or *.Service.";
            }
        }
        public Delegate CacheFunc { get; private set; }
    }
}