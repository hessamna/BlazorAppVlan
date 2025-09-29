using BalzorAppVlan.Helper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IEntityService<T> where T : class
{
    Task<List<T>> GetAllAsync();
    Task<T?> GetByIdAsync(Guid id);
    Task<ServiceResult> AddOrEditAsync(T model);
    Task<ServiceResult> DeleteAsync(Guid id);
}