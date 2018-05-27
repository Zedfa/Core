using Core.Cmn;
using Core.Cmn.Attributes;
using Core.Entity;
using Core.Rep;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Core.Service
{
    [Injectable(InterfaceType = typeof(IConstantService), DomainName = "Core")]
    public class ConstantService : ServiceBase<Constant>, IConstantService
    {
        private const string DEFAULT_CULTURE = "fa-IR";
        private const string DEFAULT_LANGUAGE_KEY = "DefaultLanguage";
        private const string GENERAL_CONFIG_NAME = "GeneralConfig";

        public ConstantService(IDbContextBase context)
            : base(context)
        {
            _repositoryBase = new ConstantRepository(context);
        }

        public ConstantService()
            : base()
        {
            _repositoryBase = new ConstantRepository();
            //  _constantCategoryRepository = new ConstantRepository();
        }

        [Cacheable(
            AutoRefreshInterval = 10,
            EnableCoreSerialization = true
            )]
        public static T GetValueByCategory_Cache<T>(string key, string category)
        {
            var service = ServiceBase.DependencyInjectionFactory.CreateInjectionInstance<IConstantService>();
            T value;
            service.TryGetValue(key, category, out value);
            return value;
        }

        public List<Constant> GetConstantByNameOfCategory(string categoryName, bool allowGetNotShared = true, bool useCache = true)
        {
            List<Constant> constantsList = new List<Constant>();

            if (allowGetNotShared == false && !categoryName.Contains("Shared_"))
            {
                //var eLog = Core.Cmn.AppBase.LogService.GetEventLogObj();
                //eLog.UserId = "constantService";
                //eLog.CustomMessage = "Access Denied! [" + categoryName + "] constantCategory is not shared";
                //eLog.LogFileName = "constantService";
                //Core.Cmn.AppBase.LogService.Handle(eLog);
                Core.Cmn.AppBase.LogService.Write("Access Denied! [" + categoryName + "] constantCategory is not shared");
                return constantsList;
            }
            constantsList = GetConstantByNameOfCategoryAndCulture(categoryName, CurrentCulture.Name, useCache);

            if (constantsList.Count == 0)
            {
                constantsList = GetConstantByNameOfCategoryAndCulture(categoryName, GetDefaultCulture(useCache), useCache);
            }
            return constantsList;
        }

        public List<Constant> GetConstantByNameOfCategoryAndCulture(string nameCategory, string cultureName, bool useCache = true)
        {
            return ((ConstantRepository)_repositoryBase).GetConstantsByCategoryAndCulture(nameCategory, cultureName, useCache);
        }

        public List<Constant> GetConstants(bool useCache = true)
        {
            return ((ConstantRepository)_repositoryBase).All(useCache).ToList();
        }

        public string GetDefaultCulture(bool useCache = true)
        {
            string defaultCulture = DEFAULT_CULTURE;

            Constant constant = ((ConstantRepository)_repositoryBase)
                .AllCache(useCache)
                .FirstOrDefault(
                c =>
                c.Key == DEFAULT_LANGUAGE_KEY && c.ConstantCategory.Name == GENERAL_CONFIG_NAME
                );

            if (constant != null)
            {
                if (constant.Value == "fa")
                {
                    defaultCulture = "fa-IR";
                }
                else if (constant.Value == "ar")
                {
                    defaultCulture = "ar-SA";
                }
                else if (constant.Value == "en")
                {
                    defaultCulture = "en-US";
                }
            }

            return defaultCulture;
        }

        public int GetIdByEnum(Enum currentEnumValue)
        {
            var categoryName = currentEnumValue.GetType().Name;
            var constant = ((ConstantRepository)_repositoryBase).GetByCategoryNameAndKey(
                categoryName,
                currentEnumValue.ToString()
                );

            if (constant != null)
            {
                return constant.ID;
            }
            else
                throw (new Exception($"{currentEnumValue.ToString()} not found on constant table."));
        }

        public T GetValueByCategory<T>(string key, string category)
        {
            var value = Cache(GetValueByCategory_Cache<T>, key, category);
            return (T)Convert.ChangeType(value, typeof(T));
        }

        public T GetValueByEnum<T>(Enum enumValue)
        {
            var categoryName = enumValue.GetType().Name;
            var constant = ((ConstantRepository)_repositoryBase).GetByCategoryNameAndKey(
                categoryName,
                enumValue.ToString()
                );

            if (constant != null)
            {
                try
                {
                    var value = (T)Convert.ChangeType(constant.Value, typeof(T));
                    return value;
                }
                catch (Exception exception)
                {
                    exception.Throw($"Error on GetValueByEnum on Core.Constant.Key {enumValue}: {constant.Value}");
                    throw;
                }
            }
            else
                return default(T);
        }

        public bool TryGetValue<T>(string key, out T value, bool useCache = true)
        {
            IList<Constant> result = ((ConstantRepository)_repositoryBase)
                .AllCache(useCache)
                .Where(item => item.Key == key)
                .ToList();

            if (result.Count == 0)
            {
                value = default(T);
                return false;
            }

            if (result.Count == 1)
            {
                value = (T)Convert.ChangeType(result[0].Value, typeof(T));
                return true;
            }

            Constant constant = result
                .Where(c => c.ConstantCategory.Name == GENERAL_CONFIG_NAME && c.Culture == CurrentCulture.Name)
                .FirstOrDefault();
            if (constant != null)
            {
                value = (T)Convert.ChangeType(constant.Value, typeof(T));
                return true;
            }
            else
            {
                value = default(T);
                return false;
            }
        }

        public bool TryGetValue<T>(string key, string category, out T value, bool useCache = true)
        {
            IList<Constant> result = ((ConstantRepository)_repositoryBase)
                .AllCache(useCache)
                .Where(c => c.Key == key && c.ConstantCategory.Name == category)
                .ToList();

            if (result.Count == 0)
            {
                value = default(T);
                return false;
            }

            if (result.Count == 1)
            {
                value = (T)Convert.ChangeType(result[0].Value, typeof(T));
                return true;
            }

            Constant constant = result
                .Where(c => c.Culture == CurrentCulture.Name)
                .FirstOrDefault();
            if (constant != null)
            {
                value = (T)Convert.ChangeType(constant.Value, typeof(T));
                return true;
            }
            else
            {
                value = default(T);
                return false;
            }
        }

        public bool TryGetValue<T>(string key, string category, string culture, out T value, bool useCache = true)
        {
            Constant constant = ((ConstantRepository)_repositoryBase)
                .AllCache(useCache)
                .FirstOrDefault(c => c.Key == key && c.ConstantCategory.Name == category && c.Culture == culture);

            if (constant != null)
            {
                value = (T)Convert.ChangeType(constant.Value, typeof(T));
                return true;
            }
            else
            {
                value = default(T);
                return false;
            }
        }

        public T TryGetValueByKey<T>(string key, bool useCache = true)
        {
            IList<Constant> result = ((ConstantRepository)_repositoryBase)
                .AllCache(useCache)
                .Where(item => item.Key == key)
                .ToList();

            if (result.Count == 1)
            {
                return (T)Convert.ChangeType(result[0].Value, typeof(T));
            }
            else if (result.Count == 2)
            {
                Constant constant = result
                .Where(c => c.Culture == CurrentCulture.Name)
                .FirstOrDefault();

                if (constant != null)
                {
                    return (T)Convert.ChangeType(constant.Value, typeof(T));
                }
            }

            return default(T);
        }

        private int? GetId(Expression<Func<Constant, bool>> predicate)
        {
            Constant constant = ((ConstantRepository)_repositoryBase).AllCache().FirstOrDefault(predicate);
            if (constant != null)
            {
                var id = constant.ID;
                return id;
            }
            else
                return null;
        }

        public Dictionary<string, Constant> GetAllTraceKey()
        {
            return Cache(GetAllTraceKey_Cache);
        }

        [Cacheable(AutoRefreshInterval = 30)]
        public static Dictionary<string, Constant> GetAllTraceKey_Cache()
        {
            var service = ServiceBase.DependencyInjectionFactory.CreateInjectionInstance<IConstantService>();
            return service.GetConstantByNameOfCategory(Core.Cmn.GeneralConstant.TraceConfig).
                ToDictionary(item => item.Key, item => item);
        }

        public void UpdateValueByEnum(
            Enum enumToUpdate,
            string newValue
            )
        {
            var categoryName = enumToUpdate.GetType().Name;
            var constant = ((ConstantRepository)_repositoryBase).GetByCategoryNameAndKey(
                categoryName,
                enumToUpdate.ToString()
                );

            if (constant != null)
            {
                ((ConstantRepository)_repositoryBase).UpdateValue(constant.ID, newValue);
            }
            else
            {
                throw new Exception($"Error on UpdateValue Core.Constant.Key '{enumToUpdate}'");
            }
        }
    }
}