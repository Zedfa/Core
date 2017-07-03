using System;
using System.Collections.Generic;

namespace Core.Serialization.ObjectProxy
{
    public interface IObjectProxy
    {
        List<IPropertyProxy> ProxyPropertyList { get; }
        Type Type { get; }

        IObjectProxy Copy();

        object CreateObject();
    }

    public interface IPropertyProxy
    {
        Type PropertyType { get; }
        string ProperyName { get; }

        IPropertyProxy Copy();

        object GetProperty(object obj);

        void SetProperty(object obj, object value);
    }
}