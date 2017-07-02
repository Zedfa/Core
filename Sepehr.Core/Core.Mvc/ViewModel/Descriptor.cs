using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Cmn.Extensions;
using Core.Mvc.Extensions;

namespace Core.Mvc.ViewModel
{
    internal class Descriptor : Kendo.Mvc.UI.ModelDescriptor
    {
        private IDictionary<string, object> _model;

        private Helpers.TreeViewModelBase _modelbase;

        public Descriptor(Type type, object model)
            : base(type)
        {
            if (model is Helpers.TreeViewModelBase)
            {
                _modelbase = (Helpers.TreeViewModelBase) model;
            }
            else

                this._model = model.ToDictionary();
            // this._model = model;
        }

        protected override void Serialize(IDictionary<string, object> json)
        {

            if (_modelbase != null)
            {
                base.Serialize(json);

                // base.Serialize(_modelbase.ToJson());
            }

            else
            {

                if (Id != null)
                {
                    json["id"] = Id.Name;
                }

                var fields = new Dictionary<string, object>();

                json["fields"] = fields;

                Fields.ToList().ForEach(prop =>
                {
                    var field = new Dictionary<string, object>();
                    fields[prop.Member] = field;

                    if (!prop.IsEditable)
                    {
                        field["editable"] = false;
                    }

                    field["type"] = prop.MemberType.ToJavaScriptType().ToLowerInvariant();

                    if (_model.Count > 0)
                    {

                        prop.DefaultValue = _model[prop.Member];
                    }

                    if (prop.MemberType.IsNullableType() || prop.DefaultValue != null)
                    {
                        field["defaultValue"] = prop.DefaultValue;
                    }
                });

            }
        }
    }
}
