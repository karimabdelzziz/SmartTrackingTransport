﻿using Services.Services.UserService.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.UserService
{
	public interface IUserService
	{
		Task<UserDto> Register(RegisterDto registerDto);
		Task<UserDto> LogIn(LogInDto logInDto);
		//Task<UserDto> GetCurrentUser();
	}
}
