
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TalabatApp.Core.Entities;
using TalabatApp.Core.Repository.Contract;
using TalabatApp.Errors;
using TalabatApp.Extensions;
using TalabatApp.Helpers;
using TalabatApp.MiddleWares;
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

            builder.Services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddDependencyServices();


            var app = builder.Build();

            app.UpdateDatabase();


            // Configure the HTTP request pipeline.

            app.AddSwaggerServices();

            app.AddMiddleWare();


            app.Run();
        }
    }
}
