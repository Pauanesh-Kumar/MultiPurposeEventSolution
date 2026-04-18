using App.Application.DTOs.Request;
using App.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDomain?> GetUserDetailsAsync(string emailAddress);
        Task<bool> RegisterUserAsync(RegisterUserDto registerUserDto);
    }
}
