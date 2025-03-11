﻿using Infrastucture.IdentityEntities;
using Microsoft.AspNetCore.Identity;
using Services.Services.TokenService;
using Services.Services.UserService.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.UserService
{
	public class UserService : IUserService
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly ITokenService _tokenService;

		public UserService(UserManager<AppUser> userManager ,SignInManager<AppUser> signInManager , ITokenService tokenService) {
			_userManager = userManager;
			_signInManager = signInManager;
			_tokenService = tokenService;
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
	}
}
