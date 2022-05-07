namespace Api.Template.Data.Infrastructures;

using Microsoft.EntityFrameworkCore;

public interface IDbFactory<out TContext> : IDisposable where TContext: DbContext, new()
{
    TContext Init();
}

public class DbFactory<TContext> : Disposable, IDbFactory<TContext> where TContext: DbContext, new()
{
    private TContext? _dbContext;

    public TContext Init() => _dbContext ??= new TContext();
    
    protected override void DisposeCore()
    {
        _dbContext?.Dispose();
    }
}