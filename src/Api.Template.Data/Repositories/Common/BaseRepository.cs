namespace Api.Template.Data.Repositories.Common;

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Infrastructures;

public interface IBaseRepository<T, out TContext> where T : class where TContext : DbContext, new()
{
    IDbFactory<TContext> DbFactory { get; }

    TContext DbContext { get; }
    
    #region Sync Methods
    /// <summary>
    /// Get all entities
    /// </summary>
    /// <returns></returns>
    IEnumerable<T> GetAll(bool allowTracking = true);

    /// <summary>
    /// Get entities by lambda expression
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="allowTracking"></param>
    /// <returns></returns>
    IEnumerable<T> GetMany(Expression<Func<T, bool>> predicate, bool allowTracking = true);

    /// <summary>
    /// Get entity by id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="allowTracking"></param>
    /// <returns></returns>
    T? GetById(object id, bool allowTracking = true);

    /// <summary>
    /// Get entity by lambda expression
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="allowTracking"></param>
    /// <returns></returns>
    T? Get(Expression<Func<T, bool>> predicate, bool allowTracking = true);

    /// <summary>
    /// Add new entity
    /// </summary>
    /// <param name="entity"></param>
    void Add(T entity);

    /// <summary>
    /// Update an entity
    /// </summary>
    /// <param name="entity"></param>
    void Update(T entity);

    /// <summary>
    /// Delete an entity by id
    /// </summary>
    /// <param name="id"></param>
    void Delete(object id);

    /// <summary>
    /// Delete by expression
    /// </summary>
    /// <param name="where"></param>
    void Delete(Expression<Func<T, bool>> where);

    /// <summary>
    /// Delete the entities
    /// </summary>
    /// <param name="entities"></param>
    void DeleteRange(IEnumerable<T> entities);
    #endregion

    #region Async Methods

    /// <summary>
    /// Get entity by lambda expression
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="allowTracking"></param>
    /// <returns></returns>
    Task<T?> GetAsync(Expression<Func<T, bool>> predicate, bool allowTracking = true);

    /// <summary>
    /// Get entity by id async
    /// </summary>
    /// <param name="id"></param>
    /// <param name="allowTracking"></param>
    /// <returns></returns>
    Task<T?> GetByIdAsync(object id, bool allowTracking = true);

    /// <summary>
    /// Get entities lambda expression async
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="allowTracking"></param>
    /// <returns></returns>
    Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> predicate, bool allowTracking = true);

    /// <summary>
    /// Get all entities async
    /// </summary>
    /// <returns></returns>
    Task<List<T>> GetAllAsync(bool allowTracking = true);

    /// <summary>
    /// Add new entity async
    /// </summary>
    /// <param name="entity"></param>
    void AddAsync(T entity);

    /// <summary>
    /// Update an entity async
    /// </summary>
    /// <param name="entity"></param>
    void UpdateAsync(T entity);

    /// <summary>
    /// Delete an entity by id async
    /// </summary>
    /// <param name="id"></param>
    void DeleteAsync(object id);

    /// <summary>
    /// Delete by expression async
    /// </summary>
    /// <param name="where"></param>
    void DeleteAsync(Expression<Func<T, bool>> where);

    /// <summary>
    /// Delete the entities async
    /// </summary>
    /// <param name="entities"></param>
    void DeleteRangeAsync(IEnumerable<T> entities);
    #endregion
}

public abstract class BaseRepository<T, TContext> where T : class where TContext: DbContext, new()
{
    #region Properties
    private readonly IDbFactory<TContext> _dbFactory;
    private readonly TContext _dbContext;
    private readonly DbSet<T> _dbSet;

    protected IDbFactory<TContext> DbFactory => _dbFactory;
    protected TContext DbContext => _dbContext;
    #endregion
    
    protected BaseRepository(IDbFactory<TContext> dbFactory)
    {
        _dbFactory = dbFactory;
        _dbContext ??= _dbFactory.Init();
        _dbSet = _dbContext.Set<T>();
    }

    /// <summary>
    /// Get entity by lambda expression
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="allowTracking"></param>
    /// <returns></returns>
    public virtual T? Get(Expression<Func<T, bool>> predicate, bool allowTracking = true)
    {
        return _dbSet.FirstOrDefault(predicate);
    }

