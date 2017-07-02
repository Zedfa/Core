using Core.Cmn.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Cmn.Cache
{
    [DataContract]
    public class FunctionalCacheDataProvider<T> : CacheDataProvider<T>
    {
        public Func<T> Func { get; set; }
        public FunctionalCacheDataProvider(CacheInfo chacheInfo)
            : base(chacheInfo)
        {
            Func = chacheInfo.Func as Func<T>;
        }

        public override void SetFunc(object func)
        {
            Func = func as Func<T>;
        }

        protected override T ExcecuteCacheMethod()
        {
            return Func.Invoke();
        }

    }
    [DataContract]
    public class FunctionalCacheDataProvider<P1, T> : CacheDataProvider<T>
    {
        public Func<P1, T> Func { get; set; }

        [DataMember]
        public P1 Param1 { get; set; }
        public FunctionalCacheDataProvider(CacheInfo chacheInfo, Func<P1, T> func, P1 param1)
            : base(chacheInfo)
        {
            Func = func;
            Param1 = param1;
        }
        protected override T ExcecuteCacheMethod()
        {
            return Func.Invoke(Param1);
        }

        public override string GenerateCacheKey()
        {
            return string.Format("{0}_{1}", CacheInfo.BasicKey, Param1); ;
        }
    }

    [DataContract]
    public class FunctionalCacheDataProvider<P1, P2, T> : CacheDataProvider<T>
    {
        public Func<P1, P2, T> Func { get; set; }
        [DataMember]
        public P1 Param1 { get; set; }
        [DataMember]
        public P2 Param2 { get; set; }
        public FunctionalCacheDataProvider(CacheInfo chacheInfo, Func<P1, P2, T> func, P1 param1, P2 param2)
            : base(chacheInfo)
        {
            Func = func;
            Param1 = param1;
            Param2 = param2;
        }
        public override string GenerateCacheKey()
        {
            return string.Format("{0}_{1}_{2}", CacheInfo.BasicKey, Param1, Param2);
        }
        public override void SetFunc(object func)
        {
            Func = func as Func<P1, P2, T>;

        }
        protected override T ExcecuteCacheMethod()
        {
            return Func.Invoke(Param1, Param2);
        }
    }

    [DataContract]
    public class FunctionalCacheDataProvider<P1, P2, P3, T> : CacheDataProvider<T>
    {
        public Func<P1, P2, P3, T> Func { get; set; }
        [DataMember]
        public P1 Param1 { get; set; }
        [DataMember]
        public P2 Param2 { get; set; }
        [DataMember]
        public P3 Param3 { get; set; }
        public FunctionalCacheDataProvider(CacheInfo chacheInfo, Func<P1, P2, P3, T> func, P1 param1, P2 param2, P3 param3)
            : base(chacheInfo)
        {
            Func = func;
            Param1 = param1;
            Param2 = param2;
            Param3 = param3;
        }
        protected override T ExcecuteCacheMethod()
        {
            return Func.Invoke(Param1, Param2, Param3);
        }

        public override string GenerateCacheKey()
        {
            return string.Format("{0}_{1}_{2}_{3}", CacheInfo.BasicKey, Param1, Param2, Param3); ;
        }
    }

    [DataContract]
    public class FunctionalCacheDataProvider<P1, P2, P3, P4, T> : CacheDataProvider<T>
    {
        public Func<P1, P2, P3, P4, T> Func { get; set; }
        [DataMember]
        public P1 Param1 { get; set; }
        [DataMember]
        public P2 Param2 { get; set; }
        [DataMember]
        public P3 Param3 { get; set; }
        [DataMember]
        public P4 Param4 { get; set; }
        public FunctionalCacheDataProvider(CacheInfo chacheInfo, Func<P1, P2, P3, P4, T> func, P1 param1, P2 param2, P3 param3, P4 param4)
            : base(chacheInfo)
        {
            Func = func;
            Param1 = param1;
            Param2 = param2;
            Param3 = param3;
            Param4 = param4;
        }
        protected override T ExcecuteCacheMethod()
        {
            return Func.Invoke(Param1, Param2, Param3, Param4);
        }

        public override string GenerateCacheKey()
        {
            return string.Format("{0}_{1}_{2}_{3}_{4}", CacheInfo.BasicKey, Param1, Param2, Param3, Param4);
        }
    }

    [DataContract]
    public class FunctionalCacheDataProvider<P1, P2, P3, P4, P5, T> : CacheDataProvider<T>
    {
        public Func<P1, P2, P3, P4, P5, T> Func { get; set; }
        [DataMember]
        public P1 Param1 { get; set; }
        [DataMember]
        public P2 Param2 { get; set; }
        [DataMember]
        public P3 Param3 { get; set; }
        [DataMember]
        public P4 Param4 { get; set; }
        [DataMember]
        public P5 Param5 { get; set; }
        public FunctionalCacheDataProvider(CacheInfo chacheInfo, Func<P1, P2, P3, P4, P5, T> func, P1 param1, P2 param2, P3 param3, P4 param4, P5 param5)
            : base(chacheInfo)
        {
            Func = func;
            Param1 = param1;
            Param2 = param2;
            Param3 = param3;
            Param4 = param4;
            Param5 = param5;
        }
        protected override T ExcecuteCacheMethod()
        {
            return Func.Invoke(Param1, Param2, Param3, Param4, Param5);
        }

        public override string GenerateCacheKey()
        {
            return string.Format("{0}_{1}_{2}_{3}_{4}_{5}", CacheInfo.BasicKey, Param1, Param2, Param3, Param4, Param5); ;
        }
    }
}
