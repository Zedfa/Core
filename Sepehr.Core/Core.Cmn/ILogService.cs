using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Core.Cmn
{
    public interface ILogService
    {
        /// <summary>
        ///  1.zamani ke mikhaym chizi ro mahze etela dar log sabt konim az method e write estefade mikonim
        ///  2.zamani ke mikhaym exception dar log sabt beshe az method e handle estefade mikonim
        ///  3. faghat zamani ke mikhayd file, method , line , applicationName , source ro khodetoon poor konid az override Handle(LogInfo logInfo) estefade mikonim
        ///  4. log bardari be 2sorat anjam mishavad : sabt dar jadval logs , exceptionlogs dar sql  va agar natavanest sabt dar file xml(  makane save ro kilid e  LogPath dar config file moshakhas mikonad) 
        /// </summary>
        /// <param name="customMessage"> too field e CustomMessage gharar migirad</param>
        /// <param name="raiseThrowException">agar true bashad throw exception raise mikonad </param>
        /// <param name="source"> az tarkibe meghdari ke developer mide-agar bedahad- + (file, method , line)</param>
        /// <param name="ip">too field e IP gharar migirad</param>
        /// <param name="file ,method,line"> har se motagheyer be sorate automatic poor mishe.aslan niaz be por kardan nadarad. </param>
        /// 
        void Write(string customMessage, [CallerFilePath] string file = null, [CallerMemberName] string method = null, [CallerLineNumber] int line = 0);
        void Write(string customMessage, string ip, [CallerFilePath] string file = null, [CallerMemberName] string method = null, [CallerLineNumber] int line = 0);

        Exception Handle(Exception exception, string customMessage, string source, [CallerFilePath] string file = null, [CallerMemberName] string method = null, [CallerLineNumber] int line = 0);

        Exception Handle(Exception exception, string customMessage, bool raiseThrowException, string source, string platform, [CallerFilePath] string file = null, [CallerMemberName] string method = null, [CallerLineNumber] int line = 0);

        Exception Handle(Exception exception, string customMessage, bool raiseThrowException, string source, [CallerFilePath] string file = null, [CallerMemberName] string method = null, [CallerLineNumber] int line = 0);

        Exception Handle(Exception exception, string customMessage, [CallerFilePath] string file = null, [CallerMemberName] string method = null, [CallerLineNumber] int line = 0);

        Exception Handle(string ip, Exception exception, string customMessage, [CallerFilePath] string file = null, [CallerMemberName] string method = null, [CallerLineNumber] int line = 0);
        Exception Handle(LogInfo logInfo);

        void BatchHandle(List<LogInfo> eventLogs);
    }
}
