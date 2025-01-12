using Microsoft.AspNetCore.Mvc;
using TalabatApp.Core.Repository.Contract;
using TalabatApp.Errors;
using TalabatApp.Helpers;
using TalabatApp.Repository.Data;
using TalabatApp.Repository;
using TalabatApp.Repository.Repositories;
using Microsoft.EntityFrameworkCore;
using TalabatApp.MiddleWares;
using TalabatApp.Repository.Data.Identity;
using Microsoft.AspNetCore.Identity;
using TalabatApp.Core.Entities.Identity;

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
            services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));
            

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

        //public async static void UpdateDatabase( this WebApplication app)
        //{
            


        //}

        //public static WebApplication AddSwaggerServices(this WebApplication app)
        //{

           

        //    return app;
        //}

        //public static WebApplication AddMiddleWare(this WebApplication app)
        //{
           


        //    return app;
        //}




    }
}
