using App.Domain.Entities;
using App.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.UseCases.UserToken
{
    public class GetUserTokenUseCase
    {
        private readonly IUserTokenRepository? _userTokenRepository;

        public GetUserTokenUseCase(IUserTokenRepository? userTokenRepository)
        {
            _userTokenRepository = userTokenRepository;
        }
        public async Task<UserTokenDomain> GetUserTokenAsync(int userId)
        {
            if (_userTokenRepository == null)
                throw new InvalidOperationException("UserTokenRepository is not initialized.");

            return await _userTokenRepository.GetTokenByUserIdAsync(userId);
        }
    }
}