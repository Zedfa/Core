using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Core.Mvc.Helpers
{
    public class TreeViewItemBuilder : EssentialItemBuilder<TreeViewItem, TreeViewItemBuilder>
    {
       private readonly TreeViewItem item;
        private readonly ViewContext viewContext;

        public TreeViewItemBuilder(TreeViewItem item, ViewContext viewContext)
            : base(item, viewContext)
        {
            
            this.item = item;
            this.viewContext = viewContext;
        }

       
        //public virtual TreeViewItemBuilder Items(Action<TreeViewItemFactory> addAction)
        //{

        //    TreeViewItemFactory factory = new TreeViewItemFactory(item, viewContext);

        //    addAction(factory);

        //    return this;
        //}

        
        public TreeViewItemBuilder Id(string id)
        {
            item.Id = id;

            return this;
        }

        
        public TreeViewItemBuilder Expanded(bool value)
        {
            item.Expanded = value;

            return this;
        }

        
        public TreeViewItemBuilder Checked(bool value)
        {
            item.Checked = value;

            return this;
        }

        public TreeViewItemBuilder HasChildren(bool value)
        {
            item.HasChildren = value;

            return this;
        }
    }

  
}
