using Kendo.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace Core.Mvc.Helpers
{
    public class TreeViewItem : EssentialItem<TreeViewItem>
    {


        public TreeViewItem()
        {
            this.Items = new LinkedObjectCollection<TreeViewItem>(this);
        }

        public bool Checked { get; set; }

        public bool Expanded { get; set; }

        public bool HasChildren { get; set; }

        public string Id { get; set; }

        public IList<TreeViewItem> Items { get; private set; }


        //[ScriptIgnore]
        //public IDictionary<string, object> LinkHtmlAttributes { get; private set; }

        //public IDictionary<string, object> Serialize()
        //{
        //    Dictionary<string, object> jsonData = new Dictionary<string, object>();
        //    jsonData["id"] = this.Id;
        //    jsonData["text"] = base.Text;
        //    this.Serialize<bool>(jsonData, "hasChildren", this.HasChildren, false);
        //    this.Serialize<bool>(jsonData, "encoded", base.Encoded, true);
        //    this.Serialize<bool>(jsonData, "expanded", this.Expanded, false);
        //    this.Serialize<bool>(jsonData, "checked", this.Checked, false);
        //    this.Serialize<bool>(jsonData, "selected", base.Selected, false);
        //    this.Serialize<string>(jsonData, "imageUrl", base.ImageUrl, null);
        //    this.Serialize<string>(jsonData, "url", base.Url, null);
        //    this.Serialize<string>(jsonData, "spriteCssClass", base.SpriteCssClasses, null);
        //    if (this.Items.Any())
        //    {

        //        SetItemsTreeToJson(jsonData, this.Items);
        //        jsonData["items"] = this.Items;
        //    }
        //    return jsonData;
        //}

        //public static void SetItemsTreeToJson(Dictionary<string, object> jsonData, IList<TreeViewItem> items)
        //{
        //    if (items.Count < 0) jsonData["items"] = null;
        //    var itemsArr = new IDictionary<string, object>[items.Count];
        //    for (var i = 0; i < items.Count; i++)
        //    {
        //        itemsArr[i] = items[i].Serialize();
        //    }
        //    jsonData["items"] = itemsArr;
        //}

        //private void Serialize<T>(IDictionary<string, object> json, string field, T value, T defaultValue) where T : IComparable<T>
        //{
        //    if (((value == null) && (defaultValue != null)) || ((value != null) && (value.CompareTo(defaultValue) != 0)))
        //    {
        //        json[field] = value;
        //    }
        //}

    }


}
