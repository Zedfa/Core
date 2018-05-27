using Core.Cmn;
using Core.Cmn.Trace;
using Core.Service.Trace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.TraceViewer
{
    public class TraceListManager
    {

        private static List<TraceDto> _data;
        //private static Dictionary<string, string> _displayCoumns;

        //public static event EventHandler ColumnChangedEvent;

        public static event EventHandler DataChangedEvent;

        private static ITraceViewer _traceViewer;
        private static ITraceViewer TraceViewer
        {
            get
            {
                _traceViewer = _traceViewer ?? new TraceViewerService();
                return _traceViewer;
            }
        }
        public static List<TraceDto> Data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value?.OrderByDescending(item => item.SystemTime).ToList();

                OnDataChanged(EventArgs.Empty);
            }
        }


        public static void RefreshData(FilterBaseModel filter)
        {
            var filterStr = filter.FormattedFilter;

            if (string.IsNullOrEmpty(filter.Writer.Operand))
            {
                Data = TraceViewer.GetTracesViaWCF(filterStr);
            }
            else
            {
                Data = TraceViewer.GetTracesByWriterViaWCF(filterStr, filter.Writer.Operand);
            }
        }

        public static List<string> GetWriters()
        {
            return TraceViewer.GetTraceWritersViaWCF();
        }
        //private static void OnColumnChanged(EventArgs e)
        //{
        //    ColumnChangedEvent?.Invoke(null, e);
        //}
        public static void DeleteRange(DateTime startDate, DateTime endDate, string writer)
        {
            TraceViewer.DeleteWriterTraces(startDate, endDate, writer);
        }
        private static void OnDataChanged(EventArgs e)
        {
            DataChangedEvent?.Invoke(null, e);
        }
    }
}
