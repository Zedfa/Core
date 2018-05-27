using Core.Cmn;
using Core.Cmn.EntityBase;
using Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repository
{
    public interface IDTOQueryableBuilder<T> where T : ObjectBase 
    {
        IQueryable<IDto> GetDtoQueryable(IQueryable<T> queryable);
    }
}
