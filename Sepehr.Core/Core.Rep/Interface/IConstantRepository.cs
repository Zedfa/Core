using Core.Entity;
using Core.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Cmn.EntityBase;
using System.Linq.Expressions;
using Core.Cmn.Attributes;

namespace Core.Rep
{
    public interface IConstantRepository : IRepositoryBase<Constant>
    {
        Constant GetConstant(string key);
        List<Constant> GetConstantsOfCategory(string constantCat, bool useCache = true);
        List<Constant> GetConstantsByCategoryAndCulture(string constantCat, string cultureName, bool useCache = true);
        T TryGetValueByKey<T>(string key, bool useCache = true);
        bool TryGetValue<T>(string key, out T value, bool useCache = true);

    }
}
