using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Api.Template.Data.Entities.Common;

namespace Api.Template.Data.Configurations.Common;

public abstract class SoftDeletedConfiguration<T> : IEntityTypeConfiguration<T> where T : SoftDeletedBaseEntity
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasDefaultValueSql("NEWID()");

        builder.HasIndex(_ => _.Id)
            .HasFilter("IsDeleted = 0")
            .IsUnique();

        ConfigureMoreProperties(builder);
    }

    protected abstract void ConfigureMoreProperties(EntityTypeBuilder<T> builder);
}

