
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Text;
using TalabatApp.Core.Entities;
using TalabatApp.Core.Entities.Identity;
using TalabatApp.Core.Repository.Contract;
using TalabatApp.Errors;
using TalabatApp.Extensions;
using TalabatApp.Helpers;
using TalabatApp.MiddleWares;
using TalabatApp.Repository;
using TalabatApp.Repository.Data;
using TalabatApp.Repository.Data.Identity;
using TalabatApp.Repository.Repositories;

namespace TalabatApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });

            builder.Services.AddSingleton<IConnectionMultiplexer>((serviceProvider) =>
            {
                var connect = builder.Configuration.GetConnectionString("RedisConnection");
                return ConnectionMultiplexer.Connect(connect);
            }
            );

            builder.Services.AddAuthentication().AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["JWT:ValidIssure"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWT:ValidAudience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:authKey"] ?? string.Empty))


                };
            });


            builder.Services.AddIdentity<AppUser, IdentityRole>()
                            .AddEntityFrameworkStores<AppIdentityDbContext>();


            builder.Services.AddAutoMapper(typeof(MappingProfile));

            builder.Services.AddDependencyServices();


            var app = builder.Build();

            app.Services.GetRequiredService<ILogger<Program>>();
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var dbcontext = services.GetRequiredService<StoreContext>();
            var userManager = services.GetRequiredService<UserManager<AppUser>>();
            var identityDbcontext = services.GetRequiredService<AppIdentityDbContext>();

            var loggerFactor = services.GetRequiredService<ILoggerFactory>();

            try
            {
                await dbcontext.Database.MigrateAsync();
                await StoreContextSeeding.SeedAsync(dbcontext);
                await AppIdentityDbContextSeed.SeedAsync(userManager);
                await identityDbcontext.Database.MigrateAsync();
              


            }
            catch (Exception ex)
            {
                var logger = loggerFactor.CreateLogger<Program>();
                logger.LogError(ex, "There is Error in Migration Process");
            }
            //app.UpdateDatabase();


            // Configure the HTTP request pipeline.

            //app.AddSwaggerServices();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.AddMiddleWare();
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseStatusCodePagesWithReExecute("/Errors/{0}"); // NotFound(EndPoint) Or UnAuthorized

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseStaticFiles();

            app.MapControllers();


            app.Run();
        }
    }
}
