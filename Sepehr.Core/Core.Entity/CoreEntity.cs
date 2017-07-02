using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity
{
    [Serializable]
    [DataContract]
    public class CoreEntity<T> : Core.Cmn.EntityBase<T> where T : Core.Cmn.EntityBase<T>, new()
    {
        public int? CurrentCompanyId { get; set; }
    }
}
