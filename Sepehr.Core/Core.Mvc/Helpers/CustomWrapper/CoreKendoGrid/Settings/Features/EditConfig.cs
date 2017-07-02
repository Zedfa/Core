using Kendo.Mvc;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Mvc.Helpers.CoreKendoGrid.Settings.Features
{
    [Serializable()]
    public class EditConfig
    {
        public EditConfig()
        {
            CustomConfig = new EditCustomConfig();
            Editable = true;
        }
        public bool Editable { get; set; }
        public EditCustomConfig CustomConfig { get; set; }
    }
}
