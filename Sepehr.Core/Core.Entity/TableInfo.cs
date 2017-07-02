using Core.Cmn;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity
{

    [Table("TableNames", Schema = "core")] 
    [DataContract]
    public class TableInfo : EntityBase<TableInfo>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Caption { get; set; }

    }
}
