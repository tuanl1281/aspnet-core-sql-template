using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Api.Template.Data.Entities.Common;

namespace Api.Template.Data.Configurations.Common;

public abstract class TenantBaseConfiguration<T> : IEntityTypeConfiguration<T> where T : TenantBaseEntity
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasDefaultValueSql("NEWID()");

        ConfigureMoreProperties(builder);
    }

    protected abstract void ConfigureMoreProperties(EntityTypeBuilder<T> builder);
}