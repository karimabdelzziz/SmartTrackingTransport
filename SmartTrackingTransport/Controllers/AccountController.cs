using Infrastucture.IdentityEntities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Exceptions;
using NETCore.MailKit.Core;
using Refit;
using Services.Services.TokenService;
using Services.Services.UserService;
using Services.Services.UserService.Dto;
using Services.Services.IEmailService;
using SmartTrackingTransport.Extensions.ExceptionsHandler;
using System.Security.Claims;


namespace SmartTrackingTransport.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AccountController : ControllerBase
	{
		
		private readonly IUserService _userService;
		private readonly UserManager<AppUser> _userManager;
		private readonly Services.Services.IEmailService.IEmailService _emailService;

		public AccountController(IUserService userService , UserManager<AppUser> userManager,Services.Services.IEmailService.IEmailService emailService)
		{
			_userService = userService;
			_userManager = userManager;
			_emailService = emailService;
		}
		[HttpPost("register")]
		public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
		{
			var user = await _userService.Register(registerDto);
			if (user == null)
				return BadRequest(new CustomApiException(400, "Email Already Exist"));
			var appUser = await _userManager.FindByEmailAsync(user.Email);
			if (appUser == null) return BadRequest("User registration failed");
			return Ok(user);
		}
		[HttpPost("login")]
		public async Task<ActionResult<UserDto>> LogIn(LogInDto logInDto)
		{
			var user = await _userService.LogIn(logInDto);
			if (user == null)
				return Unauthorized(new CustomApiException(401, "you are not Authorized!!"));
			return Ok(user);
		}
		[HttpGet("getCurrentUser")]
		[Microsoft.AspNetCore.Authorization.Authorize]
		public async Task<ActionResult<UserDto>> GetCurrentUser()	
		{
			var email = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
			if (email == null) return Unauthorized("User not found");

			var user = await _userManager.FindByEmailAsync(email);
			if (user == null) return NotFound("User does not exist");

			return new UserDto
			{
				DisplayName = user.DisplayName,
				Email = user.Email
			};
		}
		[HttpPost("reset-password-request")]
		public async Task<IActionResult> ResetPasswordRequest([FromBody] ResetPasswordRequestDto dto)
		{
			var user = await _userManager.FindByEmailAsync(dto.Email);
			if (user == null) return NotFound("User not found");

			// Generate a password reset token
			var token = await _userManager.GeneratePasswordResetTokenAsync(user);


			//for testing
			Console.WriteLine("Generated Token: " + token);

			// Create email content
			var resetLink = $"https://localhost:7130/reset-password?email={dto.Email}&code={Uri.EscapeDataString(token)}";
			var emailBody = $@"
        <p>Hello,</p>
        <p>You requested a password reset. Click the link below to reset your password:</p>
        <a href='{resetLink}'>Reset Password</a>
        <p>If you did not request this, please ignore this email.</p>
    ";

			await _emailService.SendEmailAsync(dto.Email, "Password Reset Request", emailBody);

			return Ok("Password reset email sent");
		}
		[HttpPost("reset-password")]
		public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
		{
			var user = await _userManager.FindByEmailAsync(dto.Email);
			if (user == null) return NotFound("User not found");

			// Reset the password
			var result = await _userManager.ResetPasswordAsync(user, dto.Code, dto.NewPassword);
			if (!result.Succeeded)
			{
				return BadRequest(result.Errors);
			}

			return Ok("Password reset successfully");
		}
	}
}