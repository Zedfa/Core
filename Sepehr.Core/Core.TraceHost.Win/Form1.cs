using Core.Cmn;
using Core.Service.Trace;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Core.TraceHost.WinForm
{
    public partial class Form1 : Form
    {
        //private static ITraceWriter _traceWriter;
        private static int counter;
        public Form1()
        {
           // Core.Cmn.AppBase.StartApplication();

         //   _traceWriter = new TraceWriterService();  //  Core.Cmn.AppBase.TraceViewer; 
            
            InitializeComponent();

            btnStart.Enabled = true;
            btnOpenPort.Enabled = false;
            btnCreateFakeTrace.Enabled = false;

        }

        //private void btnCreateFakeTrace_Click(object sender, EventArgs e)
        //{
           
        //    Stopwatch timer = new Stopwatch();
        //    timer.Start();

        //    for (int i = 0; i < 10000; i++)
        //    {
        //        var platformExp = new PlatformNotSupportedException();
        //        platformExp.Data.Add("p1", "dddd");
        //        platformExp.Data.Add("p2", "dddd");
        //        platformExp.Data.Add("p3", "dddd");

        //        var exp = new Exception("exception test in container ", new NotImplementedException("not implemented", platformExp));
        //        exp.Data.Add("a", "1");
        //        exp.Data.Add("b", "20000");
        //        exp.Data.Add("c", "20000");

        //        _traceWriter.Exception(exp, "Container Test");

        //        var source = new Core.Cmn.Trace.TraceDto { Message = "test submit data from Source", TraceKey = "Container Test" };
        //        source.Data.Add("d1", "ttttt");
        //        source.Data.Add("d2", "ttttt");
        //        source.Data.Add("d3", "ttttt");

        //        _traceWriter.SubmitData(source);

        //        _traceWriter.Failure($"test for fail {DateTime.Now.ToLongDateString()} {Guid.NewGuid()}");

        //        _traceWriter.Inform($"test for information");

        //        _traceWriter.Attention($"test for warning");


        //    }
        //    timer.Stop();


        //    if (counter== 1)
        //    {
        //        var source = new Core.Cmn.Trace.TraceDto { Message = "test submit data from Source==> count= 1 ", TraceKey = "TraceHostTest" };
        //        source.Data.Add("M", "a");
        //        source.Data.Add("a", "aa");
        //        source.Data.Add("r", "aaa");
        //        source.Data.Add("i", "98745855");
        //        source.Data.Add("aa", "25");

        //        _traceWriter.SubmitData(source);
        //    }

        //    if (counter == 2)
        //    {
        //        var source = new Core.Cmn.Trace.TraceDto { Message = "test submit data from Source", TraceKey = "TraceHostTest" };
        //        source.Data.Add("y", "25");

        //        _traceWriter.SubmitData(source);
        //    }
        //    counter++;
        //    MessageBox.Show(timer.Elapsed.ToString());
        //}

        private void btnOpenPort_Click(object sender, EventArgs e)
        {
            TraceHostService.Start();
            btnOpenPort.Enabled = false;
            btnCreateFakeTrace.Enabled = true;

        }

        private void btnStart_Click(object sender, EventArgs e)
        {              

            ETWRegistrantService etwService = new ETWRegistrantService();
            etwService.Start();
          //  Thread.Sleep(10000);
            btnStart.Enabled = false;
            //btnOpenPort.Enabled = true;

        }
    }
}
