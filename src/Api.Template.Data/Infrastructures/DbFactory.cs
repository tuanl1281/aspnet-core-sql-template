using Microsoft.EntityFrameworkCore;

namespace Api.Template.Data.Infrastructures;

public interface IDbFactory<out TContext> where TContext : DbContext
{
    TContext Init();
}

public class DbFactory<TContext>: Disposable, IDbFactory<TContext> where TContext: DbContext, new()
{
    private TContext? _dbContext;

    public TContext Init() => _dbContext ??= new TContext();
    
    protected override void DisposeCore()
    {
        _dbContext?.Dispose();
    }
}