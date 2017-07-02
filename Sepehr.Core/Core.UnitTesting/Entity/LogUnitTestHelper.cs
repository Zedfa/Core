using Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            Assert.AreEqual(expected.LogType, actual.LogType);
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
                && e.LogType == entity.LogType
                && e.InnerExceptionCount == entity.InnerExceptionCount;
        }

        public override Expression<Func<Log, bool>> GetFindByIdPredicate(Log entity)
        {
            return e => e.ID == entity.ID;
        }

        public override ExpressionInfo GetFilterExpressionInfo(Log entity)
        {
            return new ExpressionInfo
            {
                CurrentPage = 0,
                PageSize = 10,
                Expression = new KeyValuePair<string, string>("ID", entity.ID.ToString())
            };
        }

        public override void EditSampleEntity(Log entity)
        {
            entity.CreateDate = GetRandomDateTime();
            entity.CustomMessage = GetRandomString();
            entity.LogType = GetRandomString();
            entity.InnerExceptionCount = GetRandomInt();            
        }

        public override Expression<Func<Log, Log>> GetUpdatePredicate(Log entity)
        {
            return item => new Log
            {
                CreateDate = entity.CreateDate,
                CustomMessage = entity.CustomMessage,
                LogType = entity.LogType,
                InnerExceptionCount = entity.InnerExceptionCount
            };
        }

        protected override void SeedMockEntityList(IList<Log> mockEntityList)
        {

            IList<Log> createdList = CreateSampleEntityList(10, 100);
            for (int i = 0; i < createdList.Count; i++)
            {
                createdList[i].ID = Guid.NewGuid();
                mockEntityList.Add(createdList[i]);
            }
        }
    }
}
