using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Cmn;

namespace Core.Entity
{
    [Table("ExceptionLogs", Schema = "core")] 

    public class ExceptionLog : EntityBase<ExceptionLog>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public System.Guid ID { get; set; }
        public string ExceptionType { get; set; }
        public string Message { get; set; }
        public String StackTrace { get; set; }
        public string Source { get; set; }
        public virtual ExceptionLog InnerException { get; set; }
    }
}
