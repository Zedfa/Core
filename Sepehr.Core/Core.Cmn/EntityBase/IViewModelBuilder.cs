using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Cmn.EntityBase
{
    interface IViewModelBuilder<T> where T : ObjectBase  , new()
    {
        IModelBase<T> GetViewModel<ViewModel>(T model);
    }
}
