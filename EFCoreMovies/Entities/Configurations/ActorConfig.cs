using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace EFCoreMovies.Entities.Configurations
{
    public class ActorConfig : IEntityTypeConfiguration<Actor>
    {
        public void Configure(EntityTypeBuilder<Actor> builder)
        {
            builder.Property(p => p.Name).IsRequired(true);
            //builder.Property(p => p.DateOfBirth).HasColumnType("date");
            builder.Property(p => p.Biography).HasColumnType("nvarchar(max)");
        }
    }
}
