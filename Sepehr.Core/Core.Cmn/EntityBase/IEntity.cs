using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Cmn
{
    public interface IEntity
    {
        [IgnoreDataMember]
        object this[string propertyName] { get; set; }
        event PropertyChangedEventHandler NavigationPropertyChangedByCache;
    }
}
