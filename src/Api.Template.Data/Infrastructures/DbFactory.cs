using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Api.Template.Data.Infrastructures;

public interface IDbFactory<out TContext> where TContext : DbContext
{
    TContext Init();
}

public class DbFactory<TContext>: Disposable, IDbFactory<TContext> where TContext: DbContext, new()
{
    private TContext? _dbContext;
    private readonly IConfiguration _configuration;
    
    public DbFactory(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public TContext Init() => _dbContext ??= (TContext) Activator.CreateInstance(typeof(TContext), _configuration);
    
    protected override void DisposeCore()
    {
        _dbContext?.Dispose();
    }
}