using App.Domain.Entities;
using App.Domain.Interfaces.Repositories;
using App.Domain.Interfaces.Services;
using App.Domain.UseCases.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.UseCases.User
{
    public class RegisterUserUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;

        public RegisterUserUseCase(IUserRepository userRepository, IPasswordService passwordService)
        {

            _userRepository = userRepository;
            _passwordService = passwordService;
        }
        public async Task<bool> RegisterUserAsync(UserDomain userDomain)
        {
            if (string.IsNullOrEmpty(userDomain.Email))
                throw new ArgumentException("Email is required.");
            var existingUser = await _userRepository.GetUserByEmailAsync(userDomain.Email);
            if (existingUser != null)
                throw new InvalidOperationException("User already registered. Please check the Email!");

            // Validate UserRoles
            if (userDomain.UserRoles == null || !userDomain.UserRoles.Any())
                throw new ArgumentException("At least one user role is required.");

            var validRoles = Enum.GetNames(typeof(RoleEnum));
            var invalidRoles = userDomain.UserRoles
                .Where(role => !validRoles.Contains(role, StringComparer.OrdinalIgnoreCase))
                .ToList();

            if (invalidRoles.Any())
                throw new ArgumentException($"Invalid user roles: {string.Join(", ", invalidRoles)}");

            int rowsInserted = await _userRepository.AddUserAsync(userDomain);

            return rowsInserted > 0;
        }
    }
}
