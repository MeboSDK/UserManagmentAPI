using Application.DTOs;
using Application.Interfaces;
using Domain.Abstractions;
using Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Application.Services;
public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<User> _userValidator;
    public UserService(UserManager<User> userManager,
                       SignInManager<User> signInManager,
                       ITokenService tokenService, IUnitOfWork unitOfWork,
                       IValidator<User> validator)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _unitOfWork = unitOfWork;
        _userValidator = validator;
    }

    public async Task<string> RegisterAsync(RegisterUserDto userDto)
    {
        var user = new User { UserName = userDto.UserName, Email = userDto.Email, FirstName = userDto.FirstName };

        var validationResult = await _userValidator.ValidateAsync(user);

        if (!validationResult.IsValid)
            throw new InvalidDataException(string.Join(", ", validationResult.Errors));

        var result = await _userManager.CreateAsync(user, userDto.Password);
        if (result.Succeeded)
        {
            if (!string.IsNullOrEmpty(userDto.Role))
            {
                await _userManager.AddToRoleAsync(user, userDto.Role);
            }

            await _unitOfWork.CommitAsync();
            return _tokenService.GenerateToken(user, await _userManager.GetRolesAsync(user));
        }
        throw new ArgumentException(string.Join(", ", result.Errors.Select(e => e.Description)));
    }

    public async Task<string> LoginAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            throw new InvalidDataException("Invalid credentials");
        }

        var result = await _signInManager.PasswordSignInAsync(user.UserName, password, false, false);
        if (result.Succeeded)
        {
            return _tokenService.GenerateToken(user, await _userManager.GetRolesAsync(user));
        }

        throw new InvalidDataException("Invalid credentials");
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
            Role = string.Join(",", await _userManager.GetRolesAsync(user))
            // Map other properties as needed
        };
    }

    public async Task<bool> UpdateUserProfileAsync(string userId, UpdateUserDTO userDto)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) throw new Exception("User not found");

        user.FirstName = userDto.FirstName;
        user.UserName = userDto.UserName;
        user.Email = userDto.Email;

        var validationResult = await _userValidator.ValidateAsync(user);
        if (!validationResult.IsValid)
            throw new InvalidDataException(string.Join(", ", validationResult.Errors));

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
