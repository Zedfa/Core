using System;
using System.Data.SqlClient;

namespace Core.Ef
{
    public class NotifierErrorEventArgs : EventArgs
    {
        public string Sql { get; set; }
        public SqlNotificationEventArgs Reason { get; set; }
    }
}
