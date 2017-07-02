using Core.Cmn;
using Core.Cmn.UnitTesting;
using Core.Entity;
using Core.UnitTesting.Mock;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Core.UnitTesting.Entity
{
    public abstract class EntityUnitTestHelperBase<TEntity> : UnitTestBase
        where TEntity : EntityBase<TEntity>, new()        
    {
        private const int DEFAULT_MIN_ENTITY_COUNT = 2;
        private const int DEFAULT_MAX_ENTITY_COUNT = 5;

        

        private static MockDbSet<TEntity> mockData = new MockDbSet<TEntity>();
        
        public static MockDbSet<TEntity> GetMockData()
        {
            return mockData;
        }

        private static MethodInfo _getMockDataMethod;
        public static MethodInfo GetMockDataMethod
        {
            get
            {
                return _getMockDataMethod ?? (_getMockDataMethod = typeof(EntityUnitTestHelperBase<TEntity>).GetMethod("GetMockData", BindingFlags.Static | BindingFlags.Public));
            }
        }

        public abstract string TableName { get; }
        public abstract void AssertEntitiesAreEqual(TEntity expected, TEntity actual);
        public abstract TEntity CreateSampleEntity();        
        public abstract Expression<Func<TEntity, bool>> GetFindPredicate(TEntity entity);
        public abstract Expression<Func<TEntity, bool>> GetFindByIdPredicate(TEntity entity);
        public abstract ExpressionInfo GetFilterExpressionInfo(TEntity entity);
        public abstract void EditSampleEntity(TEntity entity);
        public abstract Expression<Func<TEntity, TEntity>> GetUpdatePredicate(TEntity entity);
        protected abstract void SeedMockEntityList(IList<TEntity> mockEntityList);
        
        public IList<TEntity> CreateSampleEntityList(int minCount = DEFAULT_MIN_ENTITY_COUNT, int maxCount = DEFAULT_MAX_ENTITY_COUNT)
        {
            minCount = Math.Max(minCount, DEFAULT_MIN_ENTITY_COUNT);
            maxCount = Math.Max(maxCount, minCount);
            IList<TEntity> list = new List<TEntity>();
            int entityCount = new Random().Next() % (maxCount - minCount + 1) + minCount;
            for (int i = 0; i < entityCount; i++)
            {
                list.Add(CreateSampleEntity());
            }
            return list;
        }

        public void SeedData()
        {            
            SeedMockEntityList(mockData.MockEntityList);
        }

        public void CleanData()
        {
            mockData.MockEntityList.Clear();            
        }
    }
}
