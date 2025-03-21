using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderAPI.Domain.Entities;

namespace OrderAPI.Application.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.CustomerName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.CustomerEmail)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.CustomerGSM)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.TotalAmount)
                .HasPrecision(18, 2);

            builder.HasMany(x => x.OrderDetails)
                   .WithOne(x => x.Order)
                   .HasForeignKey(x => x.OrderId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
