using System.Linq;

namespace Core.Cmn.Cache
{
    public interface IQueryableCacheDataProvider
    {
        IQueryable GetQuery();
        IDbContextBase DbContext { get; set; }
    }
}