using Application.DTOs;
using Application.Interfaces;
using Domain.Abstractions;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;
        public UserService(UserManager<User> userManager, 
                           SignInManager<User> signInManager, 
                           ITokenService tokenService, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> RegisterAsync(UserDto userDto, string password)
        {
            var user = new User { UserName = userDto.UserName, Email = userDto.Email };
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await _unitOfWork.CommitAsync();
                return _tokenService.GenerateToken(user);
            }
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new Exception("Invalid credentials");
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, password, false, false);
            if (result.Succeeded)
            {
                return _tokenService.GenerateToken(user);
            }

            throw new Exception("Invalid credentials");
        }
    }
}
