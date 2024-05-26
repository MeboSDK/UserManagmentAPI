using Application.DTOs;
using Application.Interfaces;
using Domain.Abstractions;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Services;
public class UserService : IUserService
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

    public async Task<string> RegisterAsync(RegisterUserDto userDto)
    {
        var user = new User { UserName = userDto.UserName, Email = userDto.Email, FirstName = userDto.FirstName };
        var result = await _userManager.CreateAsync(user, userDto.Password);

        if (result.Succeeded)
        {
            if (!string.IsNullOrEmpty(userDto.Role))
            {
                await _userManager.AddToRoleAsync(user, userDto.Role);
            }

            await _unitOfWork.CommitAsync();
            return await _tokenService.GenerateTokenAsync(user);
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
            return await _tokenService.GenerateTokenAsync(user);
        }

        throw new Exception("Invalid credentials");
    }

    public async Task<UserDto> GetUserProfileAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) throw new Exception("User not found");

        return new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            UserName = user.UserName,
            Email = user.Email,
            // Map other properties as needed
        };
    }

    public async Task<bool> UpdateUserProfileAsync(string userId, UpdateUserDTO userDto)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) throw new Exception("User not found");

        user.UserName = userDto.UserName;
        user.Email = userDto.Email;
        // Update other properties as needed

        var result = await _userManager.UpdateAsync(user);

        if (result.Succeeded)
        {
            await _unitOfWork.CommitAsync();
            return true;
        }
        else
        {
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }
}
