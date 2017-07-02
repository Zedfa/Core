namespace Kendo.Mvc.UI
{
    using System.Collections.Generic;
    using System.Linq;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI.Html;

    public class GridCustomActionCommand<T> : GridCustomCommandBase, INavigatable
        where T: class
    {
        public GridCustomActionCommand()
        {
            DataRouteValues = new List<IGridDataKey<T>>();
            SendDataKeys = true;
            SendState = true;
            Click = new ClientHandlerDescriptor();
        }        
        
        public IList<IGridDataKey<T>> DataRouteValues 
        { 
            get; 
            private set; 
        }

        public bool SendDataKeys
        {
            get;
            set;
        }

        public bool SendState
        {
            get;
            set;
        }

        public ClientHandlerDescriptor Click
        {
            get;
            set;
        }

        public override IDictionary<string, object> Serialize(IGridUrlBuilder urlBuilder)
        {
            var state = base.Serialize(urlBuilder);

            if (Text.HasValue())
            {
                state["text"] = Text;
            }

            if (Click.HasValue())
            {
                state["click"] = Click;
            }
            
            //if (SendState /*&& !Ajax*/)
            //{
            //    RouteValues.AddRange(urlBuilder.GetState());
            //}

            //var dataRouteValues = DataRouteValues.Cast<IDataKey>();

            ////if (Ajax)
            ////{
            ////    state["ajax"] = true;
            ////}

            //if (SendDataKeys)
            //{
            //    dataRouteValues = dataRouteValues.Concat(urlBuilder.GetDataKeys());
            //}

            //foreach (var route in dataRouteValues)
            //{
            //    RouteValues[route.RouteKey] = "<#=" + route.Name + "#>";
            //}

            //state["url"] = urlBuilder.Url(this);

            return state;
        }

        public override IEnumerable<IGridButtonBuilder> CreateDisplayButtons(IGridUrlBuilder urlBuilder, IGridHtmlHelper htmlHelper)
        {
            var button = CreateButton<GridLinkButtonBuilder>(Text, CssClass());

            button.Url = (dataItem) =>
                {
                    var navigatable = new RequestSettings
                    {
                        ActionName = ActionName,
                        ControllerName = ControllerName,
                    };

                    navigatable.RouteValues.AddRange(RouteValues);

                    var dataRouteValues = DataRouteValues.Cast<IDataKey>();

                    if (SendDataKeys)
                    {
                        dataRouteValues = dataRouteValues.Concat(urlBuilder.GetDataKeys());
                    }

                    foreach (var key in dataRouteValues)
                    {
                        navigatable.RouteValues.Add(key.RouteKey, key.GetValue(dataItem));
                    }

                    return urlBuilder.Url(navigatable, SendState);
                };

            return new [] { button };
        }
    }
}
