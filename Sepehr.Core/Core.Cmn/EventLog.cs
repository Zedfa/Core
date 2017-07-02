using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Cmn
{
    public class EventLog : IEventLog
    {
        
        public EventLog()
        {
            //LogType = ConfigHelper.GetConfigValue("ApplicationNameForLog");//System.AppDomain.CurrentDomain.FriendlyName;
            LogType = ConfigHelper.GetConfigValue<string>("ApplicationNameForLog");//System.AppDomain.CurrentDomain.FriendlyName;
            CustomMessage = "";
            UserId = "";
        }
           
      
       private string    _logFileName      ;  
   

       public Exception OccuredException        { get ; set; }
       public string UserId                     { get; set; } 
       public string CustomMessage              { get; set; }
       public string LogFileName
       { 
           get
           {
               return _logFileName;
           }
           set
           {
               //TODO:Put the hardcoded value "Log" in a proper Resource file.
               _logFileName = "Log";
           }
       }
       //public bool LogEx
       //{
       //    get
       //    {
       //        return _logEx;
       //    }
       //    set
       //    {
       //        if (_logEx)
       //            _logEx = true;
       //        else
       //            _logEx = value;
       //    }
       //}
      
       public bool ThrowEx   { get; set; }

        /// <summary>
        /// In this context,is Equivalent of Currently name of the Executing Assembly.(Program)
        /// </summary>
       public string LogType   { get; private set; }

       public DateTime? CreateDate { get; set; }

    }
}
