using Core.Cmn.Extensions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Cmn.Extensions
{
    public static class IDbContextExt
    {
        private static IDbContextBaseExtentions _iDbContextBaseExtentions;
        public static IDbContextBaseExtentions IDbContextBaseExtentions
        {
            get
            {

                if (_iDbContextBaseExtentions == null)
                {
                    _iDbContextBaseExtentions = AppBase.DependencyInjectionManager.Resolve<IDbContextBaseExtentions>();
                }

                return _iDbContextBaseExtentions;
            }
        }
        public static List<string> GetKeyColumnNames<T>(this IDbContextBase context) where T : ObjectBase
        {
            return IDbContextBaseExtentions.GetKeyColumnNames<T>(context);
        }

        public static string GetSchemaName<T>(this IDbContextBase context) where T : ObjectBase
        {
            return IDbContextBaseExtentions.GetSchemaName<T>(context);
        }

        public static string GetTableName<T>(this IDbContextBase context) where T : ObjectBase
        {
            return IDbContextBaseExtentions.GetTableName<T>(context);
        }
    }
}
