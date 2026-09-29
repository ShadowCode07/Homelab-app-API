using DotNetEnv;
using HomelabAPI.Application.Interfaces.Services;
using HomelabAPI.Application.Services;
using HomelabAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HomelabAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Env.TraversePath().Load();

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddControllers();

            builder.Services.AddScoped<IDeviceService, DeviceService>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
