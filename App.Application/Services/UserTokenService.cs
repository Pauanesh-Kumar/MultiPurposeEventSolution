using App.Application.DTOs.Request;
using App.Application.Interfaces;
using App.Domain.Entities;
using App.Domain.UseCases.UserToken;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Services
{
    public class UserTokenService : IUserTokenService
    {
        private readonly IMapper _mapper;
        private readonly UserTokenUseCase _userTokenUseCase;
        private readonly GetUserTokenUseCase _getUserTokenUseCase;

        public UserTokenService(
            IMapper mapper,
            UserTokenUseCase userTokenUseCase,
            GetUserTokenUseCase getUserTokenUseCase

            )
        {
            this._mapper = mapper;
            this._userTokenUseCase = userTokenUseCase;
            this._getUserTokenUseCase = getUserTokenUseCase;
        }
        public async Task<int> PersistToken(UserTokenRequestDto userTokenRequestDto)
        {
            var userTokenDomain = _mapper.Map<UserTokenDomain>(userTokenRequestDto);
            return await _userTokenUseCase.PersistToken(userTokenDomain);
        }
        public async Task<UserTokenDomain> GetUserTokenAsync(int userId)
        {
            return await _getUserTokenUseCase.GetUserTokenAsync(userId);
        }
    }
}
