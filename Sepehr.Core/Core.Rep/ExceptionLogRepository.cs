using Core.Cmn;
using Core.Entity;

using Core.Rep.DTO;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Core.Rep
{
    public class ExceptionLogRepository : RepositoryBase<ExceptionLog>, IExceptionLogRepository
    {
        public ExceptionLogRepository(IDbContextBase dbContextBase)
            : base(dbContextBase)
        {

        }

        protected override void SetQueryable(IQueryable<ExceptionLog> queryable)
        {
            //DtoQueryableDelegate = quer => {
            //    quer = All();
            //}
            QueryableDtos = queryable.Select(
                                el =>
                                new ExceptionLogDTO()
                                {
                                    ID = el.ID,
                                    ExceptionType = el.ExceptionType,
                                    Message = el.Message,
                                    StackTrace = el.StackTrace,
                                    Source = el.Source  //,
                                    //ExceptionLog = el.InnerException
                                }
                                
                                );
        }
    }

}