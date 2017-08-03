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
       
        public int Id { get; set; }
        //public string UserId { get; set; }
        public string CustomMessage { get; set; }
        public DateTime CreateDate { get; set; }
        public int InnerExceptionCount { get; set; }
        //public string LogType { get; set; }
        public virtual ExceptionLog ExceptionLog { get; set; }
        /// <summary>
        /// fill automatically or manually
        /// </summary>
        public string IP { get; set; }
        /// <summary>
        /// fill in compile time automatically
        /// </summary>
        public string Source { get; set; }
        public string ClientPlatform { get; set; }
        /// <summary>
        /// fill from config file by "ApplicationNameForLog" key 
        /// </summary>
        public string ApplicationName{ get; set; }



    }
}
