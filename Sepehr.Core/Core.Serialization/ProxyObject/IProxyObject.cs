using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core.Serialization.ObjectProxy
{
    public interface IObjectProxy
    {
        Type Type { get; }
        object CreateObject();
        List<IPropertyProxy> ProxyPropertyList { get; }
        IObjectProxy Copy();
    }

    public interface IPropertyProxy
    {
        Type PropertyType { get; }
        string ProperyName { get; }
        void SetProperty(object obj, object value);
        object GetProperty(object obj);
        IPropertyProxy Copy();
    }
}
