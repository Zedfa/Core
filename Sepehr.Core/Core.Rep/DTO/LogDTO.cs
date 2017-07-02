using Core.Cmn;
using Core.Cmn.EntityBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Core.Rep.DTO
{
    public class LogDTO : EntityBase<LogDTO> , IDto //DtoBase<LogDTO>
    {
        public System.Guid ID                  { get; set; }
        public string      UserId              { get; set; }
        public string      CustomMessage       { get; set; }
        public DateTime    CreateDate          { get; set; }
        public string LogType { get; set; }
        public int         InnerExceptionCount { get; set; }

        //public string InnerExceptionType { get; set; }

        //public string Inner { get; set; }
       // public  ExceptionLog ExceptionLog { get; set; }
    }
}