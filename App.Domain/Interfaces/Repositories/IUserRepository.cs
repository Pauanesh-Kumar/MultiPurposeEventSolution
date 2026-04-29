using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Domain.Entities;

namespace App.Domain.Interfaces.Repositories
{
    public interface IUserRepository : IGenericRepository<UserDomain>
    {
        Task<UserDomain> GetUserByEmailAsync(string emailAddress);

        Task<int> AddUserAsync(UserDomain user);
    }
}
