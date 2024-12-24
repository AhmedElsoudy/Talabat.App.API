using Microsoft.AspNetCore.Mvc;
using TalabatApp.Core.Repository.Contract;
using TalabatApp.Errors;
using TalabatApp.Helpers;
using TalabatApp.Repository.Data;
using TalabatApp.Repository;
using TalabatApp.Repository.Repositories;
using Microsoft.EntityFrameworkCore;
using TalabatApp.MiddleWares;

namespace TalabatApp.Extensions
{
    public static class ApiApplicationExtensions
    {
        public static IServiceCollection AddDependencyServices(this IServiceCollection services)
        {

            //builder.Services.AddScoped<IGenericRepository<Product>, GenericRepositories<Product>>();
            //builder.Services.AddScoped<IGenericRepository<ProductBrand>, GenericRepositories<ProductBrand>>();
            //builder.Services.AddScoped<IGenericRepository<ProductCategory>, GenericRepositories<ProductCategory>>();

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepositories<>));
            services.AddAutoMapper(typeof(MappingProfile));

            services.Configure<ApiBehaviorOptions>(options =>
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


            return services;
        }

        public async static void UpdateDatabase( this WebApplication app)
        {
            app.Services.GetRequiredService<ILogger<Program>>();
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var dbcontext = services.GetRequiredService<StoreContext>();

            var loggerFactor = services.GetRequiredService<ILoggerFactory>();

            try
            {
                await dbcontext.Database.MigrateAsync();
                await StoreContextSeeding.SeedAsync(dbcontext);

            }
            catch (Exception ex)
            {
                var logger = loggerFactor.CreateLogger<Program>();
                logger.LogError(ex, "There is Error in Migration Process");
            }


        }

        public static WebApplication AddSwaggerServices(this WebApplication app)
        {

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            return app;
        }

        public static WebApplication AddMiddleWare(this WebApplication app)
        {
            app.UseMiddleware<ExceptionMiddleware>();

            app.AddSwaggerServices();

            app.UseStatusCodePagesWithReExecute("/Errors/{0}"); // NotFound(EndPoint) Or UnAuthorized

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseStaticFiles();

            app.MapControllers();


            return app;
        }


    }
}
