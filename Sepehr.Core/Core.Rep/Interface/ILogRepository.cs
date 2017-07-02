using Core.Entity;
using Core.Repository.Interface;
using System;

namespace Core.Rep
{
    public interface ILogRepository : IRepositoryBase<Log>
    {
        ExceptionLog GetExceptionLog(Guid logId);
        ExceptionLog GetExceptionLogOfThisParent(Guid parentId);
        bool HasAnyChildren(Guid Id);
    }
}
