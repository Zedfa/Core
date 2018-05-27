using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Cmn.Extensions.Interfaces
{
    public interface IDbContextBaseExtentions
    {
        string GetTableName<T>(IDbContextBase context) where T : ObjectBase;
        string GetSchemaName<T>(IDbContextBase context) where T : ObjectBase;
        List<string> GetKeyColumnNames<T>(IDbContextBase context) where T : ObjectBase;
    }
}
