using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Api.Template.Data.Infrastructures;
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

public interface IBaseService<TContext, TE, TF, TV, TA, TU> where TContext : DbContext where TE : class where TF : class where TV : class where TA : class where TU : class
{
    Task<object> Add(TA model);

    Task<object> Update(TU model, Guid id);

    Task<object> Delete(Guid id);

    Task<TV> Get(Guid id);

    Task<PagingResponseModel<TV>> GetPagedResult(TF filter, Guid userId);
}

public class BaseService<TContext, TE, TF, TV, TA, TU>: IBaseService<TContext, TE, TF, TV, TA, TU> where TContext : DbContext, new() where TE : class where TF : class where TV : class where TA : class where TU : class
{
    protected readonly IMapper _mapper;
    protected readonly IUnitOfWork<TContext> _unitOfWork;

    protected BaseService(IMapper mapper, IUnitOfWork<TContext> unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public virtual async Task<object> Add(TA model)
    {
        /* Builder */
        var entity = _mapper.Map<TA, TE>(model);
        /* Save */
        _unitOfWork.Repository<TE>().Add(entity);
        await _unitOfWork.SaveChangesAsync();
        
        return null;
    }

    public virtual async Task<object> Update(TU model, Guid id)
    {
        /* Validate */
        var entity = await _unitOfWork.Repository<TE>().GetByIdAsync(id);
        if (entity == null)
            throw new NotFoundException();
        /* Builder */
        entity = _mapper.Map<TU, TE>(model, entity);
        /* Save */
        _unitOfWork.Repository<TE>().Update(entity);
        await _unitOfWork.SaveChangesAsync();

        return id;
    }

    public virtual async Task<object> Delete(Guid id)
    {
        /* Validate */
        var entity = await _unitOfWork.Repository<TE>().GetByIdAsync(id);
        if (entity == null)
            throw new NotFoundException();
        /* Save */
        _unitOfWork.Repository<TE>().Delete(id);
        await _unitOfWork.SaveChangesAsync();

        return id;
    }

    public virtual async Task<TV> Get(Guid id)
    {
        /* Validate */
        var entity = await _unitOfWork.Repository<TE>().GetByIdAsync(id);
        if (entity == null)
            throw new NotFoundException();
        
        return _mapper.Map<TE, TV>(entity);
    }

    public virtual async Task<PagingResponseModel<TV>> GetPagedResult(TF filter, Guid userId)
    {
        var result = new PagingResponseModel<TV>();
        /* Query */
        var entities = await _unitOfWork.Repository<TE>().GetAllAsync();
        /* Builder */
        var entityModels = _mapper.Map<List<TE>, List<TV>>(entities);

        result.TotalCounts = entities.Count;
        result.Data = entityModels;
        return result;
    }
}