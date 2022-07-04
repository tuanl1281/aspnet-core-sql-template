using AutoMapper;
using Api.Template.Data.Context;
using Api.Template.Data.Infrastructures;
using Api.Template.Data.Repositories.Common;
using Api.Template.ViewModel.Common.Exception;
using Api.Template.ViewModel.Common.Response;

namespace Api.Template.Service.Core.Common;

/*
    Generic
    - TE: Entity
    - TF: Filter model
    - TV: View model
    - TA: Add model
    - TU: Update model
*/

public interface IBaseService<TE, TF, TV, TA, TU> where TE : class where TF : class where TV : class where TA : class where TU : class
{
    Task<object> Add(TA model);

    Task<object> Update(TU model, Guid id);

    Task<object> Delete(Guid id);

    Task<TV> Get(Guid id);

    Task<PagingResponseModel<TV>> GetPagedResult(TF filter, Guid userId);
}

public class BaseService<TE, TF, TV, TA, TU>: IBaseService<TE, TF, TV, TA, TU> where TE : class where TF : class where TV : class where TA : class where TU : class
{
    protected readonly IMapper _mapper;
    protected readonly IUnitOfWork<SqlDbContext> _unitOfWork;
    protected readonly IBaseRepository<TE, SqlDbContext> _repository;

    protected BaseService(IMapper mapper, IUnitOfWork<SqlDbContext> unitOfWork, IBaseRepository<TE, SqlDbContext> repository)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _repository = repository;
    }

    public virtual async Task<object> Add(TA model)
    {
        /* Builder */
        var entity = _mapper.Map<TA, TE>(model);
        /* Save */
        _repository.Add(entity);
        await _repository.DbContext.SaveChangesAsync();
        
        return null;
    }

    public virtual async Task<object> Update(TU model, Guid id)
    {
        /* Validate */
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null)
            throw new NotFoundException();
        /* Builder */
        entity = _mapper.Map<TU, TE>(model, entity);
        /* Save */
        _repository.Update(entity);
        await _repository.DbContext.SaveChangesAsync();

        return id;
    }

    public virtual async Task<object> Delete(Guid id)
    {
        /* Validate */
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null)
            throw new NotFoundException();
        /* Save */
        _repository.Delete(id);
        await _repository.DbContext.SaveChangesAsync();

        return id;
    }

    public virtual async Task<TV> Get(Guid id)
    {
        /* Validate */
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null)
            throw new NotFoundException();
        
        return _mapper.Map<TE, TV>(entity);
    }

    public virtual async Task<PagingResponseModel<TV>> GetPagedResult(TF filter, Guid userId)
    {
        var result = new PagingResponseModel<TV>();
        /* Query */
        var entities = await _repository.GetAllAsync();
        /* Builder */
        var entityModels = _mapper.Map<List<TE>, List<TV>>(entities);

        result.TotalCounts = entities.Count;
        result.Data = entityModels;
        return result;
    }
}