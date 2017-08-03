using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Cmn;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Core.Entity
{
    [Table("Constants", Schema = "core")] 

    public class Constant : EntityBase<Constant>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required]
        [MaxLength(200)]

        public string Key { get; set; }
        [Required]
        public string Value { get; set; }
        public int ConstantCategoryID { get; set; }
        [Required]
        [MaxLength(10)]
        public string Culture { get; set; }
        [ForeignKey("ConstantCategoryID")]
        public virtual ConstantCategory ConstantCategory { get; set; }
    }
}
