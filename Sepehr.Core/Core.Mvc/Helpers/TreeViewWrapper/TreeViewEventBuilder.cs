using Core.Mvc.Helpers.CustomWrapper.DataSource;
using Kendo.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mvc.Helpers
{


    public class TreeViewEventBuilder
    {

        internal IDictionary<string, object> handler = new Dictionary<string, object>();
        

        public TreeViewEventBuilder()
        {

        }


        public string Collapse
        {
            get
            {
                return handler["collapse"].ToString();
            }


            set
            {
                SetEventHandler("collapse", value);
            }
        }

        public string DataBound
        {
            get
            {
                return handler["dataBound"].ToString();
            }

            set
            {

                // string initDataBoundScript = string.Format("if (args.node != undefined){{ $('#{0}').prev('.k-grouping-header').hide();}}" +
                //   "else {{$('#{0}').prev('.k-grouping-header').show();}}"  , this._treeViewName);
                //SetEventHandler("dataBound", initDataBoundScript, value);
                SetEventHandler("dataBound", value);

            }
        }

        public string Drag
        {
            get
            {
                return handler["drag"].ToString();
            }


            set
            {
                SetEventHandler("drag", value);

            }
        }

        public string DragEnd
        {
            get
            {
                return handler["dragend"].ToString();
            }

            set
            {
                SetEventHandler("dragend", value);

            }
        }

        public string DragStart
        {
            get
            {
                return handler["dragstart"].ToString();
            }

            set
            {
                SetEventHandler("dragstart", value);
            }
        }

        public string Drop
        {
            get
            {
                return handler["drop"].ToString();
            }

            set
            {
                SetEventHandler("drop", value);
            }
        }

        public string Expand
        {
            get
            {
                return handler["Expand"].ToString();
            }

            set
            {
                SetEventHandler("Expand", value);

            }
        }

        public string Select
        {
            get
            {
                return handler["select"].ToString();
            }

            set
            {
                SetEventHandler("select", value);
            }
        }

        public string Change
        {
            get
            {
                return handler["change"].ToString();
            }

            set
            {
                SetEventHandler("change", value);
            }
        }

        

        private void SetEventHandler(string eventName, string customeEventName)
        {
            handler.Add(eventName, new ClientHandlerDescriptor() { TemplateDelegate = obj => customeEventName });
        }

        private void SetEventHandler(string eventName, string treeEventBody, string customeEventName)
        {
            handler.Add(eventName, new ClientHandlerDescriptor()
            {
                TemplateDelegate = obj =>
                    string.Format("function(args){{ {0} {1}(args); }}"
                    , treeEventBody
                    , customeEventName)
            });
        }

    }
}

