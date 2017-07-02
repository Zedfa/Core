using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Cmn;

namespace Core.Entity
{
    [Table("Logs", Schema = "core")] 

    public class Log : EntityBase<Log>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public System.Guid ID { get; set; }
        public string UserId { get; set; }
        public string CustomMessage { get; set; }
        public DateTime CreateDate { get; set; }
        public int InnerExceptionCount { get; set; }
        public string LogType { get; set; }
        public virtual ExceptionLog ExceptionLog { get; set; }
    }
}
