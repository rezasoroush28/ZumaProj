using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Zuma.Domain.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity> CreateAsync(TEntity entity); 
        Task<TEntity> GetByIdAsync(Guid id);       
        Task UpdateAsync(TEntity entity);          
        Task DeleteAsync(int id);                 
    }
}