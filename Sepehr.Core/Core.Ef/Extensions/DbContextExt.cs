using Core.Cmn.Extensions;
using Core.Cmn;
using System;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Core.Cmn.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Ef.Extensions
{
    public static class IDbContextExt
    {
        public static string GetTableName<T>(this IDbContextBase context) where T : Core.Cmn.EntityBase<T>
        {
            string tableNameResult = null;
            ObjectContext objectContext = ((IObjectContextAdapter)context).ObjectContext;
            Type entityType = typeof(T);

            if (entityType.BaseType != null && entityType.Namespace == "System.Data.Entity.DynamicProxies")
                entityType = entityType.BaseType;

            string entityTypeName = entityType.Name;

            EntityContainer container =
                objectContext.MetadataWorkspace.GetEntityContainer(objectContext.DefaultContainerName, DataSpace.CSpace);
            var entitySet = (from meta in container.BaseEntitySets
                             where meta.ElementType.Name == entityTypeName
                             select meta).First();
            if (entitySet.MetadataProperties.Contains("Configuration") && entitySet.MetadataProperties["Configuration"].Value != null)
            {
                var tableName = ((entitySet.MetadataProperties["Configuration"].Value)).ToDictionary()["TableName"];
                if (tableName == null)
                    tableNameResult = entitySet.Name;
                else
                    tableNameResult = tableName.ToString();
            }
            else
            {
                tableNameResult = entityType.Name;
            }

            return tableNameResult;
        }

        public static string GetSchemaName<T>(this IDbContextBase context) where T : Core.Cmn.EntityBase<T>
        {

            ObjectContext objectContext = ((IObjectContextAdapter)context).ObjectContext;
            Type entityType = typeof(T);

            if (entityType.BaseType != null && entityType.Namespace == "System.Data.Entity.DynamicProxies")
                entityType = entityType.BaseType;

            string entityTypeName = entityType.Name;

            EntityContainer container =
                objectContext.MetadataWorkspace.GetEntityContainer(objectContext.DefaultContainerName, DataSpace.CSpace);
            var entitySetName = (from meta in container.BaseEntitySets
                                 where meta.ElementType.Name == entityTypeName
                                 select meta).First();

            var schemaName = "dbo";

            if (entitySetName.MetadataProperties.Contains("Configuration") && entitySetName.MetadataProperties["Configuration"].Value != null)
            {
                var schema = ((entitySetName.MetadataProperties["Configuration"].Value)).ToDictionary()["SchemaName"];
                if (schema == null)
                    schemaName = "dbo";
                else
                    schemaName = schema.ToString();
            }
            return schemaName;
        }
        public static string GetKeyColumnName<T>(this IDbContextBase context) where T : Core.Cmn.EntityBase<T>
        {
            System.Data.Entity.Core.Objects.ObjectContext objectContext = ((IObjectContextAdapter)context).ObjectContext;
            var set = objectContext.CreateObjectSet<T>();
            return set.EntitySet.ElementType
                        .KeyMembers
                         .Select(k => k.Name).FirstOrDefault();


        }
    }
}
