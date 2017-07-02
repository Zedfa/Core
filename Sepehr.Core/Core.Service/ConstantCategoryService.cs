using Core.Cmn.Attributes;
using Core.Entity;
using Core.Cmn;
using Core.Rep;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    [Injectable(InterfaceType = typeof(IConstantCategoryService), DomainName = "Core")]
    public class ConstantCategoryService : ServiceBase<ConstantCategory>, IConstantCategoryService
    {
        public ConstantCategoryService():base()
        {

        }
        public ConstantCategoryService(IDbContextBase context)
            : base(context)
        {
            _repositoryBase = new ConstantCategoryRepository(context);

        }
    }
}