    /// <summary>
    /// Get entity by id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="allowTracking"></param>
    /// <returns></returns>
    public virtual T? GetById(object id, bool allowTracking = true)
    {
        return _dbSet.Find(id);
    }

    /// <summary>
    /// Get entities by lambda expression
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="allowTracking"></param>
    /// <returns></returns>
    public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> predicate, bool allowTracking = true)
    {
        return _dbSet.Where(predicate).AsEnumerable();
    }

    /// <summary>
    /// Get list of entities
    /// </summary>
    /// <returns></returns>
    public virtual IEnumerable<T> GetAll(bool allowTracking = true)
    {
        return _dbSet.AsEnumerable();
    }

    /// <summary>
    /// Add new entity
    /// </summary>
    /// <param name="entity"></param>
    public virtual void Add(T entity)
    {
        _dbSet.Add(entity);
    }

    /// <summary>
    /// Update an entity
    /// </summary>
    /// <param name="entity"></param>
    public virtual void Update(T entity)
    {
        _dbContext.Entry(entity).State = EntityState.Detached;
        _dbSet.Update(entity);
    }

    /// <summary>
    /// Delete an entity by id
    /// </summary>
    /// <param name="id"></param>
    public virtual void Delete(object id)
    {
        T? existing = _dbSet.Find(id);
        if (existing != null)
            _dbSet.Remove(existing);
    }

    /// <summary>
    /// Delete by expression
    /// </summary>
    /// <param name="where"></param>
    public virtual void Delete(Expression<Func<T, bool>> where)
    {
        IEnumerable<T> entities = _dbSet.Where(where).AsEnumerable();
        foreach (T entity in entities)
            _dbSet.Remove(entity);
    }

    /// <summary>
    /// Delete entities
    /// </summary>
    /// <param name="entities"></param>
    public virtual void DeleteRange(IEnumerable<T> entities)
    {
        _dbSet.RemoveRange(entities);
    }

    /// <summary>
    /// Get entities by lambda expression
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="allowTracking"></param>
    /// <returns></returns>
    public virtual async Task<T?> GetAsync(Expression<Func<T, bool>> predicate, bool allowTracking = true)
    {
        if (allowTracking)
            return await _dbSet.FirstOrDefaultAsync(predicate);
        
        return await _dbSet.AsNoTracking().FirstOrDefaultAsync(predicate);
    }

    /// <summary>
    /// Get entity by id async
    /// </summary>
    /// <param name="id"></param>
    /// <param name="allowTracking"></param>
    /// <returns></returns>
    public virtual async Task<T?> GetByIdAsync(object id, bool allowTracking = true)
    {
        return await _dbSet.FindAsync(id);
    }

    /// <summary>
    /// Get entities by lambda expression
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="allowTracking"></param>
    /// <returns></returns>
    public virtual async Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> predicate, bool allowTracking = true)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    /// <summary>
    /// Get all entities async
    /// </summary>
    /// <returns></returns>
    public virtual async Task<List<T>> GetAllAsync(bool allowTracking = true)
    {
        return await _dbSet.ToListAsync();
    }

    /// <summary>
    /// Add new entity async
    /// </summary>
    /// <param name="entity"></param>
    public virtual void AddAsync(T entity)
    {
        _dbSet.AddAsync(entity);
    }

    /// <summary>
    /// Update an entity async
    /// </summary>
    /// <param name="entity"></param>
    public virtual void UpdateAsync(T entity)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
        _dbSet.Attach(entity);
    }

    /// <summary>
    /// Delete an entity
    /// </summary>
    /// <param name="id"></param>
    public virtual void DeleteAsync(object id)
    {
        T? existing = _dbSet.Find(id);
        if (existing != null)
            _dbSet.Remove(existing);
    }

    /// <summary>
    /// Delete by expression
    /// </summary>
    /// <param name="predicate"></param>
    public virtual void DeleteAsync(Expression<Func<T, bool>> predicate)
    {
        IEnumerable<T> entities = _dbSet.Where(predicate).AsEnumerable();
        foreach (T entity in entities)
            _dbSet.Remove(entity);
    }

    /// <summary>
    /// Delete entities
    /// </summary>
    /// <param name="entities"></param>
    public virtual void DeleteRangeAsync(IEnumerable<T> entities)
    {
        _dbSet.RemoveRange(entities);
    }
}