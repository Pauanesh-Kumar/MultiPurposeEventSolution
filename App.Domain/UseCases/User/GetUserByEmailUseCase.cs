using App.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.UseCases.User
{
    public class GetUserByEmailUseCase
    {
        private readonly IUserRepository _userRepository;

        public GetUserByEmailUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<Entities.UserDomain?> GetUserByEmailAsync(string emailAddress)
        {
            // Business logic for retrieving a user can be added here
            if (string.IsNullOrEmpty(emailAddress))
                throw new ArgumentException("Email is required.");

            var user = await _userRepository.GetUserByEmailAsync(emailAddress);
            if (user == null)
                throw new KeyNotFoundException("User not found.");
            return user;
        }
    }
}