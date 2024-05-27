using Domain.Abstractions;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Services
{
    public class UserRepository(UserManagmentDBContext dBContext) : Repository<User>(dBContext), IUserRepository
    {
        public async Task<User> GetByIdAsync(string userId)
        {
            return await dBContext.Users.FindAsync(userId);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await dBContext.Users.SingleOrDefaultAsync(u => u.Email == email);
        }
    }
}
