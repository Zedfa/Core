using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Cmn;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity
{


    public class ConstantCategory : EntityBase<ConstantCategory>
    {

        public int ID { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Constant> Constants { get; set; }
    }
}
