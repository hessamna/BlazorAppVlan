using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IEntityService<T> where T : class
{
    Task<List<T>> GetAllAsync();
    Task<T?> GetByIdAsync(Guid id);
    Task AddOrEditAsync(T model);
}