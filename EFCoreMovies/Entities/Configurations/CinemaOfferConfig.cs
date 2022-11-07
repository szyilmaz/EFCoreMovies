using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace EFCoreMovies.Entities.Configurations
{
    public class CinemaOfferConfig : IEntityTypeConfiguration<CinemaOffer>
    {
        public void Configure(EntityTypeBuilder<CinemaOffer> builder)
        {
            builder.Property(p => p.DiscountPercentage).HasPrecision(5, 2);
            //modelBuilder.Entity<CinemaOffer>().Property(p => p.Begin).HasColumnType("date");
            //modelBuilder.Entity<CinemaOffer>().Property(p => p.End).HasColumnType("date");
        }
    }
}
