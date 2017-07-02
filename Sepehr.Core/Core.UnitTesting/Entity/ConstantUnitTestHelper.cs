using Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Core.UnitTesting.Entity
{
    public class ConstantUnitTestHelper : EntityUnitTestHelperBase<Constant>
    {

        public override string TableName { get { return "core.Constants"; } }

        public override void AssertEntitiesAreEqual(Constant expected, Constant actual)
        {
            Assert.IsNotNull(expected);
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.ConstantCategoryID, actual.ConstantCategoryID);
            Assert.AreEqual(expected.Culture, actual.Culture);
            Assert.AreEqual(expected.Key, actual.Key);
            Assert.AreEqual(expected.Value, actual.Value);
        }

        public override Constant CreateSampleEntity()
        {
            ConstantCategoryUnitTestHelper categoryHelper = new ConstantCategoryUnitTestHelper();
            Constant constant = BuildSampleEntity<Constant>();
            constant.ConstantCategory = categoryHelper.CreateSampleEntity();
            constant.ConstantCategoryID = constant.ConstantCategory.ID;
            constant.Culture = GetRandomCulture();
            return constant;
        }

        public override Expression<Func<Constant, bool>> GetFindPredicate(Constant entity)
        {
            return constant =>
                constant.ConstantCategoryID == entity.ConstantCategoryID
                && constant.Culture == entity.Culture
                && constant.Key == entity.Key
                && constant.Value == entity.Value;
        }

        public override Expression<Func<Constant, bool>> GetFindByIdPredicate(Constant entity)
        {
            return constant => constant.ID == entity.ID;
        }

        public override ExpressionInfo GetFilterExpressionInfo(Constant entity)
        {
            return new ExpressionInfo
            {
                CurrentPage = 0,
                PageSize = 10,
                Expression = new KeyValuePair<string, string>("ID", entity.ID.ToString())
            };
        }

        public override void EditSampleEntity(Constant entity)
        {
            entity.Value = GetRandomString();
        }

        public override Expression<Func<Constant, Constant>> GetUpdatePredicate(Constant entity)
        {
            return item => new Constant
            {
                Value = entity.Value
            };
        }

        protected override void SeedMockEntityList(IList<Constant> mockEntityList)
        {
            ConstantCategoryUnitTestHelper categoryHelper = new ConstantCategoryUnitTestHelper();
            IList<ConstantCategory> categoryList = categoryHelper.CreateSampleEntityList(2, 10);

            for (int i = 0; i < categoryList.Count; i++)            
            {
                ConstantCategory cat = categoryList[i];
                cat.ID = i + 1;
            }

            IList<Constant> createdList = CreateSampleEntityList(10, 100);
            for (int i = 0; i < createdList.Count; i++)
            {
                ConstantCategory category = categoryList[_rand.Next() % categoryList.Count];
                createdList[i].ID = i + 1;
                createdList[i].ConstantCategory = category;
                createdList[i].ConstantCategoryID = category.ID;
                mockEntityList.Add(createdList[i]);
            }
        }       

    }
}
