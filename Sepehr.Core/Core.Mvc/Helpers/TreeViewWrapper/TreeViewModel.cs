﻿using System;
using System.Collections.Generic;


namespace Core.Mvc.Helpers
{
    public class TreeViewItemModel 
    {
        public TreeViewItemModel()
        {
            this.Enabled = true;
            this.Encoded = true;
            this.Items = new List<TreeViewItemModel>();
            this.HtmlAttributes = new Dictionary<string, string>();
            this.ImageHtmlAttributes = new Dictionary<string, string>();
        }

        public bool Checked { get; set; }

        public bool Enabled { get; set; }

        public bool Encoded { get; set; }

        public bool Expanded { get; set; }

        public bool HasChildren { get; set; }

        public IDictionary<string, string> HtmlAttributes { get; set; }

        public string Id { get; set; }

        public IDictionary<string, string> ImageHtmlAttributes { get; set; }

        public string ImageUrl { get; set; }

        public List<TreeViewItemModel> Items { get; set; }

        public string Text { get; set; }

        public string Url { get; set; }
    }
}
