using Core.Entity;
using System.Data.Entity.ModelConfiguration;

namespace Core.Ef.Mapping
{

    public class CompanyChartMap : EntityTypeConfiguration<CompanyChart>
    {
      
        public CompanyChartMap()
        {
            this.HasMany(x => x.ChildCompanyChart)
            .WithOptional()
            .HasForeignKey(s => s.ParentId);
        }
                
    }
}