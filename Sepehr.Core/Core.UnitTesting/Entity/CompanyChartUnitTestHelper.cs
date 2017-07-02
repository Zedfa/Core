using Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Core.UnitTesting.Entity
{
    public class CompanyChartUnitTestHelper : EntityUnitTestHelperBase<CompanyChart>
    {

        public override string TableName { get { return "core.CompanyCharts"; } }

        public override void AssertEntitiesAreEqual(CompanyChart expected, CompanyChart actual)
        {
            Assert.IsNotNull(expected);
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Depth, actual.Depth);
            Assert.AreEqual(expected.Level, actual.Level);
            Assert.AreEqual(expected.Lineage, actual.Lineage);
            Assert.AreEqual(expected.Title, actual.Title);
        }

        public override CompanyChart CreateSampleEntity()
        {
            CompanyChart entity = BuildSampleEntity<CompanyChart>();
            return entity;
        }

        public override Expression<Func<CompanyChart, bool>> GetFindPredicate(CompanyChart entity)
        {
            return e =>
                e.Depth == entity.Depth
                && e.Level == entity.Level
                && e.Lineage == entity.Lineage
                && e.Title == entity.Title;
        }

        public override Expression<Func<CompanyChart, bool>> GetFindByIdPredicate(CompanyChart entity)
        {
            return e => e.Id == entity.Id;
        }

        public override ExpressionInfo GetFilterExpressionInfo(CompanyChart entity)
        {
            return new ExpressionInfo
            {
                CurrentPage = 0,
                PageSize = 10,
                Expression = new KeyValuePair<string, string>("Id", entity.Id.ToString())
            };
        }

        public override void EditSampleEntity(CompanyChart entity)
        {
            entity.Depth = GetRandomInt();
            entity.Level = GetRandomShortNullable();
            entity.Lineage = GetRandomString();
            entity.Title = GetRandomString();            
        }

        public override Expression<Func<CompanyChart, CompanyChart>> GetUpdatePredicate(CompanyChart entity)
        {
            return item => new CompanyChart
            {
                Depth = entity.Depth,
                Level = entity.Level,
                Lineage = entity.Lineage,
                Title = entity.Title
            };
        }

        protected override void SeedMockEntityList(IList<CompanyChart> mockEntityList)
        {

            IList<CompanyChart> createdList = CreateSampleEntityList(10, 100);
            for (int i = 0; i < createdList.Count; i++)
            {
                createdList[i].Id = i + 1;
                mockEntityList.Add(createdList[i]);
            }
        }
    }
}
