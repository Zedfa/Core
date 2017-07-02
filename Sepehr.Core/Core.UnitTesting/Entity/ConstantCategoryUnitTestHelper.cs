using Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Core.UnitTesting.Entity
{
    public class ConstantCategoryUnitTestHelper : EntityUnitTestHelperBase<ConstantCategory>
    {

        public const string GENERAL_CONFIG_NAME = "GeneralConfig";

        public static readonly ConstantCategory GENERAL_CONFIG_CONSTANT_CATEGORY = new ConstantCategory
        {
            ID = 1,
            Name = GENERAL_CONFIG_NAME
        };

        public override string TableName { get { return "core.ConstantCategories"; } }

        public override void AssertEntitiesAreEqual(ConstantCategory expected, ConstantCategory actual)
        {
            Assert.IsNotNull(expected);
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Name, actual.Name);
        }

        public override ConstantCategory CreateSampleEntity()
        {
            ConstantCategory constantCategory = BuildSampleEntity<ConstantCategory>();            
            return constantCategory;
        }        

        public override Expression<Func<ConstantCategory, bool>> GetFindPredicate(ConstantCategory entity)
        {
            return constant =>
                constant.Name == entity.Name;
        }

        public override Expression<Func<ConstantCategory, bool>> GetFindByIdPredicate(ConstantCategory entity)
        {
            return e => e.ID == entity.ID;
        }

        public override ExpressionInfo GetFilterExpressionInfo(ConstantCategory entity)
        {
            return new ExpressionInfo
            {
                CurrentPage = 0,
                PageSize = 10,
                Expression = new KeyValuePair<string, string>("ID", entity.ID.ToString())
            };
        }

        public override void EditSampleEntity(ConstantCategory entity)
        {
            entity.Name = GetRandomString();
        }

        public override Expression<Func<ConstantCategory, ConstantCategory>> GetUpdatePredicate(ConstantCategory entity)
        {
            return item => new ConstantCategory
            {
                Name = entity.Name
            };
        }

        protected override void SeedMockEntityList(IList<ConstantCategory> mockEntityList)
        {            

            IList<ConstantCategory> createdList = CreateSampleEntityList(10, 100);
            for (int i = 0; i < createdList.Count; i++)
            {
                createdList[i].ID = i + 1;
                mockEntityList.Add(createdList[i]);
            }
        }
    }
}
