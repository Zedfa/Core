using Core.Entity;
using System.Data.Entity.ModelConfiguration;

namespace Core.Ef.Mapping
{
    public class CompanyViewElementMap : EntityTypeConfiguration<CompanyViewElement>
    {
        public CompanyViewElementMap()
        {
            this.HasRequired(t => t.ViewElement)
           .WithMany(t => t.CompanyViewElements)
           .HasForeignKey(d => d.ViewElementId)
           .WillCascadeOnDelete(true);

           // Ignore(a=>a.CurrentCompanyId);
        }
    }
}
