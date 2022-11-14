using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace EFCoreMovies.Entities.Configurations
{
    public class GenreConfig : IEntityTypeConfiguration<Genre>
    {
        public void Configure(EntityTypeBuilder<Genre> builder)
        {
            builder.Property(p => p.Name).IsRequired(true);
            builder.HasQueryFilter(g => !g.IsDeleted);
            builder.HasIndex(p => p.Name).IsUnique().HasFilter("IsDeleted = 'false'");
        }
    }
}
