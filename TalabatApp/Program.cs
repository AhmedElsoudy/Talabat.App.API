
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TalabatApp.Core.Entities;
using TalabatApp.Core.Repository.Contract;
using TalabatApp.Errors;
using TalabatApp.Helpers;
using TalabatApp.Repository;
using TalabatApp.Repository.Data;
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

            //builder.Services.AddScoped<IGenericRepository<Product>, GenericRepositories<Product>>();
            //builder.Services.AddScoped<IGenericRepository<ProductBrand>, GenericRepositories<ProductBrand>>();
            //builder.Services.AddScoped<IGenericRepository<ProductCategory>, GenericRepositories<ProductCategory>>();
          
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepositories<>));
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            builder.Services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actioncontext) =>
                {
                    var errors = actioncontext.ModelState.Where(P => P.Value.Errors.Count() > 0)
                                                         .SelectMany(P => P.Value.Errors)
                                                         .Select(E => E.ErrorMessage)
                                                         .ToList();

                    var response = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(response);

                };
            });

            var app = builder.Build();

            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var dbcontext = services.GetRequiredService<StoreContext>();

            var loggerFactor = services.GetRequiredService<ILoggerFactory>();

            try
            {
                await dbcontext.Database.MigrateAsync();
                await StoreContextSeeding.SeedAsync(dbcontext);

            }catch(Exception ex)
            {
                var logger = loggerFactor.CreateLogger<Program>();
                logger.LogError(ex, "There is Error in Migration Process");
            }



            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseStaticFiles();

            app.MapControllers();

            app.Run();
        }
    }
}
