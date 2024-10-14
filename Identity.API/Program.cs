using Identity.API.Database;
using Identity.API.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Authentication and Authorization
        builder.Services.AddAuthorization();
        builder.Services.AddAuthentication()
            .AddCookie(IdentityConstants.ApplicationScheme);

        // Identity and Entity Framework Core configuration
        builder.Services.AddIdentityCore<IdentityUser>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddApiEndpoints();

        // Fix typo in connection string key
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("Docker-Postgres")));

        var app = builder.Build();

        // Configure the HTTP request pipeline
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            // Apply migrations automatically in Development
            app.ApplyMigration();
        }

        app.UseHttpsRedirection();

        // Add authentication and authorization middleware
        app.UseAuthentication();
        app.UseAuthorization();

        // Map identity API endpoints
        app.MapIdentityApi<IdentityUser>();

        app.Run();
    }
}
