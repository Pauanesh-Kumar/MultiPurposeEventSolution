using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Interfaces.Services
{
    public interface IPasswordService
    {
        bool VerifyPassword(string password, string passwordHash);
        string HashPassword(string password);
    }
}
