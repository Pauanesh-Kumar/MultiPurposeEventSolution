using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Interfaces.Repositories
{
    public interface IGenericRepository<TDomain> where TDomain : class
    {
        Task<IEnumerable<TDomain>> GetAllAsync();
        Task AddAsync(TDomain domainEntity);
        //Task UpdateAsync(T domainEntity);
        Task<TDomain?> GetByIdAsync(object id);
        Task UpdateAsync(TDomain entity, object id);
        Task DeleteAsync(object id);
        Task<bool> ExistsAsync(object id);
        Task<int> CommitAsync();
        IQueryable<TDomain> AsQueryable();
    }
}
