using AwsCognito.Test.Domain.Samples;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AwsCognito.Test.Infrastructure.Database.App.Configurations
{
    public class SampleConfiguration : IEntityTypeConfiguration<Sample>
    {
        public void Configure(EntityTypeBuilder<Sample> builder)
        {
            builder.ToTable("Samples");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .HasConversion(
                    name => name.Value,
                    value => SampleName.FromTrusted(value))
                .IsRequired()
                .HasMaxLength(SampleName.MaxLength);

            builder.HasIndex(x => x.Name)
                .IsUnique();

            builder.Property(x => x.CreatedAtUtc)
                .IsRequired();
        }
    }
}
