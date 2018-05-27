using Core.Cmn;
using Core.Cmn.EntityBase;
using Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Core.Repository
{
    public class DTOQueryableConformer<T> : IDTOQueryableBuilder<T> where T : ObjectBase 
    {
        public IQueryable<IDto> GetDtoQueryable(IQueryable<T> queryable)
        {
            throw new NotImplementedException();
        }
    }
}