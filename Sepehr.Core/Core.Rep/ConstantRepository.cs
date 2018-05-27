using Core.Cmn;
using Core.Cmn.Attributes;
using Core.Cmn.Extensions;
using Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Core.Rep
{
    [Injectable(InterfaceType = typeof(IConstantRepository), DomainName = "Accounting")]
    public class ConstantRepository : RepositoryBase<Constant>, IConstantRepository
    {
        #region Const

        private const string GENERAL_CONFIG_NAME = "GeneralConfig";
        private const string DEFAULT_LANGUAGE_KEY = "DefaultLanguage";
        private const string DEFAULT_CULTURE = "fa-IR";

        #endregion Const

        #region Variable

        private IDbContextBase _dc;
        //private ILogService _logService = AppBase.LogService;

        #endregion Variable

        #region Constructors

        public ConstantRepository()
            : base()
        {
        }

        public ConstantRepository(IDbContextBase dc)
            : base(dc)
        {
            _dc = dc;
        }

        #endregion Constructors

        #region Methods

        [Cacheable(
            EnableSaveCacheOnHDD = true,
            AutoRefreshInterval = 600,
            CacheRefreshingKind = Cmn.Cache.CacheRefreshingKind.SqlDependency,
            EnableToFetchOnlyChangedDataFromDB = true,
            EnableCoreSerialization = true
            )]
        public static IQueryable<Constant> AllConstantCache(IQueryable<Constant> query)
        {
            return query.Include(item => item.ConstantCategory).AsNoTracking();
        }

        public IQueryable<Constant> AllCache(bool canUseCacheIfPossible = true)
        {
            return Cache<Constant>(AllConstantCache, canUseCacheIfPossible);
        }

        public Constant GetConstant(string key)
        {
            try
            {
                return AllCache().First(c => c.Key == key);
            }
            catch (Exception ex)
            {
                AppBase.LogService.Handle(ex, "This Name[" + key + "] not found in constants table.");

                return null;
            }
        }

        public T TryGetValueByKey<T>(string key, bool useCache = true)
        {
            IList<Constant> result =
                 AllCache(useCache)
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

        public bool TryGetValue<T>(string key, out T value, bool useCache = true)
        {
            IList<Constant> result =
                AllCache(useCache)
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

        private List<Constant> GetConstantsOfCategory(Expression<Func<Constant, bool>> predicate, bool useCache)
        {
            return AllCache(useCache)
                .Where(predicate)
                .ToList();
        }

        public List<Constant> GetConstantsOfCategory(string constantCat, bool useCache = true)
        {
            try
            {
                return
                    GetConstantsOfCategory(con =>
                    con.ConstantCategory.Name == constantCat, useCache
                    );
            }
            catch (Exception ex)
            {
                AppBase.LogService.Handle(ex, "This Name[" + constantCat + "] not found in constantCategories in constant table.");
                return null;
            }
        }

        public List<Constant> GetConstantsByCategoryAndCulture(string constantCat, string cultureName, bool useCache = true)
        {
            try
            {
                return
                    GetConstantsOfCategory(constant =>
                    constant.ConstantCategory.Name == constantCat
                    &&
                    constant.Culture == cultureName, useCache
                    );
            }
            catch (Exception ex)
            {
                AppBase.LogService.Handle(ex, "This Name[" + constantCat + "] not found in constantCategories in constant table.");

                return null;
            }
        }

        public Constant GetByCategoryNameAndKey(
            string categoryName,
            string key
            )
        {
            return AllCache()
            .FirstOrDefault(item =>
                item.ConstantCategory.Name == categoryName
                &&
                item.Key == key
            );
        }

        public void UpdateValue(
            int constantId,
            string newValue
            )
        {
            IQueryable<Constant> source =
                this.All()
                .Where(c => c.ID == constantId);

            Update(source, item => new Constant
            {
                Value = newValue
            });
        }

        #endregion Methods

        //IQueryable<IDTO> IDTOQueryableBuilder<Log>.GetDtoQueryable(IQueryable<Log> queryable)
        //{
        //    throw new NotImplementedException();
        //}
    }
}