﻿using System;
using System.Collections.Generic;
using System.Linq;
using Kendo.Mvc.Infrastructure;

namespace Kendo.Mvc.UI
{
    public class DataSourceSchema : JsonObject
    {
        public string Data { get; set; }

        public string Total { get; set; }

        public string Errors { get; set; }

        public ModelDescriptor Model
        {
            get;
            set;
        }

        public DataSourceSchema()
        {
            Data = "Data";
            Total = "Total"; 
            Errors = "Errors";
        }

        protected override void Serialize(IDictionary<string, object> json)
        {
            FluentDictionary.For(json)
                .Add("data", Data, string.Empty)
                .Add("total", Total, string.Empty)
                .Add("errors", Errors, string.Empty);

            if (Model != null)
            {
                json.Add("model", Model.ToJson());
            }
        }
    }
}
