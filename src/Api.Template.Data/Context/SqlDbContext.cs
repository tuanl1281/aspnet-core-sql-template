using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Api.Template.Data.Entities.Common;
using Api.Template.Data.Constants.Common;

namespace Api.Template.Data.Context;

public class SqlDbContext: DbContext
{
    private Guid TenantId
    {
        get
        {
            try
            {
                var value = new HttpContextAccessor().HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimConstants.TenantId))?.Value;
                return string.IsNullOrEmpty(value) ? Guid.Empty : Guid.Parse(value);
            }
            catch (Exception exception)
            {
                return Guid.Empty;
            }
        }
    }
    
    public SqlDbContext()
    {
    }

    public SqlDbContext(DbContextOptions<SqlDbContext> options): base(options)
    {
    } 
    
    #region --- Filter ---
    public void TenantFilter<T>(ModelBuilder modelBuilder) where T : class
        =>  modelBuilder.Entity<T>().HasQueryFilter(_ => EF.Property<Guid>(_, "TenantId") == TenantId);
    #endregion
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Apply global filter for entity which has tenantId
        if (TenantId != Guid.Empty)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var prop = entityType.FindProperty("TenantId");
                if (prop != null && prop.ClrType == typeof(int))
                {
                    GetType()
                        .GetMethod(nameof(TenantFilter))
                        .MakeGenericMethod(entityType.ClrType)
                        .Invoke(this, new object[] { modelBuilder });
                }
            }
        }
    }
    
    public override int SaveChanges()
    {
        /* Insert */
        var insertedEntries = ChangeTracker.Entries()
            .Where(x => x.State == EntityState.Added)
            .Select(x => x.Entity);

        foreach (var insertedEntry in insertedEntries)
        {
            /* Date Created */
            if (insertedEntry is BaseEntity)
                ((BaseEntity) insertedEntry).DateCreated = DateTime.Now;
            
            if (insertedEntry is TenantBaseEntity)
                ((TenantBaseEntity) insertedEntry).DateCreated = DateTime.Now;
            
            if (insertedEntry is SoftDeletedBaseEntity)
                ((SoftDeletedBaseEntity) insertedEntry).DateCreated = DateTime.Now;
            /* Tenant */
            if (TenantId != Guid.Empty && insertedEntry is TenantBaseEntity)
                ((TenantBaseEntity) insertedEntry).TenantId = TenantId;
        }
        
        /* Update */
        var modifiedEntries = ChangeTracker.Entries()
            .Where(x => x.State == EntityState.Modified)
            .Select(x => x.Entity);
        
        foreach (var modifiedEntry in modifiedEntries)
        {
            /* Date Created */
            if (modifiedEntry is BaseEntity)
                ((BaseEntity) modifiedEntry).DateCreated = DateTime.Now;
            
            if (modifiedEntry is TenantBaseEntity)
                ((TenantBaseEntity) modifiedEntry).DateCreated = DateTime.Now;
            
            if (modifiedEntry is SoftDeletedBaseEntity)
                ((SoftDeletedBaseEntity) modifiedEntry).DateCreated = DateTime.Now;
            /* Tenant */
            if (TenantId != Guid.Empty && modifiedEntry is TenantBaseEntity)
                ((TenantBaseEntity) modifiedEntry).TenantId = TenantId;
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
            /* Date Created */
            if (insertedEntry is BaseEntity)
                ((BaseEntity) insertedEntry).DateCreated = DateTime.Now;
            
            if (insertedEntry is TenantBaseEntity)
                ((TenantBaseEntity) insertedEntry).DateCreated = DateTime.Now;
            
            if (insertedEntry is SoftDeletedBaseEntity)
                ((SoftDeletedBaseEntity) insertedEntry).DateCreated = DateTime.Now;
            /* Tenant */
            if (TenantId != Guid.Empty && insertedEntry is TenantBaseEntity)
                ((TenantBaseEntity) insertedEntry).TenantId = TenantId;
        }
        
        /* Update */
        var modifiedEntries = ChangeTracker.Entries()
            .Where(x => x.State == EntityState.Modified)
            .Select(x => x.Entity);
        
        foreach (var modifiedEntry in modifiedEntries)
        {
            /* Date Created */
            if (modifiedEntry is BaseEntity)
                ((BaseEntity) modifiedEntry).DateCreated = DateTime.Now;
            
            if (modifiedEntry is TenantBaseEntity)
                ((TenantBaseEntity) modifiedEntry).DateCreated = DateTime.Now;
            
            if (modifiedEntry is SoftDeletedBaseEntity)
                ((SoftDeletedBaseEntity) modifiedEntry).DateCreated = DateTime.Now;
            /* Tenant */
            if (TenantId != Guid.Empty && modifiedEntry is TenantBaseEntity)
                ((TenantBaseEntity) modifiedEntry).TenantId = TenantId;
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}