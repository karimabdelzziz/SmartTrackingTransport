using Infrastucture.Entities;
using Infrastucture.IdentityEntities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Services.Services.TokenService;
using Services.Services.UserService.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.UserService
{
	public class UserService : IUserService
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly ITokenService _tokenService;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public UserService(UserManager<AppUser> userManager ,SignInManager<AppUser> signInManager , ITokenService tokenService,IHttpContextAccessor httpContextAccessor) {
			_userManager = userManager;
			_signInManager = signInManager;
			_tokenService = tokenService;
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task<UserDto> CreateToken(AppUser appUser)
		{
			var token = _tokenService.CreateToken(appUser); 

			return new UserDto
			{
				DisplayName = appUser.DisplayName,
				Email = appUser.Email,
				Token = token
			};
		}

		public async Task<UserDto> GetCurrentUser()
		{
			var email = _httpContextAccessor.HttpContext?.User?.Claims
		.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

			if (string.IsNullOrEmpty(email))
				return null;

			var user = await _userManager.FindByEmailAsync(email);
			if (user == null)
				return null;

			return new UserDto
			{
				DisplayName = user.DisplayName,
				Email = user.Email,
				Token = _tokenService.CreateToken(user)
			};
		}

		public async Task<UserDto> LogIn(LogInDto logInDto)
		{
			var user = await _userManager.FindByEmailAsync(logInDto.Email);
			if (user == null)
				return null;
			var result =  await _signInManager.CheckPasswordSignInAsync(user, logInDto.Password,false);
			if (!result.Succeeded)
				return null;
			return new UserDto
			{
				DisplayName = user.DisplayName,
				Email = user.Email,
				Token = _tokenService.CreateToken(user)
			};
		}

		public async Task<UserDto> Register(RegisterDto registerDto)
		{
			var user = await _userManager.FindByEmailAsync(registerDto.Email);
			if (user != null)
				return null;

			var appUser = new AppUser
			{
				DisplayName = registerDto.DisplayName,
				Email = registerDto.Email,
				UserName = registerDto.Email.Split('@')[0]
			};
			var result = await _userManager.CreateAsync(appUser, registerDto.Password);

			if (!result.Succeeded)
				return null;
			return new UserDto
			{
				DisplayName = appUser.DisplayName,
				Email = appUser.Email,
				Token = _tokenService.CreateToken(appUser)
			};
		}

		public async Task<UserDto> UpdateCurrentUser(string? displayName, string? email)
		{
			var currentEmail = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);
			if (currentEmail == null) return null;

			var user = await _userManager.FindByEmailAsync(currentEmail);
			if (user == null) return null;

			if (!string.IsNullOrWhiteSpace(displayName))
				user.DisplayName = displayName;

			if (!string.IsNullOrWhiteSpace(email) && email != user.Email)
			{
				user.Email = email;
				user.UserName = email.Split('@')[0];
			}

			var result = await _userManager.UpdateAsync(user);
			if (!result.Succeeded) return null;

			return new UserDto
			{
				DisplayName = user.DisplayName,
				Email = user.Email,
				Token = _tokenService.CreateToken(user)
			};
		}

	}
}
