using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Cmn;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity
{
    [Table("ConstantCategories", Schema = "core")] 

    public class ConstantCategory : EntityBase<ConstantCategory>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Constant> Constants { get; set; }
    }
}
