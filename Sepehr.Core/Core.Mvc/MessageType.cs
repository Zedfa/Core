using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Core.Mvc
{
    public enum MessageType
    {
        error ,
        success ,
        warning ,
        info 
    }
    public class Message
    {
        public string text { get; set; }
        public MessageType type { get; set; }
        
    }
}