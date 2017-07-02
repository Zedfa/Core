namespace Kendo.Mvc.UI
{    
    using System.Web.Script.Serialization;

    public class DropDownListItem
    {
        public string Text { get; set; }
        public string Value { get; set; }
        [ScriptIgnore]
        public bool Selected { get; set; }
    }
}
