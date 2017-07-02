using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace Core.Cmn
{
    public static class ExceptionExt
    {
        /// <summary>
        /// bekhatere inke ma bazi jaha mikhaim message haye custome khodemon ro toye exception return konim
        /// vali az tarafi mikhaim ke stack trace ro az dast nadim, in method neveshte shode
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="message">
        /// In message ezafe mishe be source exception
        /// </param>
        public static void Throw(this Exception exception, string message)
        {
            ExceptionDispatchInfo exceptionDispatchInfo = ExceptionDispatchInfo.Capture(exception);
            exceptionDispatchInfo.SourceException.Source = $"{message} {Environment.NewLine} {exceptionDispatchInfo.SourceException.Source}";
            exceptionDispatchInfo.Throw();
        }
    }
}
