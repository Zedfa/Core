using Core.Cmn;
using Core.Entity.Enum;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity
{
    [Table("UserLogs", Schema = "core")]

    public class CoreUserLog : EntityBase<CoreUserLog>
    {
        public int Id { get; set; }
        public string Ip { get; set; }

        public virtual TableInfo TableInfo { get; set; }
        public int? TableNameId { get; set; }
        public string RecordId { get; set; }
        public DateTime DateTime { get; set; }
        public virtual User User { get; set; }
        public int UserId { get; set; }
        public LogType LogType { get; set; }
        public dynamic changedTable { get; set; }
    }
}
