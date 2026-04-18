using App.Domain.Entities;
using App.Domain.Interfaces.Repositories;
using App.Infrastructure.Data.Entities;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<UserDomain, User>, IUserRepository
    {

        public UserRepository(ApplicationDBContext dBContext, IMapper mapper) : base(dBContext, mapper)
        {
           
        }
        public async Task<UserDomain> GetUserByEmailAsync(string emailAddress)
        {   
            var userDetails = await _dbContext.Users.Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Email == emailAddress);
            return _mapper.Map<UserDomain>(userDetails);
        }

        public async Task<int> AddUserAsync(UserDomain user)
        {
            var roles = await _dbContext.Roles.FirstOrDefaultAsync(x=> x.Name == user.UserRoles.First());
            if(roles != null)
            {   
                var userDetails = _mapper.Map<User>(user);
                userDetails.Roles.Add(roles);
                userDetails.CreatedDate = DateTime.UtcNow;    
                await _dbContext.Users.AddAsync(userDetails);
                return await _dbContext.SaveChangesAsync();
            }
            return await Task.FromResult(0);


        }
    }
}
