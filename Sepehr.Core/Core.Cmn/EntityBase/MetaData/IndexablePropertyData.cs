using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Cmn.DataSource
{
    public class IndexablePropertyData
    {
        public IndexablePropertyData(string groupName, string navigationPath, int indexOrder)
        {
            GroupName = groupName;
            IndexOrder = indexOrder;
            NavigationPath = navigationPath;
        }
        public string GroupName { get; set; }
        public int IndexOrder { get; set; }
        public string NavigationPath { get; set; }
    }
    public class GroupedIndexablePropertyData
    {
        public GroupedIndexablePropertyData(string groupName, string firstPropertyPath, int indexOrder, List<IndexablePropertyData> indexes)
        {
            GroupName = groupName;
            IndexOrder = indexOrder;
            Indexes = indexes;
            FirstPropertyPath = firstPropertyPath;
        }
        public string GroupName { get; set; }
        public int IndexOrder { get; set; }
        public string FirstPropertyPath { get; set; }
        public List<IndexablePropertyData> Indexes { get; set; }
    }
}
