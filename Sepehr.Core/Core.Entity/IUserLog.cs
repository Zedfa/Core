namespace Core.Entity
{
    public interface IUserLog
    {
        string GetIpAddress();
        string GetUserName();
        int? GetCompanyId();
    }

}
