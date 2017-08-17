using Core.Cmn;
using Core.Cmn.UnitTesting;

using Core.Repository.Interface;
using Core.Service;
using Core.UnitTesting.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;

using System.Linq;

namespace Core.UnitTesting.Service
{
    public abstract class ServiceUnitTestBase<TService, TRepository, TEntity> : UnitTestBase
        where TEntity : EntityBase<TEntity>, new()
        where TRepository : IRepositoryBase<TEntity>
        where TService : IServiceBase<TEntity>
    {
        protected abstract EntityUnitTestHelperBase<TEntity> EntityUnitTestHelper { get; }

        protected abstract TService ConstructService();

        public virtual void ConstructorTest()
        {
            // assemble & act
            TService service = ConstructService();

            // assert
            Assert.IsNotNull(service);
        }

        public virtual void GetContextTest()
        {
            // assemble
            TService service = ConstructService();

            // act
            IDbContextBase ctx = service.ContextBase;

            // assert
            Assert.IsNotNull(service.ContextBase);
        }

        public virtual void AllTest()
        {
            // assemble
            TService service = ConstructService();

            // act
            IQueryable<TEntity> all = service.All(false);

            // assert
            Assert.IsNotNull(all);
        }

        public virtual void CreateByEntityTest()
        {
            // assemble
            TService service = ConstructService();
            TEntity entity = EntityUnitTestHelper.CreateSampleEntity();

            // act
            entity = service.Create(entity);

            TEntity found = service.Find(EntityUnitTestHelper.GetFindByIdPredicate(entity));

            // assert
            Assert.IsNotNull(found);
            EntityUnitTestHelper.AssertEntitiesAreEqual(entity, found);
        }

        public virtual void CreateByListTest()
        {
            // assemble
            TService service = ConstructService();
            IList<TEntity> entityList = EntityUnitTestHelper.CreateSampleEntityList();

            // act
            service.Create(entityList.ToList());

            foreach (TEntity entity in entityList)
            {
                TEntity found = service.Find(EntityUnitTestHelper.GetFindPredicate(entity));

                // assert
                Assert.IsNotNull(found);
                EntityUnitTestHelper.AssertEntitiesAreEqual(entity, found);
            }
        }

        public virtual void DeleteByEntityTest()
        {
            // assemble
            TService service = ConstructService();
            TEntity entity = EntityUnitTestHelper.CreateSampleEntity();
            entity = service.Create(entity);

            // act
            service.Delete(entity);

            // assert
            TEntity found = service.Find(EntityUnitTestHelper.GetFindByIdPredicate(entity));
            Assert.IsNull(found);
        }

        public virtual void DeleteByPredicateTest()
        {
            // assemble
            TService service = ConstructService();
            TEntity entity = EntityUnitTestHelper.CreateSampleEntity();
            entity = service.Create(entity);

            // act
            service.Delete(EntityUnitTestHelper.GetFindByIdPredicate(entity));

            // assert
            TEntity found = service.Find(EntityUnitTestHelper.GetFindByIdPredicate(entity));
            Assert.IsNull(found);
        }

        public virtual void DeleteByEntityListTest()
        {
            // assemble
            TService service = ConstructService();
            IList<TEntity> entityList = EntityUnitTestHelper.CreateSampleEntityList();

            // act
            service.Create(entityList.ToList());

            foreach (TEntity entity in entityList)
            {
                TEntity found = service.Find(EntityUnitTestHelper.GetFindByIdPredicate(entity));

                // assert
                Assert.IsNotNull(found);
            }
        }

        public virtual void UpdateByEntityTest()
        {
            // assemble
            TService service = ConstructService();
            TEntity entity = EntityUnitTestHelper.CreateSampleEntity();
            entity = service.Create(entity);
            EntityUnitTestHelper.EditSampleEntity(entity);

            // act
            service.Update(entity);

            TEntity found = service.Find(EntityUnitTestHelper.GetFindPredicate(entity));

            // assert
            EntityUnitTestHelper.AssertEntitiesAreEqual(entity, found);
        }

        public virtual void UpdateByPredicateTest()
        {
            // assemble
            TEntity edited;
            TService service1 = ConstructService();
            TEntity entity = EntityUnitTestHelper.CreateSampleEntity();
            entity = service1.Create(entity);

            edited = entity.ShallowCopy() as TEntity;
            EntityUnitTestHelper.EditSampleEntity(edited);

            // act
            service1.Update(EntityUnitTestHelper.GetFindByIdPredicate(edited), EntityUnitTestHelper.GetUpdatePredicate(edited));

            TService service2 = ConstructService();
            TEntity found = service2.Find(EntityUnitTestHelper.GetFindByIdPredicate(edited));

            // assert
            EntityUnitTestHelper.AssertEntitiesAreEqual(edited, found);
        }

        public virtual void GetCountTest()
        {
            // assemble
            TService service = ConstructService();

            int countBeforeAdd = service.Count;

            TEntity entity = EntityUnitTestHelper.CreateSampleEntity();
            entity = service.Create(entity);

            int countAfterAdd = service.Count;

            // assert
            Assert.AreEqual(countBeforeAdd + 1, countAfterAdd);
        }

        public virtual void FindByKeysTest()
        {
            //TODO: What Key??
        }

        public virtual void FilterByPredicateTest()
        {
            // assemble
            TService service = ConstructService();

            IQueryable<TEntity> filteredList;

            // act
            filteredList = service.Filter(c => true);

            // assert
            Assert.IsNotNull(filteredList);

            // act
            filteredList = service.Filter(c => false);

            // assert
            Assert.IsNotNull(filteredList);
            Assert.AreEqual(0, filteredList.Count());
        }

        public virtual void FilterByExpressionTest()
        {
            // assemble
            TService service = ConstructService();

            IQueryable<TEntity> filteredList;

            // act
            filteredList = service.Filter("1 = 1");

            // assert
            Assert.IsNotNull(filteredList);

            // act
            filteredList = service.Filter("1 = 0");

            // assert
            Assert.IsNotNull(filteredList);
            Assert.AreEqual(0, filteredList.Count());
        }

        public virtual void FindByPredicateTest()
        {
            // assemble
            TService service = ConstructService();

            TEntity entity = EntityUnitTestHelper.CreateSampleEntity();
            entity = service.Create(entity);

            // act
            TEntity found = service.Find(EntityUnitTestHelper.GetFindPredicate(entity));

            // assert
            Assert.IsNotNull(found);
            EntityUnitTestHelper.AssertEntitiesAreEqual(entity, found);
        }

        public virtual void GetAppBaseTest()
        {
            // assemble
            TService service = ConstructService();

            // act
            Core.Service.AppBase app = service.AppBase;

            // assert
            Assert.IsNotNull(app);
        }
    }
}