using Core.Cmn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Core.Mvc
{
    public class MvcExceptionInfo : ExceptionInfo
    {
        public string Details { get; set; }
        public bool IsRTL { get; set; }

        private int? _statusCode;
        public int? StatusCode
        {
            get
            {
                if (_statusCode == null)
                    _statusCode = 500;

                return _statusCode;
            }
            set
            {
                _statusCode = value.Value;
            }
        }
        public MvcExceptionInfo(string message, int status, bool isRTL) : base(message)
        {
            this.Message = message;
            this.StatusCode = status;
            this.IsRTL = isRTL;
        }
        public MvcExceptionInfo(string message, bool isRTL = true) : base(message)
        {
            this.Message = message;
            this.IsRTL = isRTL;
        }
        public MvcExceptionInfo(Exception excp, bool isRTL = true) : base(excp)
        {
            this.Message = excp.Message;
            this.Details = excp.InnerException != null ? excp.InnerException.Message : string.Empty;
            this.StackTrace = excp.StackTrace;
            this.IsRTL = isRTL;
            Source = excp.Source;
        }
    }
}