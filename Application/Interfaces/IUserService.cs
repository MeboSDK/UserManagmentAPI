using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<string> RegisterAsync(UserDto userDto, string password);
        Task<string> LoginAsync(string email, string password);
    }
}
