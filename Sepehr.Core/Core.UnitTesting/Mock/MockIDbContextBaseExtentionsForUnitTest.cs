using Core.Cmn;
using Core.Cmn.Attributes;
using Core.Cmn.Extensions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.UnitTesting.Mock
{
    [Injectable(InterfaceType = typeof(IDbContextBaseExtentions), Version = 1000)]
    public class MockIDbContextBaseExtentionsForUnitTest : IDbContextBaseExtentions
    {
        public List<string> GetKeyColumnNames<T>(IDbContextBase context) where T : EntityBase<T>
        {
            return new List<string> { "test" };
        }

        public string GetSchemaName<T>(IDbContextBase context) where T : EntityBase<T>
        {
            var schemaName = "test";
            //ObjectContext objectContext = ((IObjectContextAdapter)context).ObjectContext;
            //Type entityType = typeof(T);

            //if (entityType.BaseType != null && entityType.Namespace == "System.Data.Entity.DynamicProxies")
            //    entityType = entityType.BaseType;

            //string entityTypeName = entityType.Name;

            //EntityContainer container =
            //    objectContext.MetadataWorkspace.GetEntityContainer(objectContext.DefaultContainerName, DataSpace.CSpace);
            //var entitySetName = (from meta in container.BaseEntitySets
            //                     where meta.ElementType.Name == entityTypeName
            //                     select meta).First();

            //var schemaName = "dbo";

            //if (entitySetName.MetadataProperties.Contains("Configuration") && entitySetName.MetadataProperties["Configuration"].Value != null)
            //{
            //    var schema = ((entitySetName.MetadataProperties["Configuration"].Value)).ToDictionary()["SchemaName"];
            //    if (schema == null)
            //        schemaName = "dbo";
            //    else
            //        schemaName = schema.ToString();
            //}
            return schemaName;
        }

        public string GetTableName<T>(IDbContextBase context) where T : EntityBase<T>
        {
            var tableNameResult = "test";
            //string tableNameResult = null;
            //ObjectContext objectContext = ((IObjectContextAdapter)context).ObjectContext;
            //Type entityType = typeof(T);

            //if (entityType.BaseType != null && entityType.Namespace == "System.Data.Entity.DynamicProxies")
            //    entityType = entityType.BaseType;

            //string entityTypeName = entityType.Name;

            //EntityContainer container =
            //    objectContext.MetadataWorkspace.GetEntityContainer(objectContext.DefaultContainerName, DataSpace.CSpace);
            //var entitySet = (from meta in container.BaseEntitySets
            //                 where meta.ElementType.Name == entityTypeName
            //                 select meta).First();
            //if (entitySet.MetadataProperties.Contains("Configuration") && entitySet.MetadataProperties["Configuration"].Value != null)
            //{
            //    var tableName = ((entitySet.MetadataProperties["Configuration"].Value)).ToDictionary()["TableName"];
            //    if (tableName == null)
            //        tableNameResult = entitySet.Name;
            //    else
            //        tableNameResult = tableName.ToString();
            //}
            //else
            //{
            //    tableNameResult = entityType.Name;
            //}

            return tableNameResult;
        }
    }
}
