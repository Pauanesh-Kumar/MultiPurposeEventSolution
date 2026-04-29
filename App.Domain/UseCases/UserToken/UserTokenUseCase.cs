using App.Domain.Interfaces.Repositories;
using App.Domain.UseCases.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace App.Domain.UseCases.UserToken
{
    public class UserTokenUseCase
    {
        private readonly IUserTokenRepository _userTokenRepository;

        public UserTokenUseCase(
            IUserTokenRepository userTokenRepository)
        {
            _userTokenRepository = userTokenRepository;
        }
        public async Task<int> PersistToken(Entities.UserTokenDomain userTokenDomain)
        {
           return await _userTokenRepository.AddUserTokenAsync(userTokenDomain);
        }
    }
}
