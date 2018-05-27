using Core.Mvc.Attribute.Validation;
using Core.Mvc.Helpers.CustomWrapper.Infrastructure;
using Core.Mvc.Helpers.CustomWrapper.SearchRelated;
using Core.Mvc.Helpers.Lookup;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Mvc.Helpers.CoreKendoGrid
{
    [Serializable()]
    public class Column : JsonObjectBase
    {
        public Column()
        {
            Sortable = true;
            Filterable = false;
            Groupable = false;
            Encoded = true;
            Searchable = true;
            //SearchInfo = new KeyValuePair<string, ISearch>();
        }

        public Dictionary<AggregateType, string> Aggregates { get; set; }
        public string Title { get; set; }
        public string Field { get; set; }
        public string Format { get; set; }
        public bool Sortable { get; set; }
        public bool Filterable { get; set; }
        public bool Groupable { get; set; }

        public bool Searchable { get; set; }

        public string ConstantsCategoryName { get; set; }

        //private List<IValidationRule> _validationRules;
        //public List<IValidationRule> ValidationRules {
        //    get {
        //        return _validationRules ?? new List<IValidationRule>();
        //}
        //    set {
        //        _validationRules.AddRange( value);
        //    }
        //}
        public List<ValidationRuleInfo> ValidationRules { get; set; }

        public List<ColumnCommand> Commands { get; set; }
        public string GridID { get; set; }

        /// <summary>
        /// None encoded string which can be used to hold Html elements.
        /// </summary>
        public string Template { get; set; }//

        /// <summary>
        /// When Grid scrolling is enabled (by default), the Grid table layout style is "fixed".
        /// This means that all width-less columns will be equally wide no matter what their content is.
        /// When Grid scrolling is disabled, the Grid table layout style is "auto", i.e. the column widths
        /// are determined by the browser and cell content, if not set explicitly
        /// </summary>
        public string Width { get; set; }

        private Dictionary<string, object> GetValidationRules()
        {
            var validationDic = new Dictionary<string, object>();
            if (ValidationRules != null)
            {
                ValidationRules.ForEach(rules =>
                {
                    //if (vr is RequiredValidation)
                    //{
                    //    var valRule = vr as RequiredValidation;
                    //    valDic.Add("RequiredValidator", valRule.ToJson());
                    //}
                    var name = rules.Name;
                    var instance = rules.ToJson();
                    validationDic.Add(name, instance);
                });
            }

            return validationDic;
        }

        protected override void Serialize(IDictionary<string, object> json)
        {
            if (Commands == null)
            {
                json["field"] = string.IsNullOrEmpty(Field) ? string.Empty : Field;
                if (!string.IsNullOrEmpty(Format))
                {
                    json["format"] = Format;
                }
            }
            else
            {
                json["command"] = DefineCustomCommand(Commands);
            }
            if (!string.IsNullOrEmpty(Template))
            {
                json["template"] = Template;
            }

            json["title"] = string.IsNullOrEmpty(Title) ? string.Empty : Title;

            json["width"] = string.IsNullOrEmpty(Width) ? string.Empty : Width;

            json["hidden"] = Hidden;

            json["sortable"] = Sortable;

            json["searchable"] = Searchable;

            json["filterable"] = Filterable;

            json["groupable"] = Groupable;

            json["encoded"] = Encoded;

            json["editor"] = null;

            json["isCurrency"] = IsCurrency;

            json["validationRules"] = GetValidationRules();

            if (this.Aggregates != null)
            {
                if (this.Aggregates.Count > 0)
                {
                    json["aggregates"] = this.Aggregates.Select(a => a.ToString().ToLower());
                }
            }
            if (!string.IsNullOrEmpty(this.ClientFooterTemplate))
            {
                json["footerTemplate"] = this.ClientFooterTemplate;
            }
        }

        private object DefineCustomCommand(List<ColumnCommand> Commands)
        {
            var dicList = new List<IDictionary<string, object>>();
            Commands.ForEach(c => { c.GridID = this.GridID; dicList.Add(c.ToJson()); });
            return dicList;
        }

        public string ClientFooterTemplate
        {
            get;
            set;
        }

        public void DefineCustomCommand()
        {
        }

        public string ClientTemplate
        {
            get;
            set;
        }

        public bool Encoded
        {
            get;
            set;
        }

        public bool Hidden
        {
            get;
            set;
        }

        public Dictionary<string, string> HtmlAttributes
        {
            get;
            private set;
        }

        public bool IncludeInMenu
        {
            get;
            set;
        }

        public bool IsLast
        {
            get;
            private set;
        }

        public bool Visible
        {
            get;
            set;
        }

        public bool CustomCommand { get; set; }

        public ILookupView LookupViewInfo { get; set; }

        public DropDownListInfo DropDownListInfo { get; set; }

        public AutoCompleteInfo AutoCompleteInfo { get; set; }

        public Type Type { get; set; }

        public bool IsCurrency { get; set; }
        //public Func<ISearch, ISearch> Search { get; set; }
    }
}