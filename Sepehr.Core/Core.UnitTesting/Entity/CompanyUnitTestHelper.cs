using Core.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Core.UnitTesting.Entity
{
    public class CompanyUnitTestHelper : EntityUnitTestHelperBase<Company>
    {
        public override string TableName { get { return "core.Companies"; } }

        public override void AssertEntitiesAreEqual(Company expected, Company actual)
        {
            Assert.IsNotNull(expected);
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Active, actual.Active);
            Assert.AreEqual(expected.Address, actual.Address);
            Assert.AreEqual(expected.Code, actual.Code);
            Assert.AreEqual(expected.Family, actual.Family);
            Assert.AreEqual(expected.FatherName, actual.FatherName);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.NationalId, actual.NationalId);
            Assert.AreEqual(expected.Phone, actual.Phone);
            Assert.AreEqual(expected.Title, actual.Title);
        }

        public override Company CreateSampleEntity()
        {
            Company entity = BuildSampleEntity<Company>();
            return entity;
        }

        public override Expression<Func<Company, bool>> GetFindPredicate(Company entity)
        {
            return e =>
                e.Active == entity.Active
                && e.Address == entity.Address
                && e.Code == entity.Code
                && e.Family == entity.Family
                && e.FatherName == entity.FatherName
                && e.Name == entity.Name
                && e.NationalId == entity.NationalId
                && e.Phone == entity.Phone
                && e.Title == entity.Title
                ;
        }

        public override Expression<Func<Company, bool>> GetFindByIdPredicate(Company entity)
        {
            return e => e.Id == entity.Id;
        }

        public override ExpressionInfo GetFilterExpressionInfo(Company entity)
        {
            return new ExpressionInfo
            {
                CurrentPage = 0,
                PageSize = 10,
                Expression = new KeyValuePair<string, string>("Id", entity.Id.ToString())
            };
        }

        public override void EditSampleEntity(Company entity)
        {
            entity.Active = GetRandomBoolean();
            entity.Address = GetRandomString();
            entity.Code = GetRandomString();
            entity.Family = GetRandomString();
            entity.FatherName = GetRandomString();
            entity.Name = GetRandomString();
            entity.NationalId = GetRandomString();
            entity.Phone = GetRandomString();
            entity.Title = GetRandomString();
        }

        public override Expression<Func<Company, Company>> GetUpdatePredicate(Company entity)
        {
            return item => new Company
            {
                Active = entity.Active,
                Address = entity.Address,
                Code = entity.Code,
                Family = entity.Family,
                FatherName = entity.FatherName,
                Name = entity.Name,
                NationalId = entity.NationalId,
                Phone = entity.Phone,
                Title = entity.Title
            };
        }

        protected override void SeedMockEntityList(IList<Company> mockEntityList)
        {
            IList<Company> createdList = CreateSampleEntityList(10, 100);
            for (int i = 0; i < createdList.Count; i++)
            {
                createdList[i].Id = i + 1;
                mockEntityList.Add(createdList[i]);
            }
        }
    }
}