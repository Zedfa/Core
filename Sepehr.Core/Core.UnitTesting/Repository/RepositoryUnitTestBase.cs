using Core.Cmn;
using Core.Cmn.UnitTesting;

using Core.Repository.Interface;
using Core.UnitTesting.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;

using System.Linq;

namespace Core.UnitTesting.Repository
{
    public abstract class RepositoryUnitTestBase<TRepository, TEntity> : UnitTestBase
        where TEntity : EntityBase<TEntity>, new()
        where TRepository : IRepositoryBase<TEntity>

    {
        protected abstract EntityUnitTestHelperBase<TEntity> EntityUnitTestHelper { get; }

        protected abstract IDbContextBase CreateNewContext();

        protected abstract TRepository BuildRepository();

        protected abstract string GetTableName();

        protected abstract string GetSchemaName();

        protected RepositoryUnitTestBase()
        {
        }

        public virtual void ConstructorTest()
        {
            // assemble & act
            using (TRepository repository = BuildRepository())
            {
                // assert
                Assert.IsNotNull(repository);
            }
        }

        public virtual void AllTest()
        {
            // assemble
            using (TRepository repository = BuildRepository())
            {
                // act
                IQueryable<TEntity> all = repository.All(false);

                // assert
                Assert.IsNotNull(all);
            }
        }

        //public virtual void GetTableNameTest()
        //{
        //    // assemble
        //    using (TRepository repository = ConstructRepository())
        //    {
        //        // act
        //        string tableName = repository.TableName;

        //        // assert
        //        Assert.AreEqual(GetTableName(), tableName);
        //    }
        //}

        //public virtual void GetSchemaTest()
        //{
        //    // assemble
        //    using (TRepository repository = ConstructRepository())
        //    {
        //        // act
        //        string schema = repository.Schema;

        //        // assert
        //        Assert.AreEqual(GetSchemaName(), schema);
        //    }
        //}

        //public virtual void GetDbSetTest()
        //{
        //    // assemble
        //    using (TRepository repository = ConstructRepository())
        //    {
        //        // act
        //        DbSet<TEntity> dbSet = repository.DbSet;

        //        // assert
        //        Assert.IsNotNull(dbSet);
        //    }
        //}

        //public virtual void GetKeyNameTest()
        //{
        //    // assemble
        //    using (TRepository repository = ConstructRepository())
        //    {
        //        // act
        //        string keyName = repository.KeyName;

        //        // assert
        //        Assert.AreEqual("ID", keyName);
        //    }
        //}
        public virtual void FilterByExpressionTest()
        {
            // assemble
            using (TRepository repository = BuildRepository())
            {
                IQueryable<TEntity> filteredList;

                // act
                filteredList = repository.Filter(c => true);

                // assert
                Assert.IsNotNull(filteredList);

                // act
                filteredList = repository.Filter(c => false);

                // assert
                Assert.IsNotNull(filteredList);
                Assert.AreEqual(0, filteredList.Count());
            }
        }

        public virtual void FilterByStringTest()
        {
            // assemble
            using (TRepository repository = BuildRepository())
            {
                IQueryable<TEntity> filteredList;

                // act
                filteredList = repository.Filter("1 = 1");

                // assert
                Assert.IsNotNull(filteredList);

                // act
                filteredList = repository.Filter("1 = 0");

                // assert
                Assert.IsNotNull(filteredList);
                Assert.AreEqual(0, filteredList.Count());
            }
        }

        public virtual void FilterByExpressionIndexSizeTest()
        {
            // assemble
            using (TRepository repository = BuildRepository())
            {
                int total;
                IQueryable<TEntity> filteredList;

                // act
                filteredList = repository.Filter(c => true, out total, 0, 10);

                // assert
                Assert.IsNotNull(filteredList);

                // act
                filteredList = repository.Filter(c => false, out total, 0, 10);

                // assert
                Assert.IsNotNull(filteredList);
                Assert.AreEqual(0, total);
            }
        }

        public virtual void FilterByExpressionInfoTest()
        {
            using (TRepository repository = BuildRepository())
            {
                //TODO: Correct this test, how to use ExpressionInfo class?

                //TEntity entity = EntityUnitTestHelper.CreateSampleEntity();
                //repository.Delete(EntityUnitTestHelper.GetFindPredicate(entity));
                //entity = repository.Create(entity);

                //int total;
                //IEnumerable<TEntity> filteredList = repository.Filter(EntityUnitTestHelper.GetFilterExpressionInfo(entity), out total);

                //Assert.IsNotNull(filteredList);

                //repository.Delete(entity);

                //ReseedEntityId(repository, CreateNewContext());
            }
        }

        public virtual void ContainsCreatedEntityTest()
        {
            // assemble
            using (TRepository repository = BuildRepository())
            {
                // assemble
                TEntity entity = EntityUnitTestHelper.CreateSampleEntity();
                entity = repository.Create(entity);

                // act
                bool contains = repository.Contains(EntityUnitTestHelper.GetFindByIdPredicate(entity));

                // assert
                Assert.IsTrue(contains);
            }
        }

        public virtual void ContainsDeletedEntityTest()
        {
            // assemble
            using (TRepository repository = BuildRepository())
            {
                // assemble
                TEntity entity = EntityUnitTestHelper.CreateSampleEntity();
                entity = repository.Create(entity);
                repository.Delete(entity);

                // act
                bool contains = repository.Contains(EntityUnitTestHelper.GetFindByIdPredicate(entity));

                // assert
                Assert.IsFalse(contains);
            }
        }

        public virtual void FindByPredicateTest()
        {
            // assemble
            using (TRepository repository = BuildRepository())
            {
                //assemble
                TEntity entity = EntityUnitTestHelper.CreateSampleEntity();
                entity = repository.Create(entity);

                // act
                TEntity found = repository.Find(EntityUnitTestHelper.GetFindByIdPredicate(entity));

                // assert
                Assert.IsNotNull(found);
                EntityUnitTestHelper.AssertEntitiesAreEqual(entity, found);
            }
        }

        public virtual void CreateTest()
        {
            // assemble
            using (TRepository repository = BuildRepository())
            {
                TEntity entity = EntityUnitTestHelper.CreateSampleEntity();

                // act
                entity = repository.Create(entity);
                TEntity found = repository.Find(EntityUnitTestHelper.GetFindByIdPredicate(entity));

                // assert
                Assert.IsNotNull(found);
                EntityUnitTestHelper.AssertEntitiesAreEqual(entity, found);
            }
        }

        public virtual void CreateListTest()
        {
            // assemble
            using (TRepository repository = BuildRepository())
            {
                IList<TEntity> entityList = EntityUnitTestHelper.CreateSampleEntityList();

                // act
                repository.Create(entityList.ToList());

                foreach (TEntity entity in entityList)
                {
                    TEntity found = repository.Find(EntityUnitTestHelper.GetFindPredicate(entity));

                    // assert
                    Assert.IsNotNull(found);
                    EntityUnitTestHelper.AssertEntitiesAreEqual(entity, found);
                }
            }
        }

        public virtual void DeleteByEntityTest()
        {
            // assemble
            using (TRepository repository = BuildRepository())
            {
                // assemble
                TEntity entity = EntityUnitTestHelper.CreateSampleEntity();
                entity = repository.Create(entity);

                // act
                repository.Delete(entity);

                // assert
                TEntity found = repository.Find(EntityUnitTestHelper.GetFindByIdPredicate(entity));
                Assert.IsNull(found);
            }
        }

        public virtual void DeleteByEntityQueryableListTest()
        {
            // assemble
            using (TRepository repository = BuildRepository())
            {
                // assemble
                IList<TEntity> entityList = EntityUnitTestHelper.CreateSampleEntityList();
                repository.Create(entityList.ToList());

                // act
                repository.Delete(entityList.AsQueryable());

                // assert
                foreach (TEntity entity in entityList)
                {
                    TEntity found = repository.Find(EntityUnitTestHelper.GetFindByIdPredicate(entity));
                    Assert.IsNull(found);
                }

                // clean
                foreach (TEntity entity in entityList)
                {
                    repository.Delete(EntityUnitTestHelper.GetFindByIdPredicate(entity));
                };
            }
        }

        public virtual void DeleteByEntityListTest()
        {
            // assemble
            using (TRepository repository = BuildRepository())
            {
                // assemble
                IList<TEntity> entityList = EntityUnitTestHelper.CreateSampleEntityList();
                repository.Create(entityList.ToList());

                // act
                repository.Delete(entityList.ToList());

                // assert
                foreach (TEntity entity in entityList)
                {
                    TEntity found = repository.Find(EntityUnitTestHelper.GetFindByIdPredicate(entity));
                    Assert.IsNull(found);
                }
            }
        }

        public virtual void DeleteByPredicateTest()
        {
            // assemble
            using (TRepository repository = BuildRepository())
            {
                // assemble
                TEntity entity = EntityUnitTestHelper.CreateSampleEntity();
                entity = repository.Create(entity);

                // act
                repository.Delete(EntityUnitTestHelper.GetFindByIdPredicate(entity));

                // assert
                TEntity found = repository.Find(EntityUnitTestHelper.GetFindByIdPredicate(entity));
                Assert.IsNull(found);
            }
        }

        public virtual void GetCountTest()
        {
            // assemble
            using (TRepository repository = BuildRepository())
            {
                // act
                int countBeforeAdd = repository.Count;

                // assemble
                TEntity entity = EntityUnitTestHelper.CreateSampleEntity();
                entity = repository.Create(entity);

                // act
                int countAfterAdd = repository.Count;

                // assert
                Assert.AreEqual(countBeforeAdd + 1, countAfterAdd);
            }
        }

        public virtual void UpdateByEntityTest()
        {
            // assemble
            using (TRepository repository = BuildRepository())
            {
                // assemble
                TEntity entity = EntityUnitTestHelper.CreateSampleEntity();
                entity = repository.Create(entity);
                EntityUnitTestHelper.EditSampleEntity(entity);

                // act
                repository.Update(entity);

                TEntity found = repository.Find(EntityUnitTestHelper.GetFindPredicate(entity));

                // assert
                EntityUnitTestHelper.AssertEntitiesAreEqual(entity, found);
            }
        }

        public virtual void UpdateByPredicateTest()
        {
            //assemble
            TEntity edited;
            using (TRepository repository1 = BuildRepository())
            {
                // assemble
                TEntity entity = EntityUnitTestHelper.CreateSampleEntity();
                entity = repository1.Create(entity);

                edited = entity.ShallowCopy() as TEntity;
                EntityUnitTestHelper.EditSampleEntity(edited);

                // act
                repository1.Update(EntityUnitTestHelper.GetFindByIdPredicate(edited), EntityUnitTestHelper.GetUpdatePredicate(edited));
            }

            // assemble
            using (TRepository repository2 = BuildRepository())
            {
                TEntity found = repository2.Find(EntityUnitTestHelper.GetFindByIdPredicate(edited));

                // assert
                EntityUnitTestHelper.AssertEntitiesAreEqual(edited, found);
            }
        }
    }
}