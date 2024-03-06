using GrpcService1.DataAccess;
using GrpcService1.Services;
using Microsoft.EntityFrameworkCore;

namespace GrpcService1
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddGrpc();
            builder.Services.AddDbContext<AppDbContext>(option =>
                option.UseSqlServer("Server=.;uid=sa;pwd=12345;Database=DemoGrpc;TrustServerCertificate=true"));

            var app = builder.Build();

            app.MapGrpcService<EmployeeService>();
            app.MapGrpcService<CategoryService>();
            app.MapGet("/", async context =>
            {
                await context.Response.WriteAsync("Communication thru gRPC");
            });

            app.Run();
        }
    }
}