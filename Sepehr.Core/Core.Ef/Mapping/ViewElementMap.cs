using Core.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Ef.Mapping
{

    public class ViewElementMap : EntityTypeConfiguration<ViewElement>
    {

        public ViewElementMap()
        {
            this.HasMany(x => x.ChildrenViewElement)
            .WithOptional()
            .HasForeignKey(s => s.ParentId);
        }
    }
}
