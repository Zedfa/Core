using Core.Mvc.Helpers.CustomWrapper.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Core.Mvc.Helpers.CustomWrapper.CoreKendoGrid.Settings.Features
{
    [Serializable()]
    public class Margin :  JsonObjectBase
    {
        public string Top
        {
            get;
            set;
        }


        public string Right
        {
            get;
            set;
        }


        public string Bottom
        {
            get;
            set;
        }

        public string Left
        {
            get;
            set;
        }

        protected override void Serialize(IDictionary<string, object> json)
        {
            json["top"] = Top;
            json["right"] = Right;
            json["bottom"] = Bottom;
            json["left"] = Left;

        }
    }
}