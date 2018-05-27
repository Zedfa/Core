using Core.Cmn.Extensions;
using Core.Cmn;
using System;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Core.Cmn.Attributes;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using EntityFramework.MappingAPI.Extensions;
using System.Collections.Generic;
using Core.Cmn.Extensions.Interfaces;

namespace Core.Ef.Extensions
{
    [Injectable(InterfaceType = typeof(IDbContextBaseExtentions))]
    public class IDbContextBaseExtentionsForEf : IDbContextBaseExtentions
    {
        public List<string> GetKeyColumnNames<T>(IDbContextBase context) where T : ObjectBase 
        {
            return (context as DbContext).Db(typeof(T)).Pks.Select(p => p.ColumnName).ToList();
        }

        public string GetSchemaName<T>(IDbContextBase context) where T : ObjectBase 
        {
            var schemaName = (context as DbContext).Db(typeof(T)).Schema;
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

        public string GetTableName<T>(IDbContextBase context) where T : ObjectBase 
        {
            var tableNameResult = (context as DbContext).Db(typeof(T)).TableName;
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
