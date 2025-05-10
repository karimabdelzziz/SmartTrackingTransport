
using Infrastructure.Interfaces;
using Infrastructure.Repos;
using Infrastucture.DbContexts;
using Infrastucture.IdentityEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Services.Services.BusService;
using Services.Services.IEmailService;
using Services.Services.TokenService;
using Services.Services.UserService;
using SmartTrackingTransport.Extensions;
using SmartTrackingTransport.Mappings;


namespace SmartTrackingTransport
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.AddServiceDefaults();

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddCors(options =>
            {
				options.AddPolicy("AllowAll", builder =>
			builder.AllowAnyOrigin()
				   .AllowAnyMethod()
				   .AllowAnyHeader());
			});
			// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
			builder.Services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });
			builder.Services.AddDbContext<TransportContext>(options =>
			{
				options.UseSqlServer(builder.Configuration.GetConnectionString("AppDbConnection"));
			});
			builder.Services.AddAutoMapper(typeof(MappingProfile));

			builder.Services.AddOpenApi();
            //builder.Services.AddSwaggerGen();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSwaggerDocumentation();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IUserService, UserService>();
			builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IBusRepository, BusRepository>();
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IBusService,BusService>();
			builder.Services.AddIdentityService(builder.Configuration);
			var app = builder.Build();

            app.MapDefaultEndpoints();

            // Configure the HTTP request pipeline.
           
                app.UseSwagger();
                app.UseSwaggerUI();
                
            
			app.UseCors("AllowAll");

			app.UseHttpsRedirection();
			app.UseAuthentication();

			app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
