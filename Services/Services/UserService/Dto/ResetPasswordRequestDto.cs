﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.UserService.Dto
{
	public class ResetPasswordRequestDto
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }
	}
}
