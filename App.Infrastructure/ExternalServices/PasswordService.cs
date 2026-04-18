using App.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Infrastructure.ExternalServices
{
    public class PasswordService : IPasswordService
    {
        private readonly ILogger<PasswordService> _logger;

        public PasswordService(ILogger<PasswordService> logger)
        {
            _logger = logger;
        }

        public bool VerifyPassword(string password, string passwordHash)
        {
            try
            {
                bool isValid = BCrypt.Net.BCrypt.Verify(password, passwordHash);
                _logger.LogInformation($"Password verification for hash {passwordHash.Substring(0, 10)}... {(isValid ? "succeeded" : "failed")}");
                return isValid;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Password verification failed");
                return false;
            }
        }

        public string HashPassword(string password)
        {
            try
            {
                string hash = BCrypt.Net.BCrypt.HashPassword(password);
                _logger.LogInformation("Password hashed successfully");
                return hash;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Password hashing failed");
                throw;
            }
        }
    }
}