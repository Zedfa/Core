using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Cmn
{
    public interface IModelBase<T> where T : IEntity
    {
        T Model
        {
            get;
        }

        // [JsonIgnore]
        T GetModel();

        IModelBase<T> SetModel(T model);

        IEntity GetObjectModel();
    }
}
