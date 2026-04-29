using App.Domain.Entities;
using App.Domain.Interfaces.Repositories;
using App.Infrastructure.Data.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Infrastructure.Repositories
{
    public class UserTokenRepository : GenericRepository<UserTokenDomain, UserToken>, IUserTokenRepository
    {
        public UserTokenRepository(ApplicationDBContext dBContext, IMapper mapper) : base(dBContext, mapper)
        {
        }
        public async Task<UserTokenDomain> GetTokenByUserIdAsync(int userId)
        {
            var userToken = await _dbContext.UserTokens.FirstOrDefaultAsync(ut => ut.UserId == userId);
            return _mapper.Map<UserTokenDomain>(userToken);
        }
        public async Task<int> AddUserTokenAsync(UserTokenDomain userTokenDomain)
        {
            var existingTokenDetails = await _dbContext.UserTokens.Where(x => x.UserId == userTokenDomain.UserId).ToListAsync();
            if (existingTokenDetails.Any())
            {
                _dbContext.UserTokens.RemoveRange(existingTokenDetails);
            }
            var newUserToken = _mapper.Map<UserToken>(userTokenDomain);
            await _dbContext.UserTokens.AddAsync(newUserToken);
            return await _dbContext.SaveChangesAsync();
        }
    }
}
