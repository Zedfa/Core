using Core.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Core.UnitTesting.Entity
{
    public class LogUnitTestHelper : EntityUnitTestHelperBase<Log>
    {
        public override string TableName { get { return "core.Logs"; } }

        public override void AssertEntitiesAreEqual(Log expected, Log actual)
        {
            Assert.IsNotNull(expected);
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.CreateDate, actual.CreateDate);
            Assert.AreEqual(expected.CustomMessage, actual.CustomMessage);
            Assert.AreEqual(expected.ApplicationName, actual.ApplicationName);
            Assert.AreEqual(expected.InnerExceptionCount, actual.InnerExceptionCount);
        }

        public override Log CreateSampleEntity()
        {
            Log entity = BuildSampleEntity<Log>();
            return entity;
        }

        public override Expression<Func<Log, bool>> GetFindPredicate(Log entity)
        {
            return e =>
                e.CreateDate == entity.CreateDate
                && e.CustomMessage == entity.CustomMessage
                && e.ApplicationName == entity.ApplicationName
                && e.InnerExceptionCount == entity.InnerExceptionCount;
        }

        public override Expression<Func<Log, bool>> GetFindByIdPredicate(Log entity)
        {
            return e => e.Id == entity.Id;
        }

        public override ExpressionInfo GetFilterExpressionInfo(Log entity)
        {
            return new ExpressionInfo
            {
                CurrentPage = 0,
                PageSize = 10,
                Expression = new KeyValuePair<string, string>("ID", entity.Id.ToString())
            };
        }

        public override void EditSampleEntity(Log entity)
        {
            entity.CreateDate = GetRandomDateTime();
            entity.CustomMessage = GetRandomString();
            entity.ApplicationName = GetRandomString();
            entity.InnerExceptionCount = GetRandomInt();
        }

        public override Expression<Func<Log, Log>> GetUpdatePredicate(Log entity)
        {
            return item => new Log
            {
                CreateDate = entity.CreateDate,
                CustomMessage = entity.CustomMessage,
                ApplicationName = entity.ApplicationName,
                InnerExceptionCount = entity.InnerExceptionCount
            };
        }

        protected override void SeedMockEntityList(IList<Log> mockEntityList)
        {
            IList<Log> createdList = CreateSampleEntityList(10, 100);
            for (int i = 0; i < createdList.Count; i++)
            {
                createdList[i].Id = i + 1;
                mockEntityList.Add(createdList[i]);
            }
        }
    }
}