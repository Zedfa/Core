using Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Core.UnitTesting.Entity
{
    public class ExceptionLogUnitTestHelper : EntityUnitTestHelperBase<ExceptionLog>
    {

        public override string TableName { get { return "core.ExceptionLogs"; } }

        public override void AssertEntitiesAreEqual(ExceptionLog expected, ExceptionLog actual)
        {
            Assert.IsNotNull(expected);
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.ExceptionType, actual.ExceptionType);
            Assert.AreEqual(expected.Message, actual.Message);
            Assert.AreEqual(expected.StackTrace, actual.StackTrace);
            Assert.AreEqual(expected.Source, actual.Source);
        }

        public override ExceptionLog CreateSampleEntity()
        {
            ExceptionLog entity = BuildSampleEntity<ExceptionLog>();
            return entity;
        }

        public override Expression<Func<ExceptionLog, bool>> GetFindPredicate(ExceptionLog entity)
        {
            return e =>
                e.ExceptionType == entity.ExceptionType
                && e.Message == entity.Message
                && e.StackTrace == entity.StackTrace
                && e.Source == entity.Source;
        }

        public override Expression<Func<ExceptionLog, bool>> GetFindByIdPredicate(ExceptionLog entity)
        {
            return e => e.Id == entity.Id;
        }

        public override ExpressionInfo GetFilterExpressionInfo(ExceptionLog entity)
        {
            return new ExpressionInfo
            {
                CurrentPage = 0,
                PageSize = 10,
                Expression = new KeyValuePair<string, string>("ID", entity.Id.ToString())
            };
        }

        public override void EditSampleEntity(ExceptionLog entity)
        {
            entity.ExceptionType = GetRandomString();
            entity.Message = GetRandomString();
            entity.StackTrace = GetRandomString();
            entity.Source = GetRandomString();            
        }

        public override Expression<Func<ExceptionLog, ExceptionLog>> GetUpdatePredicate(ExceptionLog entity)
        {
            return item => new ExceptionLog
            {
                ExceptionType = entity.ExceptionType,
                Message = entity.Message,
                StackTrace = entity.StackTrace,
                Source = entity.Source
            };
        }

        protected override void SeedMockEntityList(IList<ExceptionLog> mockEntityList)
        {

            IList<ExceptionLog> createdList = CreateSampleEntityList(10, 100);
            for (int i = 0; i < createdList.Count; i++)
            {
               
                mockEntityList.Add(createdList[i]);
            }
        }
    }
}
