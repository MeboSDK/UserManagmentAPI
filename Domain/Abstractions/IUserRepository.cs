using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstractions
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByIdAsync(string userId);
        Task<User> GetByEmailAsync(string email);
    }
}
