using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace EFCoreMovies.Entities.Configurations
{
    public class MovieConfig : IEntityTypeConfiguration<Movie>
    {
        public void Configure(EntityTypeBuilder<Movie> builder)
        {
            builder.Property(p => p.Title).HasMaxLength(250).IsRequired(true);
            //builder.Property(p => p.ReleaseDate).HasColumnType("date");
            builder.Property(p => p.PosterURL).HasMaxLength(500).IsUnicode(false);
        }
    }
}
