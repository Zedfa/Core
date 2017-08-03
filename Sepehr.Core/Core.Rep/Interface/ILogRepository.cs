using Core.Entity;
using Core.Repository.Interface;
using System;

namespace Core.Rep
{
    public interface ILogRepository : IRepositoryBase<Log>
    {
        ExceptionLog GetExceptionLog(int logId);
        ExceptionLog GetExceptionLogOfThisParent(int parentId);
        bool HasAnyChildren(int Id);
    }
}
