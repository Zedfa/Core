using Core.Mvc.Helpers.CustomWrapper.Infrastructure;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Mvc.Helpers.CustomWrapper.CoreKendoGrid.Settings.Features
{
    [Serializable]
    public class FileOutput : JsonObjectBase
    {
        public FileOutput()
        {
            Rtl = true;
            AllPages = true;
            PaperSize = "A4";
            Margin = new Margin { Top = "3cm", Right = "1cm", Bottom = "1cm", Left = "1cm" };
            
        }
        public OutputType OutputType { get; set; }
        public string FileName { get; set; }

        public string ButtonText { get; set; }

        public bool AllPages { get; set; }

        public bool Rtl { get; set; }

        public bool Filterable { get; set; }

        public string PaperSize { get; set; }

        public Margin Margin { get; set; }

        protected override void Serialize(IDictionary<string, object> json)
        {
            json["allPages"] = AllPages;
            json["fileName"] = $"{FileName}.pdf";
            json["paperSize"] = PaperSize;
            json["margin"] = Margin;

        }
    }
    
}
