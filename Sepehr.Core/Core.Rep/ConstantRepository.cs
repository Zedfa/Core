using Core.Cmn;
using Core.Cmn.Attributes;
using System;
using Core.Cmn.Extensions;
using System.Collections.Generic;
using System.Linq;
using Core.Entity;

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
        #endregion

        #region Variable

        IDbContextBase _dc;
        //private ILogService _logService = AppBase.LogService;
        #endregion

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

        #endregion

        #region Methods
        [Cacheable(EnableSaveCacheOnHDD = true, ExpireCacheSecondTime = 60, EnableAutomaticallyAndPeriodicallyRefreshCache = true)]
        public static IQueryable<Constant> AllConstantCache(IQueryable<Constant> query)
        {
            return query.Include(item => item.ConstantCategory).AsNoTracking();
        }

        public override IQueryable<Constant> All(bool canUseCacheIfPossible = true)
        {
            return Cache<Constant>(AllConstantCache, canUseCacheIfPossible);
        }

        public Constant GetConstant(string key)
        {
            try
            {
                return All().First(c => c.Key == key);
            }
            catch (Exception ex)
            {
                var eLog = AppBase.LogService.GetEventLogObj();
                eLog.UserId = "constantRepository";
                eLog.CustomMessage = "This Name[" + key + "] not found in constants table.";
                eLog.LogFileName = "constantRepository";
                eLog.OccuredException = ex;
                AppBase.LogService.Handle(eLog);

                return null;
            }
        }

        public T TryGetValueByKey<T>(string key, bool useCache = true)
        {
            IList<Constant> result =
                 All(useCache)
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
                All(useCache)
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
            return All(useCache).Where(predicate).ToList();
        }

        public List<Constant> GetConstantsOfCategory(string constantCat, bool useCache = true)
        {
            try
            {
                return GetConstantsOfCategory(con => con.ConstantCategory.Name == constantCat, useCache);
            }
            catch (Exception ex)
            {
                var eLog = AppBase.LogService.GetEventLogObj();
                eLog.UserId = "constantRepository";
                eLog.CustomMessage = "This Name[" + constantCat + "] not found in constantCategories in constant table.";
                eLog.LogFileName = "constantRepository";
                eLog.OccuredException = ex;
                AppBase.LogService.Handle(eLog);

                return null;
            }
        }
        public List<Constant> GetConstantsByCategoryAndCulture(string constantCat, string cultureName, bool useCache = true)
        {
            try
            {
                return GetConstantsOfCategory(constant => constant.ConstantCategory.Name == constantCat && constant.Culture == cultureName, useCache);
            }
            catch (Exception ex)
            {
                var eLog = AppBase.LogService.GetEventLogObj();
                eLog.UserId = "constantRepository";
                eLog.CustomMessage = "This Name[" + constantCat + "] not found in constantCategories in constant table.";
                eLog.LogFileName = "constantRepository";
                eLog.OccuredException = ex;
                AppBase.LogService.Handle(eLog);

                return null;
            }
        }
        #endregion



        //IQueryable<IDTO> IDTOQueryableBuilder<Log>.GetDtoQueryable(IQueryable<Log> queryable)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
