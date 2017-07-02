using Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public interface IConstantService : IServiceBase<Constant>
    {
        List<Constant> GetConstants(bool useCache = true);

        bool TryGetValue<T>(string key, out T value, bool useCache = true);

        bool TryGetValue<T>(string key, string category, out T value, bool useCache = true);

        bool TryGetValue<T>(string key, string category, string culture, out T value, bool useCache = true);
        List<Constant> GetConstantByNameOfCategory(string categoryName, bool allowGetNotShared = true, bool useCache = true);
        List<Constant> GetConstantByNameOfCategoryAndCulture(string nameCategory, string cultureName, bool useCache = true);
        T GetValueByCategory<T>(string key, string category);
        string GetDefaultCulture(bool useCache = false);

        T TryGetValueByKey<T>(string key, bool useCache = true);
    }
}
