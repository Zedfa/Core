using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kendo.Mvc;

namespace Core.Mvc.Attribute.Filter
{
    public class ModelFieldTypeInfo : JsonObject
    {
        public ModelFieldTypeInfo()
        {
        }
        //public string viewInfoName { get; set; }
        public string ModelPropName { get; set; }
        public string CustomType { get; set; }
        public string FalseEquivalent { get; set; }
        public string TrueEquivalent { get; set; }
        public bool IdReplacement { get; set; }
        public string NavigationProperty { get; set; }
        public Dictionary<string, string> EnumKeyValue { get; set; }

        public Dictionary<string, string> LookupKeyValue { get; set; }

        public Dictionary<string, string> DropDownKeyValue { get; set; }

        public Dictionary<string, string> AutoCompleteKeyValue { get; set; }

        protected override void Serialize(IDictionary<string, object> json)
        {
            //json["vmPropName"] = viewInfoName;
            if (!string.IsNullOrEmpty(ModelPropName))
            {
                json["mdlPropName"] = ModelPropName;
            }

            if (!string.IsNullOrEmpty(CustomType))
            {
                json["custType"] = CustomType;
            }

            if (!string.IsNullOrEmpty(FalseEquivalent))
            {
                json["falseEqui"] = FalseEquivalent;
            }

            if (!string.IsNullOrEmpty(TrueEquivalent))
            {
                json["trueEqui"] = TrueEquivalent;
            }

            if (EnumKeyValue != null)
            {
                if (EnumKeyValue.Count > 0)
                {
                    json["enumDic"] = EnumKeyValue;
                }
            }

            if (IdReplacement)
            {
                json["IdReplacement"] = IdReplacement;
            }

            if (!string.IsNullOrEmpty(NavigationProperty))
            {
                json["navProp"] = NavigationProperty;
            }

            //LookupKeyValue

            if (LookupKeyValue != null)
            {
                if (LookupKeyValue.Count > 0)
                {
                    json["lookupInfo"] = LookupKeyValue;
                }

            }
            if (DropDownKeyValue != null)
            {
                if (DropDownKeyValue.Count > 0)
                {
                    json["dropdownInfo"] = DropDownKeyValue;
                }

            }
            if (AutoCompleteKeyValue != null)
            {
                if (AutoCompleteKeyValue.Count > 0)
                {
                    json["autoCompleteInfo"] = AutoCompleteKeyValue;
                }

            }

        }
    }
}
