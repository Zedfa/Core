using Core.Cmn;
using Core.Entity;

using System;
using System.Collections.Generic;



namespace Core.Rep
{
    public class ConstantCategoryRepository : RepositoryBase<ConstantCategory>, IConstantCategoryRepository
    {

        IDbContextBase _dc;
        public ConstantCategoryRepository()
            : base()
        {

        }
        public ConstantCategoryRepository(IDbContextBase dc)
            : base(dc)
        {
            _dc = dc;
        }

    }
}
