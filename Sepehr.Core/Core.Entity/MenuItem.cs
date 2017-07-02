using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity
{
   
    public class MenuItem
    {
        public MenuItem()
        {
            Childeren = new List<MenuItem>();
            ViewElement=new ViewElement();
        }

        public int ViewElementId { get; set; }
        public int SortOrder { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public bool IsMenu { get; set; }
        public MenuItem Parent { get; set; }
        public List<MenuItem> Childeren { get; set; }
        public ViewElement ViewElement { get; set; }

    }
}
