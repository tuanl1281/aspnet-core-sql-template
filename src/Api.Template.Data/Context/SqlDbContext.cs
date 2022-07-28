using Api.Template.Data.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace Api.Template.Data.Context;

public class SqlDbContext: DbContext
{
    public SqlDbContext()
    {
    }

    public SqlDbContext(DbContextOptions<SqlDbContext> options): base(options)
    {
    } 
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
    
    public override int SaveChanges()
    {
        /* Insert */
        var insertedEntries = ChangeTracker.Entries()
            .Where(x => x.State == EntityState.Added)
            .Select(x => x.Entity);

        foreach (var insertedEntry in insertedEntries)
        {
            if (insertedEntry is BaseEntity auditableEntity)
                auditableEntity.DateCreated = DateTime.Now;
        }

        var modifiedEntries = ChangeTracker.Entries()
            .Where(x => x.State == EntityState.Modified)
            .Select(x => x.Entity);
        /* Update */
        foreach (var modifiedEntry in modifiedEntries)
        {
            if (modifiedEntry is BaseEntity auditableEntity)
                auditableEntity.DateUpdated = DateTime.Now;
        }

        return base.SaveChanges();
    }
    
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        /* Insert */
        var insertedEntries = ChangeTracker.Entries()
            .Where(x => x.State == EntityState.Added)
            .Select(x => x.Entity);

        foreach (var insertedEntry in insertedEntries)
        {
            if (insertedEntry is BaseEntity auditableEntity)
                auditableEntity.DateCreated = DateTime.Now;
        }

        var modifiedEntries = ChangeTracker.Entries()
            .Where(x => x.State == EntityState.Modified)
            .Select(x => x.Entity);
        /* Update */
        foreach (var modifiedEntry in modifiedEntries)
        {
            if (modifiedEntry is BaseEntity auditableEntity)
                auditableEntity.DateUpdated = DateTime.Now;
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}