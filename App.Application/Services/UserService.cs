using App.Application.DTOs.Request;
using App.Application.Interfaces;
using App.Domain.Entities;
using App.Domain.UseCases.User;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Services
{
    public class UserService : IUserService
    {
        private readonly RegisterUserUseCase _registeruserUseCase;
        private readonly GetUserByEmailUseCase _getUserByEmailUseCase;
        private readonly IMapper _mapper;

        public UserService(
            RegisterUserUseCase registeruserUseCase,
            GetUserByEmailUseCase getUserByEmailUseCase,
            IMapper mapper
            )
        {
            this._registeruserUseCase = registeruserUseCase;
            this._getUserByEmailUseCase = getUserByEmailUseCase;
            this._mapper = mapper;
        }
        public async Task<bool> RegisterUserAsync(RegisterUserDto registerUserDto)
        {
            var userDomain = _mapper.Map<UserDomain>(registerUserDto);
            // Ensure password is not hashed by AutoMapper, assign plain text password from DTO
            //userDomain.Password = registerUserDto.Password;
            return await _registeruserUseCase.RegisterUserAsync(userDomain);
        }
        public async Task<UserDomain?> GetUserDetailsAsync(string emailAddress)
        {
            var user = await _getUserByEmailUseCase.GetUserByEmailAsync(emailAddress);
            return user;
        }

    }
}
