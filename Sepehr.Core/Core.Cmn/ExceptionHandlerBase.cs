using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Core.Cmn
{
    public static class ExceptionHandlerBase
    {

        static ExceptionHandlerBase()
        {
           // AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        //public static void asdf()
        //{
        //    AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        //    AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;
        //    Application.ThreadException += Application_ThreadException;
        //}

        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            
        }
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  
        static void CurrentDomain_FirstChanceException(object sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
        {
           
        }

        public static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {

        }
    }
}
