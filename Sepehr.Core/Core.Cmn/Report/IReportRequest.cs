using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Cmn.Report
{
    public interface IReportRequest
    {
        string FileName { get; set; }

        string Language { get; set; }
    }
}
