using Core.Mvc.Helpers.CustomWrapper.DataSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mvc.Helpers.CustomWrapper.DataModel
{
    [Serializable()]
    public class ServerRelInfo
    {
        public DataSourceType DSType { get; set; }
        public bool? Batch { get; set; }
        public bool? ServerPaging { get; set; }
        public bool? ServerSorting { get; set; }
        public bool? ServerFiltering { get; set; }
    }
}
