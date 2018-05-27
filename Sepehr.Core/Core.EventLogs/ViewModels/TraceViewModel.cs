using Core.Cmn.Trace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace Core.EventLogs.ViewModels
{
    class TraceViewModel
    {

        public TraceViewModel(TraceDto trace)
        {
            Data = trace;
        }

        private TraceDto Data { get; set; }

        public int Id { get { return Data.Id; } }

        public string Message { get { return Data.Message; } }

        public DateTime SystemTime { get { return Data.SystemTime ; } }

        public string TraceKey { get { return Data.TraceKey; } }

        private Brush _levelColor;
        public Brush LevelColor
        {
            get
            {
                switch (Data.Level)
                {
                    case 1:
                        return _levelColor = Brushes.DarkRed;
                    case 2:
                        return _levelColor = Brushes.Red;
                    case 3:
                        return _levelColor = Brushes.LightYellow;
                    default:
                        return _levelColor = Brushes.SkyBlue;
                }
            }
           

        }
        private string _levelText;
        public string LevelText
        {
            get
            {
                switch (Data.Level)
                {
                    case 1:
                        return _levelText = "Error";

                    case 2:
                        return _levelText = "Information";
                    case 3:
                        return _levelText = "Warning";

                    default:
                        return _levelText = "Information";
                }
            }
           

        }
    }
}
