using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Domain.Entities;

namespace App.Domain.Interfaces.Repositories
{
    public interface IUserTokenRepository : IGenericRepository<UserTokenDomain>
    {
        Task<UserTokenDomain> GetTokenByUserIdAsync(int userId);

        Task<int> AddUserTokenAsync(UserTokenDomain userTokenDomain);

    }
}
